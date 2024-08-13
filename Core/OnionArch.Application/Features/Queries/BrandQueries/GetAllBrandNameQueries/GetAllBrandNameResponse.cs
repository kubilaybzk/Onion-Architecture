using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllBrandNameQueries
{
    public class GetAllBrandNameResponse : GlobalResponseResult
    {
        public List<VM_BrandNameWithId_Result> BrandIdsWithName { get; set; }
    }
}
