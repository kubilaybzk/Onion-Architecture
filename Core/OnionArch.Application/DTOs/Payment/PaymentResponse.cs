using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.DTOs.Payment
{
    public class PaymentResponse
    {
      
            /// <summary>
            /// Servis yanıt sonucu (başarılı/başarısız).
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Hata kodu. İşlem hatalıysa bu değer döner.
            /// </summary>
            public string ErrorCode { get; set; }

            /// <summary>
            /// Hata mesajı. İşlem hatalıysa bu mesaj döner.
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// Hata grubu. İşlem hatalıysa hata grubu bilgisi döner.
            /// </summary>
            public string ErrorGroup { get; set; }

            /// <summary>
            /// İstek için belirtilen dil kodu (varsayılan: 'tr').
            /// </summary>
            public string Locale { get; set; } = "tr";

            /// <summary>
            /// İşlemin gerçekleştiği anın Unix timestamp değeri.
            /// </summary>
            public long SystemTime { get; set; }

            /// <summary>
            /// İstek esnasında gönderilen konuşma kimliği, aynı şekilde geri döner.
            /// </summary>
            public string ConversationId { get; set; }

            // Normal Ödeme Parametreleri
            /// <summary>
            /// Ödemeye ait id. Üye işyeri tarafından saklanmalıdır.
            /// </summary>
            public string PaymentId { get; set; }

            /// <summary>
            /// Ödeme tutarı. Sepet tutarı.
            /// </summary>
            public string Price { get; set; }

            /// <summary>
            /// Nihai tahsilat tutarı, indirim ve diğer farklar hesaplanmış haliyle.
            /// </summary>
            public string PaidPrice { get; set; }

            /// <summary>
            /// Ödemenin alındığı para birimi.
            /// </summary>
            public string Currency { get; set; }

            /// <summary>
            /// Ödemenin taksit bilgisi. Geçerli değerler: 1, 2, 3, 6, 9, 12.
            /// </summary>
            public int Installment { get; set; }

            /// <summary>
            /// İşlemin ödeme durumunu gösterir. Geçerli değerler: SUCCESS, FAILURE, vb.
            /// </summary>
            public string PaymentStatus { get; set; }

            /// <summary>
            /// Sepet id'si, üye işyeri tarafından gönderilen sepetin kimliği.
            /// </summary>
            public string BasketId { get; set; }

            /// <summary>
            /// Ödeme yapılan kartın ilk 6 hanesi (BIN numarası).
            /// </summary>
            public string BinNumber { get; set; }

            /// <summary>
            /// Kartın ait olduğu kuruluş (Visa, MasterCard, vb.).
            /// </summary>
            public string CardAssociation { get; set; }

            /// <summary>
            /// Kartın ait olduğu aile (Bonus, Axess, vb.).
            /// </summary>
            public string CardFamily { get; set; }

            /// <summary>
            /// Kartın tipi (Kredi kartı, banka kartı, ön ödemeli kart vb.).
            /// </summary>
            public string CardType { get; set; }

            /// <summary>
            /// Fraud kontrolü sonucu ödeme durumu. 0 = kontrol edilecek, -1 = reddedildi, 1 = onaylandı.
            /// </summary>
            public int FraudStatus { get; set; }

            /// <summary>
            /// İyzico işlem ücreti.
            /// </summary>
            public string IyziCommissionFee { get; set; }

            /// <summary>
            /// İyzico işlem komisyon tutarı.
            /// </summary>
            public string IyziCommissionRateAmount { get; set; }

            /// <summary>
            /// Üye işyerinin uyguladığı vade/komisyon oranı.
            /// </summary>
            public string MerchantCommissionRate { get; set; }

            /// <summary>
            /// Üye işyerinin uyguladığı vade/komisyon tutarı.
            /// </summary>
            public string MerchantCommissionRateAmount { get; set; }

            // ItemTransactions Parametreleri
            /// <summary>
            /// Ödeme kırılımına ait id, üye işyeri tarafından saklanmalıdır.
            /// </summary>
            public string PaymentTransactionId { get; set; }

            /// <summary>
            /// Sepetteki ürüne ait id.
            /// </summary>
            public string ItemId { get; set; }

            /// <summary>
            /// Sepetteki ürüne ait tutar.
            /// </summary>
            public decimal ItemPrice { get; set; }

            /// <summary>
            /// Tahsilat tutarının kırılım bazındaki dağılımı.
            /// </summary>
            public decimal ItemPaidPrice { get; set; }

            /// <summary>
            /// Ödeme kırılımının durumu. 0 = fraud kontrolünde, -1 = reddedildi, 1 = onay bekliyor, 2 = onaylandı.
            /// </summary>
            public int TransactionStatus { get; set; }

            /// <summary>
            /// Kırılım bazındaki blokaj oranı.
            /// </summary>
            public decimal BlockageRate { get; set; }

            /// <summary>
            /// Kırılım bazındaki blokaj tutarının üye işyerine yansıyan kısmı.
            /// </summary>
            public decimal BlockageRateAmountMerchant { get; set; }

            /// <summary>
            /// Blokaj çözülme tarihi.
            /// </summary>
            public string BlockageResolvedDate { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem ücreti.
            /// </summary>
            public decimal IyziCommissionFeeItemTransactions { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem komisyon tutarı.
            /// </summary>
            public decimal IyziCommissionRateAmountItemTransactions { get; set; }

            /// <summary>
            /// Kırılım bazında üye işyerinin uyguladığı vade/komisyon oranı.
            /// </summary>
            public decimal MerchantCommissionRateItemTransactions { get; set; }

            /// <summary>
            /// Kırılım bazında üye işyerinin uyguladığı vade/komisyon tutarı.
            /// </summary>
            public decimal MerchantCommissionRateAmountItemTransactions { get; set; }

            /// <summary>
            /// Kırılım bazında üye işyerine gönderilecek net ödeme tutarı.
            /// </summary>
            public decimal MerchantPayoutAmountItemTransactions { get; set; }

            // ConvertedPayout Parametreleri
            /// <summary>
            /// Kırılım bazındaki tahsilat tutarının dağılımı.
            /// </summary>
            public decimal PaidPriceItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem ücreti.
            /// </summary>
            public decimal IyziCommissionFeeItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem komisyon tutarı.
            /// </summary>
            public decimal IyziCommissionRateAmountItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazındaki blokaj tutarının üye işyerine yansıyan kısmı.
            /// </summary>
            public decimal BlockageRateAmountMerchantItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında üye işyerine gönderilecek net ödeme tutarı.
            /// </summary>
            public decimal MerchantPayoutAmountItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem ücreti, komisyon tutarı ve blokajlar düşüldükten sonra üye işyerine gönderilecek tutar.
            /// </summary>
            public decimal IyziConversationRateItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında iyzico işlem ücreti, komisyon tutarı ve blokajlar düşüldükten sonra üye işyerine gönderilecek tutar.
            /// </summary>
            public decimal IyziConversationRateAmountItemTransactionsConvertedPayout { get; set; }

            /// <summary>
            /// Kırılım bazında ödeme yapılan para birimi.
            /// </summary>
            public string CurrencyItemTransactionsConvertedPayout { get; set; }

            // 3D Ödeme Parametreleri
            /// <summary>
            /// 3D doğrulama işlemi için HTML içeriği.
            /// </summary>
            
            public string ProviderResponse { get; set; }
            public int? Commerical { get; set; }
            public string? BankName { get; set; }
            public List<InstallmentDetail>? installmentDetails { get; set; } //Taksit oranları için
    }

    // Taksit oranları için gerekli olan response değerlerinin sınıfları.


    


}
