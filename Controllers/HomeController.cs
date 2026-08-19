using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            FeaturedProducts = await context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.IsFeatured)
                .Take(8)
                .ToListAsync(),
            Categories = await context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync(),
            NewProducts = await context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync()
        };
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
