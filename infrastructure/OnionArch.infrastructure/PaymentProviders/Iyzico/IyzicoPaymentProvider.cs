using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.Application.DTOs.Payment;
using OnionArch.infrastructure.PaymentProviders.Common;
using System.Text.Json;
using System.Globalization;
using OnionArch.Application.View_Models.BasketItem;
using Microsoft.AspNetCore.Http;
using OnionArch.Domain.Entities;
using System.Numerics;

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


                var TotalBasketItemPrice = request.OrderInformation.Basket.BasketItems
                .Select(ba => new VM_Result_BasketList()
                {
                    ProductId=ba.ProductId.ToString(),
                    ProductName=ba.Product.Name,
                    Quantity = ba.Quantity,
                    ProductLastPrice = ba.Product.LastPrice,
                    ProductCurrency = ba.Product.Currency,
                    ProductOriginalPrice = ba.Product.UnitPrice,
                    Price = ba.Product.UnitPrice,
                    CategoryNames = ba.Product.Categorys.Select(p=>p.CategoryName).ToList()
                }).ToList();

                var totalDiscountedProducts = TotalBasketItemPrice.Sum(ba => (float)(ba.ProductLastPrice * ba.Quantity)); //Ürün fiyatı * toplam adet
                float totalOriginalPrice = TotalBasketItemPrice.Sum(ba => (float)(ba.ProductOriginalPrice * ba.Quantity));
                float totalDiscount;
                float totalCargoPrice;
                float totalPrice;
                float totalPriceWithOutCargo;
                if (totalDiscountedProducts > 0)
                {
                    // Toplam indirim (%10)
                    totalDiscount = totalOriginalPrice - totalDiscountedProducts; //Ürünlere yapılan toplam indirim

                    // Kargo ücreti hesaplama (1000 TL üzeri ücretsiz)
                    totalCargoPrice = totalDiscountedProducts > 1000 ? 0 : 20.00f;

                    // Toplam fiyat (Ürünler - İndirim + Kargo)
                    totalPrice = totalOriginalPrice - totalDiscount + totalCargoPrice; //Tüm sepetin toplam fiyatı indirimli
                    totalPriceWithOutCargo = totalOriginalPrice - totalDiscount;
                }
                else
                {
                    totalDiscount = 0;
                    totalCargoPrice = 0;
                    totalPrice = 0;
                    totalPriceWithOutCargo = 0;
                }


                var shippingAddress = request.OrderInformation.User.Addresses
                     .FirstOrDefault(a => a.ID.ToString() == request.OrderInformation.ShippingAddress);

                var billingAddress = request.OrderInformation.User.Addresses
                    .FirstOrDefault(a => a.ID.ToString() == request.OrderInformation.BillingAddress);


                var basketItems = TotalBasketItemPrice.Select(item => new Iyzipay.Model.BasketItem
                {
                    Id = item.ProductId,  // Ürün ID'si
                    Name = item.ProductName ?? "Ürün", // Ürün adı
                    Category1 = item.CategoryNames?.FirstOrDefault() ?? "Genel", // İlk kategori adı
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.ProductLastPrice*item.Quantity).ToString(CultureInfo.InvariantCulture), // Son fiyat
                }).ToList();

                

                var iyzipayRequest = new CreatePaymentRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = request.OrderId,
                    Price = totalPriceWithOutCargo.ToString(CultureInfo.InvariantCulture), // 2 decimal basamak
                    PaidPrice = request.OrderInformation.Basket.DiscountedAmount.HasValue
                        ? request.OrderInformation.Basket.DiscountedAmount.Value.ToString(CultureInfo.InvariantCulture)
                        : totalPrice.ToString(CultureInfo.InvariantCulture),
                    Currency = Currency.TRY.ToString(),
                    Installment = request.Installment,
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
                        Id = request.OrderInformation.UserId, // Test için sabit değer
                        Name = request.OrderInformation.User.NameSurname.ToString().Trim().Split(' ')[0],
                        Surname = request.OrderInformation.User.NameSurname.ToString().Trim().Split(' ')[1],
                        Email = request.OrderInformation.User.Email,
                        IdentityNumber = "000000000000000",
                        RegistrationAddress = shippingAddress.LongAddress,
                        City = shippingAddress.City,
                        Country = shippingAddress.Country,
                        ZipCode = shippingAddress.ZipCode,
                        Ip = "111.111.11"
                    },

                    ShippingAddress = new Iyzipay.Model.Address
                    {
                        ContactName = shippingAddress.RecipientName+shippingAddress.RecipientSurName,
                        City = shippingAddress.City,
                        Country = shippingAddress.Country,
                        Description = shippingAddress.LongAddress,
                        ZipCode = shippingAddress.ZipCode
                    },

                    BillingAddress = new Iyzipay.Model.Address
                    {
                        ContactName = billingAddress.RecipientName + billingAddress.RecipientSurName,
                        City = billingAddress.City,
                        Country = billingAddress.Country,
                        Description = billingAddress.LongAddress,
                        ZipCode = billingAddress.ZipCode
                    },

                    BasketItems  = basketItems,
                };

                if (request.Use3D)
                {
                    var baseUrl = _configuration["BaseUrl"] ?? "http://localhost:5031";  // Default değer ekledik
                    var callbackUrl = baseUrl.TrimEnd('/') + "/api/Payment/complete-3d";
                    iyzipayRequest.CallbackUrl = callbackUrl;

                    Console.WriteLine($"CallbackUrl: {callbackUrl}"); // Debug için URL'i loglayalım
                    var payment = await ThreedsInitialize.Create(iyzipayRequest, options);

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
                        ProviderResponse = payment.HtmlContent
                    };
                }
                else
                {
                    var payment = await Payment.Create(iyzipayRequest, options);

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
                        Price = payment.Price, //Ödeme sepet tutarı.
                        PaidPrice = payment.PaidPrice, //İndirim vade farkı vs. hesaplanmış POS’tan geçen, tahsil edilen, nihai tutar.
                        Currency = payment.Currency, //Ödeme alındığı para birimi,
                        //Installment = (int)payment.Installment, //Taksit bilgisi.
                        PaymentStatus = payment.PaymentStatus, // SUCCESS, FAILURE, INIT_THREEDS, CALLBACK_THREEDS, BKM_POS_SELECTED, CALLBACK_PECCO
                        BasketId = payment.BasketId, //Sepetin id değeri
                        BinNumber = payment.BinNumber,    //Kartın bin numarası,
                        CardAssociation = payment.CardAssociation, //Eğer ödeme yapılan kart yerel bir kart ise, kartın ait olduğu kuruluş. 
                        CardFamily = payment.CardFamily, //yerel bir kart ise, kartın ait olduğu aile. Geçerli değerler: Bonus, Axess, World, Maximum, Paraf, CardFinans, Advantage
                        CardType = payment.CardType, //Geçerli değerler: CREDIT_CARD, DEBIT_CARD, PREPAID_CARD
                        /*FraudStatus = (int)payment.FraudStatus, 
                            Ödeme işleminin fraud filtrelerine göre durumu. Eğer ödemenin fraud risk skoru düşük ise
                            ödemeye anında onay verilir, bu durumda 1 değeri döner. Eğer fraud risk 
                            skoru yüksek ise ödeme işlemi reddedilir ve -1 döner. Eğer ödeme işlemi daha sonradan
                            incelenip karar verilecekse 0 döner. Geçerli değerler: 0, -1 ve 1. Üye işyeri sadece 1 olan 
                            işlemlerde ürünü kargoya vermelidir, 0 olan işlemler için bilgilendirme beklemelidir. */

                        
                        Installment = payment.Installment.HasValue ? (int)payment.Installment : 0, // Default to 0 if null
                        FraudStatus = payment.FraudStatus.HasValue ? (int)payment.FraudStatus : 0, // Default to 0 if null


                        IyziCommissionFee = payment.IyziCommissionFee, //Ödemeye ait iyzico işlem ücreti.
                        IyziCommissionRateAmount = payment.IyziCommissionRateAmount, //Ödemeye ait iyzico işlem komisyon tutarı.
                        MerchantCommissionRateAmount = payment.IyziCommissionRateAmount, //Üye işyerinin uyguladığı vade/komisyon tutarı.
                        ProviderResponse = JsonSerializer.Serialize(payment)
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
        public async Task<PaymentResponse> ProcessThreeDPaymentAsync(string paymentId, string conversationId, string conversationData)
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
                    ConversationId = conversationId,
                    PaymentId = paymentId,
                    ConversationData = conversationData
                };

                var payment = await ThreedsPayment.Create(request, options);

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

                        Price = payment.Price, //Ödeme sepet tutarı.

                        PaidPrice = payment.PaidPrice, //İndirim vade farkı vs. hesaplanmış POS’tan geçen, tahsil edilen, nihai tutar.

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

                        IyziCommissionFee = payment.IyziCommissionFee, //Ödemeye ait iyzico işlem ücreti.

                        IyziCommissionRateAmount = payment.IyziCommissionRateAmount, //Ödemeye ait iyzico işlem komisyon tutarı.

                        MerchantCommissionRateAmount =payment.IyziCommissionRateAmount, //Üye işyerinin uyguladığı vade/komisyon tutarı.

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

        public async Task<PaymentResponse> CheckBinNumber(string binNumber)
        {
            try
            {
                var options = new Options
                {
                    ApiKey = _apiKey,
                    SecretKey = _secretKey,
                    BaseUrl = _baseUrl
                };

                RetrieveBinNumberRequest request = new RetrieveBinNumberRequest();
                request.Locale = Locale.TR.ToString();
                request.BinNumber = binNumber;

                BinNumber binNumberresponse = await BinNumber.Retrieve(request, options);
               if(binNumberresponse.Status == Status.SUCCESS.ToString())
                {
                    return new PaymentResponse
                    {
                        Status = Status.SUCCESS.ToString(),
                        BinNumber = binNumberresponse.Bin,
                        CardAssociation = binNumberresponse.CardAssociation,
                        CardFamily = binNumberresponse.CardFamily,
                        CardType = binNumberresponse.CardType,
                        Commerical = binNumberresponse.Commercial,
                        BankName = binNumberresponse.BankName
                    };
                }
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorCode = binNumberresponse.ErrorCode,
                    ErrorMessage = binNumberresponse.ErrorMessage
                };
            }
            catch (Exception ex)
            {
                // Hata durumunda boş response dön
                return new PaymentResponse
                {
                    Status = Status.FAILURE.ToString(),
                    ErrorMessage = ex.Message,
                    ErrorCode = "IYZICO_BIN_CHECK_ERROR"
                };
            }
        }

        

        public async Task<PaymentInstamentDTO> GetBasketInstament(string cardNumber, string paidPrice)
        {
            var options = new Options
            {
                ApiKey = _apiKey,
                SecretKey = _secretKey,
                BaseUrl = _baseUrl
            };
            RetrieveInstallmentInfoRequest request = new RetrieveInstallmentInfoRequest();
            request.Locale = Locale.TR.ToString();
            request.BinNumber = cardNumber;
            request.Price = paidPrice.ToString();

            InstallmentInfo installmentInfo = await InstallmentInfo.Retrieve(request, options);

            return new PaymentInstamentDTO()
            {
                ErrorCode = installmentInfo.ErrorCode,
                ErrorGroup = installmentInfo.ErrorGroup,
                ErrorMessage = installmentInfo.ErrorMessage,
                Status = installmentInfo.Status,
                installmentDetails = installmentInfo.InstallmentDetails?.Select(x => new OnionArch.Application.DTOs.Payment.InstallmentDetail
                {
                    binNumber = x.BinNumber,
                    price = double.Parse(x.Price ?? "0"), // Convert string to double
                    cardType = x.CardType,
                    cardAssociation = x.CardAssociation,
                    cardFamilyName = x.CardFamilyName,
                    force3ds = (int)x.Force3Ds, // Convert nullable int to int
                    bankCode = (int)x.BankCode, // Convert nullable int to int
                    bankName = x.BankName,
                    forceCvc = (int)x.ForceCvc, // Convert nullable int to int
                    commercial = (int)x.Commercial, // Convert nullable int to int
                    installmentPrices = x.InstallmentPrices?.Select(p => new OnionArch.Application.DTOs.Payment.InstallmentPrice
                    {
                        installmentPrice = p.Price , // Convert string to double
                        totalPrice = p.TotalPrice, // Convert string to double
                    }).ToList() ?? null
                }).ToList()
            };
        }
    }
}