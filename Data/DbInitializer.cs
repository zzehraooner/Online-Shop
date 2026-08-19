using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;

namespace OnlineShop.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        // Create roles
        string[] roles = { "Admin", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create admin user
        if (await userManager.FindByEmailAsync("admin@onlineshop.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@onlineshop.com",
                Email = "admin@onlineshop.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Seed categories
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new() { Name = "Elektronik", Description = "Telefon, bilgisayar ve diğer elektronik ürünler", ImageUrl = "/images/categories/elektronik.jpg" },
                new() { Name = "Giyim", Description = "Kadın, erkek ve çocuk giyim", ImageUrl = "/images/categories/giyim.jpg" },
                new() { Name = "Ev & Yaşam", Description = "Ev dekorasyonu ve yaşam ürünleri", ImageUrl = "/images/categories/ev-yasam.jpg" },
                new() { Name = "Spor", Description = "Spor ekipmanları ve giyim", ImageUrl = "/images/categories/spor.jpg" },
                new() { Name = "Kitap", Description = "Kitaplar ve dergiler", ImageUrl = "/images/categories/kitap.jpg" },
                new() { Name = "Kozmetik", Description = "Güzellik ve bakım ürünleri", ImageUrl = "/images/categories/kozmetik.jpg" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // Seed products
        if (!context.Products.Any())
        {
            var elektronik = context.Categories.First(c => c.Name == "Elektronik");
            var giyim = context.Categories.First(c => c.Name == "Giyim");
            var evYasam = context.Categories.First(c => c.Name == "Ev & Yaşam");
            var spor = context.Categories.First(c => c.Name == "Spor");
            var kitap = context.Categories.First(c => c.Name == "Kitap");
            var kozmetik = context.Categories.First(c => c.Name == "Kozmetik");

            var products = new List<Product>
            {
                // Elektronik
                new() { Name = "iPhone 15 Pro Max", Description = "Apple'ın en güçlü akıllı telefonu. A17 Pro çip, 48MP kamera sistemi, titanium tasarım.", Price = 64999.99m, Stock = 25, CategoryId = elektronik.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/1a1a2e/e94560?text=iPhone+15" },
                new() { Name = "MacBook Air M3", Description = "Ultra ince ve hafif dizüstü bilgisayar. M3 çip, 15.3 inç Liquid Retina ekran.", Price = 49999.99m, Stock = 15, CategoryId = elektronik.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/16213e/0f3460?text=MacBook+Air" },
                new() { Name = "Samsung Galaxy S24 Ultra", Description = "Galaxy AI ile güçlendirilmiş akıllı telefon. 200MP kamera, S Pen dahil.", Price = 54999.99m, Stock = 20, CategoryId = elektronik.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/1a1a2e/e94560?text=Galaxy+S24" },
                new() { Name = "AirPods Pro 2", Description = "Aktif Gürültü Engelleme, Uyarlanabilir Ses özellikli kablosuz kulaklık.", Price = 8999.99m, Stock = 50, CategoryId = elektronik.Id, ImageUrl = "https://placehold.co/400x400/16213e/0f3460?text=AirPods+Pro" },
                new() { Name = "iPad Air M2", Description = "M2 çipli tablet. 11 inç Liquid Retina ekran, Apple Pencil desteği.", Price = 24999.99m, Stock = 30, CategoryId = elektronik.Id, ImageUrl = "https://placehold.co/400x400/1a1a2e/e94560?text=iPad+Air" },

                // Giyim
                new() { Name = "Premium Denim Ceket", Description = "Yüksek kalite denim kumaş, modern kesim, her mevsim giyilebilir.", Price = 1299.99m, Stock = 40, CategoryId = giyim.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/533483/e94560?text=Denim+Ceket" },
                new() { Name = "Slim Fit Gömlek", Description = "Premium pamuklu slim fit erkek gömleği. Ofis ve günlük kullanım için ideal.", Price = 699.99m, Stock = 60, CategoryId = giyim.Id, ImageUrl = "https://placehold.co/400x400/533483/e94560?text=Slim+Fit" },
                new() { Name = "Kadın Trençkot", Description = "Klasik kesim, su geçirmez kumaş, sonbahar-kış koleksiyonu.", Price = 2499.99m, Stock = 25, CategoryId = giyim.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/533483/e94560?text=Trençkot" },

                // Ev & Yaşam
                new() { Name = "Akıllı Robot Süpürge", Description = "Lazer navigasyon, otomatik çöp boşaltma, uygulama kontrolü.", Price = 12999.99m, Stock = 20, CategoryId = evYasam.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/0f3460/16213e?text=Robot+Süpürge" },
                new() { Name = "Kahve Makinesi", Description = "Tam otomatik espresso makinesi. 15 bar basınç, süt köpürtücü dahil.", Price = 7999.99m, Stock = 15, CategoryId = evYasam.Id, ImageUrl = "https://placehold.co/400x400/0f3460/16213e?text=Kahve+Makinesi" },

                // Spor
                new() { Name = "Profesyonel Yoga Matı", Description = "6mm kalınlık, kaymaz yüzey, taşıma çantası dahil.", Price = 599.99m, Stock = 100, CategoryId = spor.Id, ImageUrl = "https://placehold.co/400x400/e94560/1a1a2e?text=Yoga+Matı" },
                new() { Name = "Koşu Ayakkabısı Pro", Description = "Ultra hafif, maksimum konfor, nefes alabilen kumaş.", Price = 2999.99m, Stock = 45, CategoryId = spor.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/e94560/1a1a2e?text=Koşu+Ayakkabısı" },

                // Kitap
                new() { Name = "Yapay Zeka ve Gelecek", Description = "AI teknolojilerinin geleceğini anlatan kapsamlı rehber.", Price = 149.99m, Stock = 200, CategoryId = kitap.Id, ImageUrl = "https://placehold.co/400x400/16213e/533483?text=AI+Kitap" },
                new() { Name = "Minimalist Yaşam", Description = "Hayatınızı sadeleştirmenin yolları. Bestseller kişisel gelişim kitabı.", Price = 89.99m, Stock = 150, CategoryId = kitap.Id, ImageUrl = "https://placehold.co/400x400/16213e/533483?text=Minimalist" },

                // Kozmetik
                new() { Name = "Lüks Cilt Bakım Seti", Description = "Serum, nemlendirici ve göz kremi içeren premium bakım seti.", Price = 1899.99m, Stock = 35, CategoryId = kozmetik.Id, IsFeatured = true, ImageUrl = "https://placehold.co/400x400/e94560/533483?text=Cilt+Bakım" },
                new() { Name = "Parfüm Collection", Description = "4 farklı koku notası, 50ml'lik özel şişeler.", Price = 2499.99m, Stock = 20, CategoryId = kozmetik.Id, ImageUrl = "https://placehold.co/400x400/e94560/533483?text=Parfüm" },
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
