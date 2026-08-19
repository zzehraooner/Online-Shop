using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(50)]
    [Display(Name = "Ad")]
    public string? FirstName { get; set; }

    [StringLength(50)]
    [Display(Name = "Soyad")]
    public string? LastName { get; set; }

    [StringLength(500)]
    [Display(Name = "Adres")]
    public string? Address { get; set; }

    [Display(Name = "Kayıt Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Ad Soyad")]
    public string DisplayName => $"{FirstName} {LastName}".Trim();
}
