using OnionArch.Domain.Entities.Common;
using OnionArch.Domain.Entities.Identity;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Entities
{
    public class PaymentTransaction : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public AppUser User { get; set; }  
        public string UserId { get; set; }
        public PaymentStatus Status { get; set; }
        public string ProviderResponse { get; set; } //Genel olarak dönen tüm response

        //Card Information
        public bool IsThreeD { get; set; }
        public string CardNumber { get; set; }  // Maskelenmiş
        public string CardHolder { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }



        public string ConversationId { get; set; } //İletişime geçilirken kullanılan Id değeri
        public string PaymentProvider { get; set; } //Kullanılan ödeme yöntemi
        public DateTimeOffset SystemTime { get; set; } //Ödeme yapıldığı zaman
        public string PaymentId { get; set; } //PAYMENTID DEĞERİ
        public decimal Price { get; set; } //Sepet Tutarı
        public decimal PaidPrice { get; set; } // Ödenen Sepet Tutarı
        public string Currency { get; set; } //Ödeme yapılan birim
        public int Installment { get; set; } //Taksit durumları
        public string? PaymentStatus { get; set; } //SUCCESS, FAILURE, vb.
        public int FraudStatus { get; set; } //Fraud kontrolü sonucu ödeme durumu. 0 = kontrol edilecek, -1 = reddedildi, 1 = onaylandı
        public decimal ProviderCommissionFee { get; set; } // Provider işlem ücreti.
        public decimal ProviderCommissionRateAmount { get; set; } // Provider işlem ücreti.




    }
}
