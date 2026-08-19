using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public string? SearchTerm { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string? SortBy { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 12;
}
