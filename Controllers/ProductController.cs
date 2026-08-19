using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

public class ProductController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? search, int? categoryId, string? sortBy, int page = 1)
    {
        var query = context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        query = sortBy switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name" => query.OrderBy(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        const int pageSize = 12;
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new ProductListViewModel
        {
            Products = products,
            Categories = await context.Categories.Where(c => c.ParentCategoryId == null).ToListAsync(),
            SearchTerm = search,
            SelectedCategoryId = categoryId,
            SortBy = sortBy,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (product == null) return NotFound();

        ViewBag.RelatedProducts = await context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.IsActive)
            .Take(4)
            .ToListAsync();

        return View(product);
    }
}
