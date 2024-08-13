using OnionArch.Application.View_Models.Brands;
using OnionArch.Application.View_Models.Category;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.View_Models.Product
{

    //Ürün listeleme ve ürün detay için kullanılıyor.

    public class VM_Result_ProductLink
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Ürün adı
        public string SmallDescription { get; set; } // Ürün hakkında ufak  açıklaması
        public string LongDescription { get; set; }  //Ürün hakkında ana açıklama 
        public VM_BrandNameWithId_Result Brand { get; set; } // Ürün markası
        public string Model { get; set; } // Ürün modeli
        public string ProductCode { get; set; } // Ürün kodu
        public decimal UnitPrice { get; set; } // Birim fiyatı
        public decimal DiscountRate { get; set; } // İndirim oranı
        public decimal DiscountPrice { get; set; } // İndirimli fiyatı
        public decimal AppliedDiscountRate { get; set; } // İndirim oranı uygulanmış hali
        public decimal AppliedDiscountPrice { get; set; } // İndirimli fiyat uygulanmış hali
        public decimal Tax { get; set; } // Vergi miktarı
        public decimal KDVRate { get; set; } // KDV oranı
        public decimal LastPrice { get; set; }  //Tüm hesaplamalardan sonraki fiyat
        public string Currency { get; set; } // Para birimi
        public int StockQuantity { get; set; } // Stok miktarı
        public int MinOrderQuantity { get; set; } = 1; // Minimum sipariş adeti
        public int MaxOrderQuantity { get; set; } = 99; // Maksimum sipariş adeti
        public string Condition { get; set; } // Ürün durumu (yeni, kullanılmış, yenilenmiş)
        public bool IsActive { get; set; } // Ürün aktif mi?
        public string MaterializedProductPath { get; set; } 
        public string MaterializedProductPathByName { get; set; } 
        public string MaterializedProductPathBySlug { get; set; }
        public List<ProductImageFile>? ProductImageFiles { get; set; }
        public List<VM_Result_CategoryList>? CategoryLists { get; set; }
        public List<ProductAttribute>? ProductAttributes { get; set; }
    }
}
