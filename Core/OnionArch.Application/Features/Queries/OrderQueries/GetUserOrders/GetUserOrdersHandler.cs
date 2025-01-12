using MediatR;
using Microsoft.AspNetCore.Http;
using OnionArch.Application.Abstractions.OrderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders
{
    public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersRequest, GetUserOrdersResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserOrdersHandler(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetUserOrdersResponse> Handle(GetUserOrdersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    throw new Exception("Kullanıcı bulunamadı");

                var orders = await _orderService.GetUserOrdersAsync(username);

                var orderDTOs = orders.Select(o => new UserOrderDTO
                {
                    OrderId = o.ID.ToString(),
                    OrderNo = o.OrderNo,
                    CreateDate = o.CreateTime,
                    Status = o.Status.ToString(),
                    TotalAmount = o.TotalAmount,
                    DiscountedAmount = o.DiscountedAmount,
                    Items = o.OrderItems.Select(oi => new OrderItemDTO
                    {
                        ProductName = oi.Product.Name,
                        UnitPrice = oi.UnitPrice,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                }).ToList();

                return new GetUserOrdersResponse
                {
                    Orders = orderDTOs,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new GetUserOrdersResponse
                {
                    Message = "Siparişler getirilirken bir hata oluştu",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true
                };
            }
        }
    }
}
