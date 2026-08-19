using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models;

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Miktar 1 ile 100 arasında olmalıdır.")]
    [Display(Name = "Miktar")]
    public int Quantity { get; set; } = 1;

    [Required]
    public string UserId { get; set; } = string.Empty;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
