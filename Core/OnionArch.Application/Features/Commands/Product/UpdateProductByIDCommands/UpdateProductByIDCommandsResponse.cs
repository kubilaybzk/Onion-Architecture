using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Product.UpdateProductByIDCommands
{
    public class UpdateProductByIDCommandsResponse:GlobalResponseResult
    {
        public bool isUpdated { get; set; }
    }
}
