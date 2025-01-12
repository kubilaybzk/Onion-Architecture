using OnionArch.Domain.Entities;
using OnionArch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.OrderServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromBasketAsync(string userId, string shippingAddress, string billingAddress);
        Task<Order> GetOrderByIdAsync(string id);
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(string orderId, OrderStatus status);
        Task<decimal> CalculateOrderTotalAsync(string orderId);
    }
}
