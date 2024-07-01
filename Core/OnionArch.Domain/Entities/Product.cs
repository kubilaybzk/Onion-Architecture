using System;
using System.Text.Json.Serialization;
using OnionArch.Domain.Entities.Common;

namespace OnionArch.Domain.Entities
{
	public class Product:BaseEntity
	{
        public string Name { get; set; } // Ürün adı
        public string SmallDescription { get; set; } // Ürün hakkında ufak  açıklaması
        public string LongDescription { get; set; }  //Ürün hakkında ana açıklama 
        public string Brand { get; set; } // Ürün markası
        public string Model { get; set; } // Ürün modeli
        public ICollection<Category> Categorys { get; set; } // Ürün kategorisi
        public string ProductCode { get; set; } // Ürün kodu
        public decimal UnitPrice { get; set; } // Birim fiyatı
        public decimal DiscountRate { get;set; } // İndirim oranı
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
        public string MaterializedProductPath { get; set; } = "";
        public string MaterializedProductPathByName { get; set; } = "";
        public string MaterializedProductPathBySlug { get; set; } = "";

        //Bir ürün birden fazla sipariş içerebilir Many-to-many relation
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public ICollection<ProductAttribute> ProductAttributes { get; set; }

    }
}

