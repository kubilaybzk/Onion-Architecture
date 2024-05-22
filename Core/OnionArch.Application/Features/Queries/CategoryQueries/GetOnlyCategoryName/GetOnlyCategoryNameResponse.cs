using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Category;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.CategoryQueries.GetOnlyCategoryName
{
    public class GetOnlyCategoryNameResponse : GlobalResponseResult
    {
        public List<VM_GetOnlyCategoryName_VM>? Categories { get; set; }

    }
}
