using System.ComponentModel.DataAnnotations;
using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class CheckoutViewModel
{
    public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal TotalPrice => CartItems.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);

    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur.")]
    [Phone]
    [Display(Name = "Telefon")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
    [Display(Name = "Teslimat Adresi")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Display(Name = "Sipariş Notu")]
    public string? Note { get; set; }
}
