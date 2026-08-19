using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(string userId, CheckoutViewModel model);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId, string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
}
