using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KKBYFinals.Data;
using KKBYFinals.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace KKBYFinals.Controllers;

[AllowAnonymous]
public class LiveStockController : Controller
{
    private readonly ApplicationDbContext _context;

    public LiveStockController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var inventory = await _context.Inventory
            .Include(i => i.Product)
            .Where(i => i.Product != null)
            .ToListAsync();

        var liveStock = inventory.Select(i => new LiveStockViewModel
{
    ProductName = i.Product!.Name,
    Price = i.Product.Price,
    RemainingQuantity = i.Quantity,
    Availability = i.Quantity > 0
        ? "Available"
        : "Out of Stock"
}).ToList();

        return View(liveStock);
    }
}
