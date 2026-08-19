using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class CartViewModel
{
    public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal TotalPrice => Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
    public int TotalItems => Items.Sum(i => i.Quantity);
}
