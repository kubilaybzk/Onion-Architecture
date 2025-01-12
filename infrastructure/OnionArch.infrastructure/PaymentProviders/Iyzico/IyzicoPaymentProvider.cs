using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
using OnionArch.infrastructure.PaymentProviders.Common;
using System.Text.Json;
using System.Globalization;

namespace OnionArch.infrastructure.PaymentProviders.Iyzico
{
    public class IyzicoPaymentProvider : PaymentProviderBase, IPaymentProvider
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public string ProviderName => "iyzico";

        public IyzicoPaymentProvider(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _apiKey = configuration["Payment:Iyzico:ApiKey"] ?? throw new Exception("Iyzico ApiKey is missing");
            _secretKey = configuration["Payment:Iyzico:SecretKey"] ?? throw new Exception("Iyzico SecretKey is missing");
            _baseUrl = configuration["Payment:Iyzico:BaseUrl"] ?? throw new Exception("Iyzico BaseUrl is missing");
        }

        /*
             İyzico ile ödeme işlemini başlatır
             Kredi kartı bilgileriyle ödeme yapar
             3D yada normal ödeme seçeneğine göre işlem yapar
         */
        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                var options = new Options
                {
                    ApiKey = _apiKey,
                    SecretKey = _secretKey,
                    BaseUrl = _baseUrl
                };

                var iyzipayRequest = new CreatePaymentRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = request.OrderId,
                    Price = "1"/* request.Amount.ToString(CultureInfo.InvariantCulture)*/,
                    PaidPrice = "1" /* request.Amount.ToString(CultureInfo.InvariantCulture),*/,
                    Currency = Currency.TRY.ToString(),
                    Installment = 1,
                    BasketId = request.OrderId,
                    PaymentChannel = PaymentChannel.WEB.ToString(),
                    PaymentGroup = PaymentGroup.PRODUCT.ToString(),

                    PaymentCard = new PaymentCard
                    {
                        CardHolderName = request.CardHolderName,
                        CardNumber = request.CardNumber,
                        ExpireMonth = request.ExpirationMonth,
                        ExpireYear = request.ExpirationYear,
                        Cvc = request.Cvc,
                        RegisterCard = 0
                    },

                    Buyer = new Buyer
                    {
                        Id = "BY789", // Test için sabit değer
                        Name = "John",
                        Surname = "Doe",
                        Email = "email@email.com",
                        IdentityNumber = "74300864791",
                        RegistrationAddress = "Test Address",
                        City = "Istanbul",
                        Country = "Turkey",
                        ZipCode = "34732",
                        Ip = "127.0.0.1"
                    },

                    ShippingAddress = new Address
                    {
                        ContactName = "John Doe",
                        City = "Istanbul",
                        Country = "Turkey",
                        Description = "Test Address",
                        ZipCode = "34732"
                    },

                    BillingAddress = new Address
                    {
                        ContactName = "John Doe",
                        City = "Istanbul",
                        Country = "Turkey",
                        Description = "Test Address",
                        ZipCode = "34732"
                    },

                    BasketItems = new List<BasketItem>
                   {
                       new BasketItem
                       {
                           Id = "BI101",
                           Name = "Test Product",
                           Category1 = "Test Category",
                           ItemType = BasketItemType.PHYSICAL.ToString(),
                           Price = "1"

                       }
                   }
                };

                if (request.Use3D)
                {
                    iyzipayRequest.CallbackUrl = $"{_configuration["BaseUrl"]}/api/payment/complete-3d";
                    var threeDPayment = await ThreedsInitialize.Create(iyzipayRequest, options);

                    if (threeDPayment.Status == Status.SUCCESS.ToString())
                    {
                        return new PaymentResponse
                        {
                            Success = true,
                            RedirectUrl = threeDPayment.HtmlContent,
                            TransactionId = threeDPayment.ConversationId,
                            ProviderResponse = JsonSerializer.Serialize(threeDPayment)
                        };
                    }

                    return new PaymentResponse
                    {
                        Success = false,
                        ErrorCode = threeDPayment.ErrorCode,
                        ErrorMessage = threeDPayment.ErrorMessage
                    };
                }
                else
                {
                    var payment = await Payment.Create(iyzipayRequest, options);

                    if (payment.Status == Status.SUCCESS.ToString())
                    {
                        return new PaymentResponse
                        {
                            Success = true,
                            TransactionId = payment.PaymentId,
                            ProviderResponse = JsonSerializer.Serialize(payment)
                        };
                    }

                    return new PaymentResponse
                    {
                        Success = false,
                        ErrorCode = payment.ErrorCode,
                        ErrorMessage = payment.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_ERROR"
                };
            }
        }

        /*
             3D doğrulama sonrası gelen yanıtı işler
             3D sürecini tamamlar
             Banka ekranından dönen sonucu değerlendirir 
         */
        public async Task<PaymentResponse> ProcessThreeDPaymentAsync(string paymentId, string threeDResponse)
        {
            try
            {
                var options = new Options
                {
                    ApiKey = _apiKey,
                    SecretKey = _secretKey,
                    BaseUrl = _baseUrl
                };

                var request = new CreateThreedsPaymentRequest
                {
                    ConversationId = paymentId,
                    PaymentId = paymentId,
                    ConversationData = threeDResponse
                };

                var payment = await ThreedsPayment.Create(request, options);

                if (payment.Status == Status.SUCCESS.ToString())
                {
                    return new PaymentResponse
                    {
                        Success = true,
                        TransactionId = payment.PaymentId,
                        ProviderResponse = JsonSerializer.Serialize(payment)
                    };
                }

                return new PaymentResponse
                {
                    Success = false,
                    ErrorCode = payment.ErrorCode,
                    ErrorMessage = payment.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_3D_ERROR"
                };
            }
        }
        /*
            İyzico üzerinden ödemeyi iptal eder
            İade olmadan direkt iptal işlemi
        */
        public async Task<PaymentResponse> CancelPaymentAsync(string transactionId)
        {
            try
            {
                var options = new Options
                {
                    ApiKey = _apiKey,
                    SecretKey = _secretKey,
                    BaseUrl = _baseUrl
                };

                var request = new CreateCancelRequest
                {
                    ConversationId = transactionId,
                    PaymentId = transactionId,
                    Ip = "127.0.0.1"
                };

                var cancel = await Cancel.Create(request, options);

                if (cancel.Status == Status.SUCCESS.ToString())
                {
                    return new PaymentResponse
                    {
                        Success = true,
                        TransactionId = cancel.PaymentId
                    };
                }

                return new PaymentResponse
                {
                    Success = false,
                    ErrorCode = cancel.ErrorCode,
                    ErrorMessage = cancel.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_CANCEL_ERROR"
                };
            }
        }
        /*
            Yapılmış bir ödemeyi iade eder
            Kısmi veya tam iade yapılabilir
        */
        public async Task<PaymentResponse> RefundPaymentAsync(string transactionId, decimal amount)
        {
            try
            {
                var options = new Options
                {
                    ApiKey = _apiKey,
                    SecretKey = _secretKey,
                    BaseUrl = _baseUrl
                };

                var request = new CreateRefundRequest
                {
                    ConversationId = transactionId,
                    PaymentTransactionId = transactionId,
                    Price = amount.ToString(CultureInfo.InvariantCulture),
                    Currency = Currency.TRY.ToString(),
                    Ip = "127.0.0.1"
                };

                var refund = await Refund.Create(request, options);

                if (refund.Status == Status.SUCCESS.ToString())
                {
                    return new PaymentResponse
                    {
                        Success = true,
                        TransactionId = refund.PaymentId
                    };
                }

                return new PaymentResponse
                {
                    Success = false,
                    ErrorCode = refund.ErrorCode,
                    ErrorMessage = refund.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_REFUND_ERROR"
                };
            }
        }
    }
}