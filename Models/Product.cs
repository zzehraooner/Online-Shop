using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [StringLength(200)]
    [Display(Name = "Ürün Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0.01 ile 999999.99 arasında olmalıdır.")]
    [Display(Name = "Fiyat")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Price { get; set; }

    [Display(Name = "Görsel URL")]
    public string? ImageUrl { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stok negatif olamaz.")]
    [Display(Name = "Stok")]
    public int Stock { get; set; }

    [Required]
    [Display(Name = "Kategori")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Öne Çıkan")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Güncellenme Tarihi")]
    public DateTime? UpdatedAt { get; set; }
}
