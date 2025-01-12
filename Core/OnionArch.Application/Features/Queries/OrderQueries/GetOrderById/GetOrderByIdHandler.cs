using MediatR;
using OnionArch.Application.Abstractions.OrderServices;
using OnionArch.Application.Features.Queries.OrderQueries.GetUserOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Queries.OrderQueries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdResponse> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(request.OrderId);
                if (order == null)
                    throw new Exception("Sipariş bulunamadı");

                var orderDTO = new UserOrderDTO
                {
                    OrderId = order.ID.ToString(),
                    OrderNo = order.OrderNo,
                    CreateDate = order.CreateTime,
                    Status = order.Status.ToString(),
                    TotalAmount = order.TotalAmount,
                    DiscountedAmount = order.DiscountedAmount,
                    Items = order.OrderItems.Select(oi => new OrderItemDTO
                    {
                        ProductName = oi.Product.Name,
                        UnitPrice = oi.UnitPrice,
                        Quantity = oi.Quantity,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                };

                return new GetOrderByIdResponse
                {
                    Order = orderDTO,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new GetOrderByIdResponse
                {
                    Message = "Sipariş getirilirken bir hata oluştu",
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    HassError = true
                };
            }
        }
    }
}
