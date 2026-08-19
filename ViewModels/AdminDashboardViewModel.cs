using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalUsers { get; set; }
    public decimal TotalRevenue { get; set; }
    public IEnumerable<Order> RecentOrders { get; set; } = new List<Order>();
    public int PendingOrders { get; set; }
    public int LowStockProducts { get; set; }
}
