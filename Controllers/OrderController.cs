using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize]
public class OrderController(IOrderService orderService, ICartService cartService) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Checkout()
    {
        var cartItems = await cartService.GetCartItemsAsync(UserId);
        if (!cartItems.Any())
        {
            TempData["Error"] = "Sepetiniz boş!";
            return RedirectToAction("Index", "Cart");
        }

        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.CartItems = await cartService.GetCartItemsAsync(UserId);
            return View("Checkout", model);
        }

        try
        {
            var order = await orderService.CreateOrderAsync(UserId, model);
            return RedirectToAction("Confirmation", new { id = order.Id });
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Cart");
        }
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await orderService.GetOrderByIdAsync(id, UserId);
        if (order == null) return NotFound();
        return View(order);
    }

    public async Task<IActionResult> History()
    {
        var orders = await orderService.GetUserOrdersAsync(UserId);
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetOrderByIdAsync(id, UserId);
        if (order == null) return NotFound();
        return View(order);
    }
}
