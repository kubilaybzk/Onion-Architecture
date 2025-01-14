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
                            Status = Status.SUCCESS.ToString(),
                            ProviderResponse = JsonSerializer.Serialize(threeDPayment),
                            ThreeDSHtmlContent=threeDPayment.HtmlContent,
                            ConversationId=threeDPayment.ConversationId,
                            ErrorCode = threeDPayment.ErrorCode,  
                            ErrorMessage = threeDPayment.ErrorMessage,
                            ErrorGroup = threeDPayment.ErrorGroup,
                            Locale =threeDPayment.Locale,
                            PaymentId = threeDPayment.PaymentId,
                            SystemTime = threeDPayment.SystemTime,
                            
                        };
                    }

                    return new PaymentResponse
                    {
                        Status = Status.FAILURE.ToString(),
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
                            Status = payment.Status,        //Yapılan isteğin sonucunu bildirir. İşlem başarılı ise success, hatalı ise failure döner.

                            ErrorCode = payment.ErrorCode,   //İşlem hatalıysa, bu hataya dair belirtilen koddur.

                            ErrorMessage = payment.ErrorMessage, //işlem hatalıysa, bu hataya dair belirtilen mesajdır

                            ErrorGroup = payment.ErrorGroup, //işlem hatalıysa, bu hataya dair belirtilen gruptur.

                            Locale = payment.Locale, //İstekte belirtilen locale değeri geri dönülür, varsayılan değeri tr’dir.

                            SystemTime = payment.SystemTime, //Dönen sonucun o anki unix timestamp değeridir.

                            ConversationId = payment.ConversationId, //sipariş numarasıdır.

                            PaymentId = payment.PaymentId, //Ödemeye ait id, üye işyeri tarafından mutlaka saklanmalıdır. 

                            Price = decimal.Parse(payment.Price), //Ödeme sepet tutarı.

                            PaidPrice = decimal.Parse(payment.PaidPrice), //İndirim vade farkı vs. hesaplanmış POS’tan geçen, tahsil edilen, nihai tutar.

                            Currency = payment.Currency, //Ödeme alındığı para birimi,

                            Installment = (int)payment.Installment, //Taksit bilgisi.

                            PaymentStatus = payment.PaymentStatus, // SUCCESS, FAILURE, INIT_THREEDS, CALLBACK_THREEDS, BKM_POS_SELECTED, CALLBACK_PECCO

                            BasketId = payment.BasketId, //Sepetin id değeri

                            BinNumber = payment.BinNumber,    //Kartın bin numarası,

                            CardAssociation = payment.CardAssociation, //Eğer ödeme yapılan kart yerel bir kart ise, kartın ait olduğu kuruluş. 

                            CardFamily = payment.CardFamily, //yerel bir kart ise, kartın ait olduğu aile. Geçerli değerler: Bonus, Axess, World, Maximum, Paraf, CardFinans, Advantage

                            CardType = payment.CardType, //Geçerli değerler: CREDIT_CARD, DEBIT_CARD, PREPAID_CARD

                            FraudStatus = (int)payment.FraudStatus, /*
                            Ödeme işleminin fraud filtrelerine göre durumu. Eğer ödemenin fraud risk skoru düşük ise
                            ödemeye anında onay verilir, bu durumda 1 değeri döner. Eğer fraud risk 
                            skoru yüksek ise ödeme işlemi reddedilir ve -1 döner. Eğer ödeme işlemi daha sonradan
                            incelenip karar verilecekse 0 döner. Geçerli değerler: 0, -1 ve 1. Üye işyeri sadece 1 olan 
                            işlemlerde ürünü kargoya vermelidir, 0 olan işlemler için bilgilendirme beklemelidir. */

                            IyziCommissionFee = decimal.Parse(payment.IyziCommissionFee), //Ödemeye ait iyzico işlem ücreti.

                            IyziCommissionRateAmount = decimal.Parse(payment.IyziCommissionRateAmount), //Ödemeye ait iyzico işlem komisyon tutarı.

                            MerchantCommissionRateAmount = decimal.Parse(payment.IyziCommissionRateAmount), //Üye işyerinin uyguladığı vade/komisyon tutarı.

                            ProviderResponse= JsonSerializer.Serialize(payment)










                        };
                    }

                    return new PaymentResponse
                    {
                        Status = Status.FAILURE.ToString(),
                        ErrorCode = payment.ErrorCode,
                        ErrorMessage = payment.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_BACKENDSERVER_ERROR"
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
                        Status = Status.SUCCESS.ToString(),

                        ProviderResponse = JsonSerializer.Serialize(payment)
                    };
                }

                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorCode = payment.ErrorCode,
                    ErrorMessage = payment.ErrorMessage ?? "3D doğrulama başarısız"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
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
                        Status = Status.SUCCESS.ToString(),

                    };
                }

                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorCode = cancel.ErrorCode,
                    ErrorMessage = cancel.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
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
                        Status = Status.SUCCESS.ToString(),

                    };
                }

                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorCode = refund.ErrorCode,
                    ErrorMessage = refund.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_REFUND_ERROR"
                };
            }
        }
    }
}