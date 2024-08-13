using MediatR;
using OnionArch.Application.Features.Queries.BrandQueries.GetAllBrandNameQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.BrandQueries.GetBrandNameWithIdQueries
{
    public class GetAllBrandNameRequest : IRequest<GetAllBrandNameResponse>
    {
    }
}
