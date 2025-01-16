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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public PaymentService(
            IPaymentTransactionReadRepository paymentTransactionReadRepository,
            IPaymentTransactionWriteRepository paymentTransactionWriteRepository,
            IPaymentFactory paymentFactory,
            OnionArchDBContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _paymentTransactionReadRepository = paymentTransactionReadRepository;
            _paymentTransactionWriteRepository = paymentTransactionWriteRepository;
            _paymentFactory = paymentFactory;
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        /*
            Ödeme kaydını veritabanında oluşturur
            Seçili provider'a (iyzico/sipay) ödeme isteği gönderir
            Sonucu veritabanına kaydeder
        */
        public async Task<PaymentTransaction> CreatePaymentTransactionAsync(string orderId, PaymentRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.ID == Guid.Parse(orderId));

                if (order == null)
                    throw new Exception("Sipariş bulunamadı");

                var providerName = _configuration["Payment:Provider"];
                var provider = _paymentFactory.CreateProvider(providerName);

                //Başlangıç olarak bir payment değeri set ediyoruz ki ilerleyen süreçte dbden okuyabilelim.
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
                    Installment = 0,
                    PaymentStatus = "PENDING",
                    FraudStatus = 0,  // 0 = kontrol edilecek
                    ProviderCommissionFee = 0,
                    ProviderCommissionRateAmount = 0
                };

                await _paymentTransactionWriteRepository.AddAsync(paymentTransaction);

                // Ödeme işlemini başlat
                var response = await provider.ProcessPaymentAsync(request);



                // 3D için HTML içeriğini kaydet
                if (request.Use3D && !string.IsNullOrEmpty(response.ProviderResponse))
                {
                    paymentTransaction.PaymentId = response.PaymentId;
                    paymentTransaction.ConversationId = response.ConversationId;
                    paymentTransaction.ProviderResponse = response.ProviderResponse;
                    paymentTransaction.Status = PaymentStatus.Processing;
                }
                else
                {
                    // Normal ödeme sonuçlarını güncelle
                    paymentTransaction.Status = response.Status == "success" ? PaymentStatus.Success : PaymentStatus.Failed;
                    paymentTransaction.ErrorCode = response.ErrorCode ?? "No Error Code";
                    paymentTransaction.ErrorMessage = response.ErrorMessage ?? "No Error Message";
                    paymentTransaction.ProviderResponse = response.ProviderResponse ?? "No Provider Response";
                    paymentTransaction.SystemTime = DateTimeOffset.FromUnixTimeMilliseconds(response.SystemTime).ToUniversalTime();

                    // Null olabilen decimal değerler için 0 kullan
                    paymentTransaction.Price = response.Price > 0 ? response.Price : paymentTransaction.Price;
                    paymentTransaction.PaidPrice = response.PaidPrice > 0 ? response.PaidPrice : 0;
                    paymentTransaction.Currency = !string.IsNullOrEmpty(response.Currency) ? response.Currency : paymentTransaction.Currency;
                    paymentTransaction.Installment = response.Installment > 0 ? response.Installment : 1;
                    paymentTransaction.PaymentStatus = !string.IsNullOrEmpty(response.PaymentStatus) ? response.PaymentStatus : response.Status;
                    paymentTransaction.FraudStatus = response.FraudStatus != null ? response.FraudStatus : 0;

                    // Commission değerleri için null check
                    paymentTransaction.ProviderCommissionFee = response.IyziCommissionFee > 0 ? response.IyziCommissionFee : 0;
                    paymentTransaction.ProviderCommissionRateAmount = response.IyziCommissionRateAmount > 0 ? response.IyziCommissionRateAmount : 0;
                    paymentTransaction.ProviderResponse = response.ProviderResponse;
                }

                await _paymentTransactionWriteRepository.SaveAsync();

                if (response.Status == "success")
                {
                    order.Status = OrderStatus.Processing;
                    await _context.SaveChangesAsync();
                }
                else
                {
                   order.Status = OrderStatus.Processing;
                   await _context.SaveChangesAsync();
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

        /*
            Var olan ödeme kaydını sorgular
            Yeni ödeme işlemi başlatmaz
            Sadece durum kontrolü yapar
        */
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

        /*
            3D ödemelerini tamamlar
            Banka ekranından dönen kullanıcıyı işler
        */
        public async Task<PaymentTransaction> CompleteThreeDPaymentAsync(string paymentId, string ConversationData)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.Table.Where(p=>p.PaymentId==paymentId).FirstOrDefaultAsync();
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            var provider = _paymentFactory.CreateProvider(paymentTransaction.PaymentProvider);
            var response = await provider.ProcessThreeDPaymentAsync(paymentTransaction.PaymentId,paymentTransaction.ConversationId, ConversationData);

            await UpdateTransactionWithResponse(paymentId, response);

            return paymentTransaction;
        }

        /*
           Belirli bir ödeme kaydının detaylarını getirir
        */
        public async Task<PaymentTransaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _paymentTransactionReadRepository.Table
                .Include(pt => pt.Order)
                .Include(pt => pt.User)
                .FirstOrDefaultAsync(pt => pt.ID == Guid.Parse(transactionId));
        }

        /*
          Bir siparişe ait tüm ödemeleri listeler
        */
        public async Task<List<PaymentTransaction>> GetOrderTransactionsAsync(string orderId)
        {
            return await _paymentTransactionReadRepository.Table
                .Where(pt => pt.OrderId.ToString() == orderId)
                .OrderByDescending(pt => pt.CreateTime)
                .ToListAsync();
        }

        /*
           Bir kullanıcının tüm ödeme geçmişini getirir
        */
        public async Task<List<PaymentTransaction>> GetUserTransactionsAsync(string userId)
        {
            return await _paymentTransactionReadRepository.Table
                .Include(pt => pt.Order)
                .Where(pt => pt.UserId == userId)
                .OrderByDescending(pt => pt.CreateTime)
                .ToListAsync();
        }

        /*
           Ödeme durumunu günceller
        */
        public async Task UpdateTransactionStatusAsync(string transactionId, PaymentStatus status, string errorMessage = null)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.GetByIdAsync(transactionId);
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            paymentTransaction.Status = status;
            paymentTransaction.ErrorMessage = errorMessage;

            await _paymentTransactionWriteRepository.SaveAsync();
        }

        /*
             Provider'dan gelen yanıtla ödeme kaydını günceller
        */
        private async Task UpdateTransactionWithResponse(string paymentId, PaymentResponse response)
        {
            var paymentTransaction = await _paymentTransactionReadRepository.Table.Where(p => p.PaymentId == paymentId).FirstOrDefaultAsync();
            if (paymentTransaction == null)
                throw new Exception("Ödeme işlemi bulunamadı");

            // Normal ödeme sonuçlarını güncelle
            paymentTransaction.Status = response.Status == "success" ? PaymentStatus.Success : PaymentStatus.Failed;
            paymentTransaction.ErrorCode = response.ErrorCode ?? "No Error Code";
            paymentTransaction.ErrorMessage = response.ErrorMessage ?? "No Error Message";
            paymentTransaction.ProviderResponse = response.ProviderResponse ?? "No Provider Response";
            paymentTransaction.SystemTime = DateTimeOffset.FromUnixTimeMilliseconds(response.SystemTime).ToUniversalTime();

            // Null olabilen decimal değerler için 0 kullan
            paymentTransaction.Price = response.Price > 0 ? response.Price : paymentTransaction.Price;
            paymentTransaction.PaidPrice = response.PaidPrice > 0 ? response.PaidPrice : 0;
            paymentTransaction.Currency = !string.IsNullOrEmpty(response.Currency) ? response.Currency : paymentTransaction.Currency;
            paymentTransaction.Installment = response.Installment > 0 ? response.Installment : 1;
            paymentTransaction.PaymentStatus = !string.IsNullOrEmpty(response.PaymentStatus) ? response.PaymentStatus : response.Status;
            paymentTransaction.FraudStatus = response.FraudStatus != null ? response.FraudStatus : 0;

            // Commission değerleri için null check
            paymentTransaction.ProviderCommissionFee = response.IyziCommissionFee > 0 ? response.IyziCommissionFee : 0;
            paymentTransaction.ProviderCommissionRateAmount = response.IyziCommissionRateAmount > 0 ? response.IyziCommissionRateAmount : 0;
            paymentTransaction.ProviderResponse = response.ProviderResponse;

            await _paymentTransactionWriteRepository.SaveAsync();
        }

        private string MaskCreditCard(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return string.Empty;

            // Sadece son 4 haneyi göster
            var lastFourDigits = cardNumber.Substring(cardNumber.Length - 4);
            var maskedPart = new string('*', cardNumber.Length - 4);
            return maskedPart + lastFourDigits;
        }
    }
}