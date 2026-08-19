using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize]
public class CartController(ICartService cartService) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
    {
        var items = await cartService.GetCartItemsAsync(UserId);
        var viewModel = new CartViewModel { Items = items };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        await cartService.AddToCartAsync(UserId, productId, quantity);
        TempData["Success"] = "Ürün sepete eklendi!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        await cartService.RemoveFromCartAsync(UserId, productId);
        TempData["Success"] = "Ürün sepetten kaldırıldı.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
    {
        await cartService.UpdateQuantityAsync(UserId, productId, quantity);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetCartCount()
    {
        var count = await cartService.GetCartCountAsync(UserId);
        return Json(new { count });
    }
}
