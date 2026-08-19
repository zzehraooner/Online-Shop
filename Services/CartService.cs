using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services;

public class CartService(ApplicationDbContext context) : ICartService
{
    public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
    {
        return await context.CartItems
            .Include(ci => ci.Product)
            .ThenInclude(p => p!.Category)
            .Where(ci => ci.UserId == userId)
            .OrderByDescending(ci => ci.AddedAt)
            .ToListAsync();
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
    {
        var existingItem = await context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            context.CartItems.Add(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }
        await context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string userId, int productId)
    {
        var item = await context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        if (item != null)
        {
            context.CartItems.Remove(item);
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
    {
        var item = await context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            await context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var items = await context.CartItems
            .Where(ci => ci.UserId == userId)
            .ToListAsync();
        context.CartItems.RemoveRange(items);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetCartCountAsync(string userId)
    {
        return await context.CartItems
            .Where(ci => ci.UserId == userId)
            .SumAsync(ci => ci.Quantity);
    }

    public async Task<decimal> GetCartTotalAsync(string userId)
    {
        return await context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.UserId == userId)
            .SumAsync(ci => (ci.Product!.Price) * ci.Quantity);
    }
}
