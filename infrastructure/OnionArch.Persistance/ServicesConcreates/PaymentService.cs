using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
using OnionArch.Application.Repositories.PaymentTransactionCrud;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Domain.Entities;
using OnionArch.Domain.Enums;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnionArch.Application.Abstractions.OrderCrud;
using OnionArch.Application.Abstractions.BasketServices;
using OnionArch.Persistance.Operations;
using OnionArch.Application.Abstractions.HubServices;
using Newtonsoft.Json;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentTransactionReadRepository _paymentTransactionReadRepository;
        private readonly IPaymentTransactionWriteRepository _paymentTransactionWriteRepository;
        private readonly IPaymentFactory _paymentFactory;
        private readonly OnionArchDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IBasketService _basketService;
        private readonly IOrderHubService _orderHubService;

        public PaymentService(
            IPaymentTransactionReadRepository paymentTransactionReadRepository,
            IPaymentTransactionWriteRepository paymentTransactionWriteRepository,
            IPaymentFactory paymentFactory,
            OnionArchDBContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IOrderReadRepository orderReadRepository,
            IBasketService basketService,
            IOrderHubService orderHubService)
        {
            _paymentTransactionReadRepository = paymentTransactionReadRepository;
            _paymentTransactionWriteRepository = paymentTransactionWriteRepository;
            _paymentFactory = paymentFactory;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _orderReadRepository = orderReadRepository;
            _basketService = basketService;
            _orderHubService = orderHubService;
        }

        public async Task<PaymentTransaction> CreatePaymentTransactionAsync(string orderId, PaymentRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _orderReadRepository.Table
                    .AsSplitQuery()
                    .Include(o => o.User)
                        .ThenInclude(user => user.Addresses)
                    .Include(o => o.Basket)
                        .ThenInclude(basket => basket.BasketItems)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Categorys)
                    .FirstOrDefaultAsync(o => o.ID == Guid.Parse(request.OrderId));

                if (order == null)
                    throw new Exception("Sipariş bulunamadı");

                request.OrderInformation = order;

                var providerName = _configuration["Payment:Provider"];
                var provider = _paymentFactory.CreateProvider(providerName);

                var paymentTransaction = new PaymentTransaction
                {
                    OrderId = Guid.Parse(orderId),
                    UserId = order.UserId,
                    User = order.User,
                    Order = order,
                    Status = PaymentStatus.Created,
                    IsThreeD = request.Use3D,
                    CardNumber = MaskCreditCard(request.CardNumber),
                    CardHolder = request.CardHolderName,
                    ErrorCode = "INIT_",
                    ErrorMessage = "INIT_",
                    ConversationId = $"INIT_{Guid.NewGuid():N}",
                    PaymentProvider = providerName,
                    SystemTime = DateTime.UtcNow,
                    PaymentId = "INIT_",
                    Price = 0,
                    PaidPrice = 0,
                    Currency = request.Currency,
                    Installment = request.Installment,
                    PaymentStatus = "PENDING",
                    FraudStatus = 0,
                    ProviderCommissionFee = 0,
                    ProviderCommissionRateAmount = 0
                };

                await _paymentTransactionWriteRepository.AddAsync(paymentTransaction);
                var response = await provider.ProcessPaymentAsync(request);

                if (request.Use3D && !string.IsNullOrEmpty(response.ProviderResponse))
                {
                    paymentTransaction.PaymentId = response.PaymentId;
                    paymentTransaction.ConversationId = response.ConversationId;
                    paymentTransaction.ProviderResponse = response.ProviderResponse;
                    paymentTransaction.Status = PaymentStatus.Processing;
                }
                else
                {
                    paymentTransaction.Price = ParseDecimal(response.Price);
                    paymentTransaction.PaidPrice = ParseDecimal(response.PaidPrice);
                    paymentTransaction.ProviderCommissionFee = ParseDecimal(response.IyziCommissionFee);
                    paymentTransaction.ProviderCommissionRateAmount = ParseDecimal(response.IyziCommissionRateAmount);

                    paymentTransaction.Status = response.Status == "success" ? PaymentStatus.Success : PaymentStatus.Failed;
                    paymentTransaction.Order.Status = response.Status == "success" ? OrderStatus.Completed : OrderStatus.Failed;
                    paymentTransaction.ErrorCode = response.ErrorCode ?? "No Error Code";
                    paymentTransaction.ErrorMessage = response.ErrorMessage ?? "No Error Message";
                    paymentTransaction.ProviderResponse = response.ProviderResponse ?? "No Provider Response";
                    paymentTransaction.SystemTime = DateTimeOffset.FromUnixTimeMilliseconds(response.SystemTime).ToUniversalTime();
                    paymentTransaction.PaymentId = response.PaymentId;
                    paymentTransaction.ConversationId = response.ConversationId;
                    paymentTransaction.Currency = response.Currency ?? paymentTransaction.Currency;
                    paymentTransaction.Installment = response.Installment > 0 ? response.Installment : 0;
                    paymentTransaction.PaymentStatus = response.PaymentStatus ?? response.Status;
                    paymentTransaction.FraudStatus = response.FraudStatus;

                    paymentTransaction.Order.DiscountCoupon = paymentTransaction.Order.Basket.DiscountCoupon;
                    paymentTransaction.Order.DiscountCouponId = paymentTransaction.Order.Basket.DiscountCouponId;
                    paymentTransaction.Order.DiscountedAmount = paymentTransaction.Order.Basket.DiscountedAmount;
                    paymentTransaction.Order.Basket.isDeleted = true;
                }

                await _paymentTransactionWriteRepository.SaveAsync();

                if (response.Status == "success")
                {
                    order.isOrdered = true;
                    order.paidStatus = true;
                    order.Status = OrderStatus.Processing;

                    if (!request.Use3D)
                    {
                        await _basketService.ClearBasketAsync(order.Basket.ID);
                        await _orderHubService.OrderAddedMessageAsync(
                            string.Format(CultureInfo.GetCultureInfo("tr-TR"),
                            "Yeni sipariş: {0:N2} ₺",
                            paymentTransaction.PaidPrice)
                        );
                    }
                }

                await transaction.CommitAsync();
                return paymentTransaction;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PaymentTransaction> ProcessPaymentAsync(string transactionId)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.GetByIdAsync(transactionId);
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            var order = await _context.Orders.FindAsync(paymentTransaction.OrderId);
            if (order == null)
                throw new Exception("Sipariş bulunamadı");

            return paymentTransaction;
        }

        public async Task<PaymentTransaction> CompleteThreeDPaymentAsync(string paymentId, string ConversationData)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.Table
                .Include(pt => pt.Order)
                .ThenInclude(ord => ord.Basket)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            var provider = _paymentFactory.CreateProvider(paymentTransaction.PaymentProvider);
            var response = await provider.ProcessThreeDPaymentAsync(paymentTransaction.PaymentId, paymentTransaction.ConversationId, ConversationData);

            await UpdateTransactionWithResponse(paymentTransaction, response);

            if (response.Status == "success")
            {
                await _basketService.ClearBasketAsync(paymentTransaction.Order.Basket.ID);
                await _orderHubService.OrderAddedMessageAsync(
                    string.Format(CultureInfo.GetCultureInfo("tr-TR"),
                    "Yeni sipariş: {0:N2} ₺",
                    paymentTransaction.PaidPrice)
                );
            }

            return paymentTransaction;
        }

        public async Task<PaymentTransaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _paymentTransactionReadRepository.Table
                .Include(pt => pt.Order)
                .Include(pt => pt.User)
                .FirstOrDefaultAsync(pt => pt.ID == Guid.Parse(transactionId));
        }

        public async Task<List<PaymentTransaction>> GetOrderTransactionsAsync(string orderId)
        {
            return await _paymentTransactionReadRepository.Table
                .Where(pt => pt.OrderId.ToString() == orderId)
                .OrderByDescending(pt => pt.CreateTime)
                .ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetUserTransactionsAsync(string userId)
        {
            return await _paymentTransactionReadRepository.Table
                .Include(pt => pt.Order)
                .Where(pt => pt.UserId == userId)
                .OrderByDescending(pt => pt.CreateTime)
                .ToListAsync();
        }

        public async Task UpdateTransactionStatusAsync(string transactionId, PaymentStatus status, string errorMessage = null)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.GetByIdAsync(transactionId);
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            paymentTransaction.Status = status;
            paymentTransaction.ErrorMessage = errorMessage;

            await _paymentTransactionWriteRepository.SaveAsync();
        }

        private async Task UpdateTransactionWithResponse(PaymentTransaction paymentTransaction, PaymentResponse response)
        {
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            paymentTransaction.Price = ParseDecimal(response.Price);
            paymentTransaction.PaidPrice = ParseDecimal(response.PaidPrice);
            paymentTransaction.ProviderCommissionFee = ParseDecimal(response.IyziCommissionFee);
            paymentTransaction.ProviderCommissionRateAmount = ParseDecimal(response.IyziCommissionRateAmount);

            paymentTransaction.Status = response.Status == "success" ? PaymentStatus.Success : PaymentStatus.Failed;
            paymentTransaction.Order.Status = response.Status == "success" ? OrderStatus.Completed : OrderStatus.Failed;
            paymentTransaction.ErrorCode = response.ErrorCode ?? "No Error Code";
            paymentTransaction.ErrorMessage = response.ErrorMessage ?? "No Error Message";
            paymentTransaction.ProviderResponse = response.ProviderResponse ?? "No Provider Response";
            paymentTransaction.SystemTime = DateTimeOffset.FromUnixTimeMilliseconds(response.SystemTime).ToUniversalTime();
            paymentTransaction.PaymentId = response.PaymentId;
            paymentTransaction.ConversationId = response.ConversationId;
            paymentTransaction.Currency = response.Currency ?? paymentTransaction.Currency;
            paymentTransaction.Installment = response.Installment > 0 ? response.Installment : 1;
            paymentTransaction.PaymentStatus = response.PaymentStatus ?? response.Status;
            paymentTransaction.FraudStatus = response.FraudStatus;

            await _paymentTransactionWriteRepository.SaveAsync();
        }

        private string MaskCreditCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return string.Empty;

            var lastFourDigits = cardNumber.Substring(cardNumber.Length - 4);
            return new string('*', cardNumber.Length - 4) + lastFourDigits;
        }

        private decimal ParseDecimal(object value)
        {
            if (value == null)
                return 0;

            // Decimal değeri string olarak al
            string stringValue = value.ToString();

            // Binlik ayraçları temizle (örneğin, "1.999,99" → "1999.99")
            stringValue = stringValue.Replace(",", "").Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            if (decimal.TryParse(stringValue,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out decimal result))
            {
                return result;
            }

            return 0;
        }

        public async Task<PaymentBinNumberDTO> GetPaymentBinNumberAsync(string cardNumber)
        {
            var providerName = _configuration["Payment:Provider"];
            var provider = _paymentFactory.CreateProvider(providerName);
            var response = await provider.CheckBinNumber(cardNumber);
             
            return new PaymentBinNumberDTO()
            {
                Status = response.Status,
                BinNumber = response.BinNumber,
                BankName = response.BankName,
                CardType = response.CardType,
                CardAssociation = response.CardAssociation,
                CardFamily = response.CardFamily,
                Commerical = response.Commerical,
                
            };
        }

        public async Task<PaymentInstamentDTO> GetPaymentInstallment(string cardNumber, string paidPrice)
        {
            var providerName = _configuration["Payment:Provider"];
            var provider = _paymentFactory.CreateProvider(providerName);
            var response = await provider.GetBasketInstament(cardNumber, paidPrice);

            return response;
        }
    }
}