using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [StringLength(100)]
    [Display(Name = "Kategori Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Display(Name = "Görsel URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Üst Kategori")]
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
