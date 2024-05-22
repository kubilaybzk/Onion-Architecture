using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Product
{
    public class VM_Result_ProductLink
    {
        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public string Brand { get; set; } // Ürün markası
        public string Model { get; set; } // Ürün modeli
        public string ProductCode { get; set; } // Ürün kodu
        public decimal UnitPrice { get; set; } // Birim fiyatı
        public decimal DiscountRate { get; set; } // İndirim oranı
        public decimal DiscountPrice { get; set; } // İndirimli fiyatı
        public decimal AppliedDiscountRate { get; set; } // İndirim oranı uygulanmış hali
        public decimal AppliedDiscountPrice { get; set; } // İndirimli fiyat uygulanmış hali
        public decimal LastPrice { get; set; }  //Tüm hesaplamalardan sonraki fiyat
        public string Currency { get; set; } // Para birimi
        public int StockQuantity { get; set; } // Stok miktarı
        public int MinOrderQuantity { get; set; } = 1; // Minimum sipariş adeti
        public int MaxOrderQuantity { get; set; } = 99; // Maksimum sipariş adeti
        public string Condition { get; set; } // Ürün durumu (yeni, kullanılmış, yenilenmiş)
        public bool IsActive { get; set; } // Ürün aktif mi?
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
    }
}
