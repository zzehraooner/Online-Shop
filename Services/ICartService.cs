using OnlineShop.Models;

namespace OnlineShop.Services;

public interface ICartService
{
    Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
    Task AddToCartAsync(string userId, int productId, int quantity = 1);
    Task RemoveFromCartAsync(string userId, int productId);
    Task UpdateQuantityAsync(string userId, int productId, int quantity);
    Task ClearCartAsync(string userId);
    Task<int> GetCartCountAsync(string userId);
    Task<decimal> GetCartTotalAsync(string userId);
}
