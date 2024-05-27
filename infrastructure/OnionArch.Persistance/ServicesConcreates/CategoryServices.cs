using Microsoft.Extensions.Logging;
using OnionArch.Application.Abstractions.CategoryServices;
using OnionArch.Application.Repositories.CategoryCrud;
using OnionArch.Domain.Entities;
using System.Text.RegularExpressions;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ILogger<CategoryServices> _logger;

        public CategoryServices(ICategoryReadRepository categoryReadRepository, ILogger<CategoryServices> logger, ICategoryWriteRepository categoryWriteRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _logger = logger;
            _categoryWriteRepository = categoryWriteRepository;
        }

        public async Task<bool> AddSubCategoryAsync(Guid parentCategoryId, Category subCategory, bool isUpdate)
        {
            try
            {
                var parentCategory = await _categoryReadRepository.GetByIdAsync(parentCategoryId.ToString());

                if (parentCategory.SubCategories == null)
                {
                    parentCategory.SubCategories = new List<Category>();
                }

                parentCategory.SubCategories.Add(subCategory);
                await _categoryWriteRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("AddSubCategoryAsync kısmında bir hata meydana geldi");
                _logger.LogError(ex.Message.ToString());
                return false;
            }
        }

        public async Task AssignMaterializedPathAsync(Category category, Guid? parentCategoryId)
        {
            if (!parentCategoryId.HasValue)
            {
                category.MaterializedPath = GenerateSlug(category.CategoryName);
                //category.MaterializedPathByName = GenerateSlug(category.CategoryName);
                category.MaterializedPathBySlug = GenerateSlug(category.CategorySlug);
            }
            else
            {
                var parentCategory = await _categoryReadRepository.GetByIdAsync(parentCategoryId.ToString());
                if (parentCategory == null)
                {
                    category.MaterializedPath = GenerateSlug(category.CategoryName);
                    //category.MaterializedPathByName = GenerateSlug(category.CategoryName);
                    category.MaterializedPathBySlug = GenerateSlug(category.CategorySlug);
                }
                else
                {
                    category.MaterializedPath = $"{parentCategory.MaterializedPath}.{GenerateSlug(category.CategoryName)}";
                    category.MaterializedPathByName = $"{parentCategory.MaterializedPathByName}.{category.MaterializedPathByName}";
                    category.MaterializedPathBySlug = $"{parentCategory.MaterializedPathBySlug}.{category.MaterializedPathBySlug}";
                }
            }

            await _categoryWriteRepository.SaveAsync();
        }

        public string GenerateSlug(string phrase)
        {
            // Türkçe karakterleri çıkar
            string str = RemoveTurkishCharacters(phrase).ToLower();

            // Geçersiz karakterleri temizle
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // Birden fazla boşluğu tek boşluğa dönüştür
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // 45 karakteri aşmayacak şekilde kırp ve boşlukları kes
            str = str.Substring(0, Math.Min(str.Length, 45)).Trim();
            // Boşlukları tireye dönüştür
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public string RemoveTurkishCharacters(string input)
        {
            // Türkçe karakterleri çevirme
            input = input.Replace("ı", "i").Replace("İ", "I")
                         .Replace("ş", "s").Replace("Ş", "S")
                         .Replace("ğ", "g").Replace("Ğ", "G")
                         .Replace("ç", "c").Replace("Ç", "C")
                         .Replace("ö", "o").Replace("Ö", "O")
                         .Replace("ü", "u").Replace("Ü", "U");

            return input;
        }
    }
 }

