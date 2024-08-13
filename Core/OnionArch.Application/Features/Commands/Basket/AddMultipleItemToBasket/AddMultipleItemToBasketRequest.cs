using MediatR;
using OnionArch.Application.View_Models.BasketItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.Basket.AddMultipleItemToBasket
{
    public class AddMultipleItemToBasketRequest:IRequest<AddMultipleItemToBasketResponse>
    {
        public List<VM_Add_BasketItem>  BasketItems{ get; set; }
    }
}
