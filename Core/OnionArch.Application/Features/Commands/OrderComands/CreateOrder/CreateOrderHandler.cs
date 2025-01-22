using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.HubServices;
using OnionArch.Application.Abstractions.OrderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.OrderComands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
      

        public CreateOrderHandler(IOrderService orderService, IHttpContextAccessor httpContextAccessor, IOrderHubService orderHubService)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
          
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    throw new Exception("Kullanıcı bulunamadı");

                var order = await _orderService.CreateOrderFromBasketAsync(
                    username,
                    request.ShippingAddress,
                    request.BillingAddress
                );
              
                return new CreateOrderResponse
                {
                    OrderId = order.ID.ToString(),
                    OrderNo = order.OrderNo,
                    TotalAmount = order.TotalAmount,
                    DiscountedAmount = order.DiscountedAmount,
                    Message = "Sipariş başarıyla oluşturuldu",
                    StatusCode = System.Net.HttpStatusCode.Created
                };
            }
            catch (Exception ex)
            {
                return new CreateOrderResponse
                {
                    Message = "Sipariş oluşturulurken bir hata oluştu",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true
                };
            }
        }
    }
}
