using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.BrandCommands.DeleteBrandCommands
{
    public class DeleteBrandCommandsRequest:IRequest<DeleteBrandCommandsResponse>
    {
        public string DeletedBrandId { get; set; }
    }
}
