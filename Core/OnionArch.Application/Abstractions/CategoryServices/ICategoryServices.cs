using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.CategoryServices
{
    public interface ICategoryServices
    {
        public Task<Boolean> AddSubCategoryAsync(Guid parentCategoryId, Category subCategory,bool isUpdate);
        public Task AssignMaterializedPathAsync(Category category, Guid? parentCategoryId);
        public string GenerateSlug(string phrase);
        public string RemoveTurkishCharacters(string input);

    }
}
