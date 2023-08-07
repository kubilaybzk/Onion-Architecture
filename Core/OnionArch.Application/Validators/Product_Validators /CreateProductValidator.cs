
using FluentValidation;
using OnionArch.Application.View_Models;

namespace OnionArch.Application.Validators.Product_Validators
{
	public class CreateProductValidator:AbstractValidator<VM_Create_Product>
	{
		//Burada her bir Validator işlemi Generic olarak bir tip ister.
		//Bizler constructor aracılığıyla bu tip içine gerekli bilgileri gireriz.

		public CreateProductValidator()
		{
			//Mesela ürün ismi 5-100 karakter arası ve boş olamaz şekilde ayarlayalım
			RuleFor(p => p.Name)
				.NotEmpty()
					.WithMessage("Ürün adı boş olamaz")
					.MaximumLength(100)
					.MinimumLength(2)
						.WithMessage("Ürün adı 2 ile 100 karakter arasında olmalıdır.");

			RuleFor(p => p.Price)
				.NotEmpty()
				.NotNull()
					.WithMessage("Fiyat bilgisi boş olamaz")
					.Must(p => p > 0)
						.WithMessage("Fiyat bilgisi negatif olamaz");

			
			RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stock bilgisi boş olamaz")
                    .Must(p => p > 0)
                        .WithMessage("Stock bilgisi negatif olamaz");
        }
    }
}

