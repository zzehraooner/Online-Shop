using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models;

public enum OrderStatus
{
    [Display(Name = "Beklemede")]
    Pending,
    [Display(Name = "Onaylandı")]
    Confirmed,
    [Display(Name = "Kargoya Verildi")]
    Shipped,
    [Display(Name = "Teslim Edildi")]
    Delivered,
    [Display(Name = "İptal Edildi")]
    Cancelled
}

public class Order
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Sipariş Tarihi")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Toplam Tutar")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Durum")]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
    [StringLength(500)]
    [Display(Name = "Teslimat Adresi")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100)]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur.")]
    [Phone]
    [Display(Name = "Telefon")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Not")]
    public string? Note { get; set; }

    [Display(Name = "Sipariş No")]
    public string OrderNumber { get; set; } = string.Empty;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
