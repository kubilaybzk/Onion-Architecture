using OnionArch.Application.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.AddMultipleItemToBasket
{
    public class AddMultipleItemToBasketResponse:GlobalResponseResult
    {
        public Boolean isAdded { get; set; }
    }
}
