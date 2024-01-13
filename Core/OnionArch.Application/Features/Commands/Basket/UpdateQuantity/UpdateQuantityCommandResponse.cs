using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.UpdateQuantity
{
    internal class UpdateQuantityCommandResponse
    {
        public bool ErorStatus { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
