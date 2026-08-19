using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(ApplicationDbContext context, IOrderService orderService, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Dashboard()
    {
        var viewModel = new AdminDashboardViewModel
        {
            TotalProducts = await context.Products.CountAsync(),
            TotalOrders = await context.Orders.CountAsync(),
            TotalUsers = await userManager.Users.CountAsync(),
            TotalRevenue = await context.Orders.Where(o => o.Status != OrderStatus.Cancelled).SumAsync(o => o.TotalAmount),
            RecentOrders = await context.Orders.OrderByDescending(o => o.OrderDate).Take(10).ToListAsync(),
            PendingOrders = await context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
            LowStockProducts = await context.Products.CountAsync(p => p.Stock < 10)
        };
        return View(viewModel);
    }

    // Product Management
    public async Task<IActionResult> Products()
    {
        var products = await context.Products.Include(p => p.Category).OrderByDescending(p => p.CreatedAt).ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name");
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            product.CreatedAt = DateTime.UtcNow;
            context.Products.Add(product);
            await context.SaveChangesAsync();
            TempData["Success"] = "Ürün başarıyla eklendi!";
            return RedirectToAction("Products");
        }
        ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name");
        return View(product);
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            product.UpdatedAt = DateTime.UtcNow;
            context.Products.Update(product);
            await context.SaveChangesAsync();
            TempData["Success"] = "Ürün başarıyla güncellendi!";
            return RedirectToAction("Products");
        }
        ViewBag.Categories = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            TempData["Success"] = "Ürün silindi.";
        }
        return RedirectToAction("Products");
    }

    // Category Management
    public async Task<IActionResult> Categories()
    {
        var categories = await context.Categories.Include(c => c.Products).ToListAsync();
        return View(categories);
    }

    public IActionResult CreateCategory()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "Kategori eklendi!";
            return RedirectToAction("Categories");
        }
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        if (category != null && !category.Products.Any())
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "Kategori silindi.";
        }
        else
        {
            TempData["Error"] = "Bu kategoride ürünler var, silinemez!";
        }
        return RedirectToAction("Categories");
    }

    // Order Management
    public async Task<IActionResult> Orders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
    {
        await orderService.UpdateOrderStatusAsync(orderId, status);
        TempData["Success"] = "Sipariş durumu güncellendi!";
        return RedirectToAction("Orders");
    }
}
