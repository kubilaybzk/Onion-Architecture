using FluentValidation;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Features.Commands.HeroSectionComands.CreateHeroSectionComands;

namespace OnionArch.Application.Validators.HeroSectionValidators
{
    public class HeroSectionValidator : AbstractValidator<CreateHeroSectionRequest>
    {
        public HeroSectionValidator()
        {
            RuleFor(x => x.ImageAltTile)
                .NotEmpty().WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("{PropertyName} alanı en fazla {MaxLength} karakter olabilir.");

            RuleFor(x => x.ImageRederictLink)
                .NotEmpty().WithMessage("{PropertyName} alanı boş bırakılamaz.");

            RuleFor(x => x.ImageRedirectLinkTitle)
                .NotEmpty().WithMessage("{PropertyName} alanı boş bırakılamaz.")
                .MaximumLength(50).WithMessage("{PropertyName} alanı en fazla {MaxLength} karakter olabilir.");

            RuleFor(x => x.Order)
                .NotEmpty().WithMessage("Sıralama alanı boş bırakılamaz.")
                .GreaterThanOrEqualTo(0).WithMessage("Sıralama değeri 0'dan küçük olamaz.");

            When(x => x.HeroSectionImage != null, () =>
            {
                RuleFor(x => x.HeroSectionImage)
                    .Must(ValidateImageFiles)
                    .WithMessage("Yüklenen resim dosyası geçersiz. Lütfen maksimum 5MB boyutunda ve JPEG, JPG veya PNG formatında bir dosya yükleyin.");
            });

            // Display names ayarlayarak property isimlerinin Türkçe görünmesini sağlayalım
            RuleSet("DisplayNames", () =>
            {
                RuleFor(x => x.ImageAltTile).NotEmpty().WithName("Resim Alternatif Metni");
                RuleFor(x => x.ImageRederictLink).NotEmpty().WithName("Yönlendirme Linki");
                RuleFor(x => x.ImageRedirectLinkTitle).NotEmpty().WithName("Link Başlığı");
                RuleFor(x => x.Order).NotEmpty().WithName("Sıralama");
                RuleFor(x => x.HeroSectionImage).NotEmpty().WithName("Resim Dosyası");
            });
        }

        

        private bool ValidateImageFiles(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return false;

            foreach (var file in files)
            {
                // Dosya boyutu kontrolü (5MB)
                if (file.Length > 5 * 1024 * 1024)
                    return false;

                // Dosya tipi kontrolü
                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
                if (!allowedTypes.Contains(file.ContentType.ToLower()))
                    return false;
            }

            return true;
        }
    }
}