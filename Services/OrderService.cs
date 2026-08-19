using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Services;

public class OrderService(ApplicationDbContext context, ICartService cartService) : IOrderService
{
    public async Task<Order> CreateOrderAsync(string userId, CheckoutViewModel model)
    {
        var cartItems = await cartService.GetCartItemsAsync(userId);
        var cartItemList = cartItems.ToList();

        if (!cartItemList.Any())
            throw new InvalidOperationException("Sepetiniz boş.");

        var order = new Order
        {
            UserId = userId,
            FullName = model.FullName,
            Phone = model.Phone,
            ShippingAddress = model.ShippingAddress,
            Note = model.Note,
            OrderNumber = GenerateOrderNumber(),
            TotalAmount = cartItemList.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity),
            Status = OrderStatus.Pending,
            OrderItems = cartItemList.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? "",
                Quantity = ci.Quantity,
                UnitPrice = ci.Product?.Price ?? 0
            }).ToList()
        };

        // Update stock
        foreach (var item in cartItemList)
        {
            var product = await context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
                if (product.Stock < 0) product.Stock = 0;
            }
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        // Clear cart
        await cartService.ClearCartAsync(userId);

        return order;
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await context.SaveChangesAsync();
        }
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
