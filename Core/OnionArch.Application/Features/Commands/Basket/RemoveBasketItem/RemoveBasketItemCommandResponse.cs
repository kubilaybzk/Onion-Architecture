using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandResponse : GlobalResponseResult
    {
        public Boolean isDeleted { get; set; }
    }
}
