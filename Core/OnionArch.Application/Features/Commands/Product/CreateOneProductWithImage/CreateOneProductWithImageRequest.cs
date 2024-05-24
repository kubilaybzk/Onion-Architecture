using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Domain.Entities;

namespace OnionArch.Application.Features.Commands.Product.CreateOneProductWithImage
{
	public class CreateOneProductWithImageRequest: IRequest<CreateOneProductWithImageResponse>
    {

        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public string Brand { get; set; } // Ürün markası
        public string Model { get; set; } // Ürün modeli
        public string Category { get; set; } // Ürün kategorisi
        public string ProductCode { get; set; } // Ürün kodu
        public decimal UnitPrice { get; set; } // Birim fiyatı
        public decimal DiscountRate { get; set; } // İndirim oranı
        public decimal DiscountPrice { get; set; } // İndirimli fiyatı
        public decimal Tax { get; set; } // Vergi miktarı
        public decimal KDVRate { get; set; } // KDV oranı
        public string Currency { get; set; } // Para birimi
        public int StockQuantity { get; set; } // Stok miktarı
        public int MinOrderQuantity { get; set; } = 1; // Minimum sipariş adeti
        public int MaxOrderQuantity { get; set; } = 99; // Maksimum sipariş adeti
        public string Condition { get; set; } // Ürün durumu (yeni, kullanılmış, yenilenmiş)
        public bool IsActive { get; set; } // Ürün aktif mi?

        public IFormFileCollection? ImageFiles { get; set; }

        public Dictionary<string, string>? Attributes { get; set; } // Dinamik ürün özellikleri


    }
}

