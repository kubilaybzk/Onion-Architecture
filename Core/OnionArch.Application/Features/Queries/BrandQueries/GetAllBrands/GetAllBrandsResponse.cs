using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Brands;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetAllBrands
{
    public class GetAllBrandsResponse:GlobalResponseResult
    {
        public List<VM_Brand_Result>? BrandList { get; set; }
    }
}
