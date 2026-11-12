using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KKBYFinals.Data;
using KKBYFinals.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace KKBYFinals.Controllers;

[Authorize(Roles = "Admin,Cashier,Kitchen Staff")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = new DashboardViewModel
        {
            PendingOrders = await _context.Order
                .CountAsync(o => o.Status == "Pending"),

            PreparingOrders = await _context.Order
                .CountAsync(o => o.Status == "Preparing"),

            ReadyOrders = await _context.Order
                .CountAsync(o => o.Status == "Ready"),

            LowStockItems = await _context.Inventory
                .CountAsync(i => i.Quantity <= i.MinStockLevel),

            UnpaidOrders = await _context.Order
                .CountAsync(o => o.PaymentStatus == "Unpaid"),

            UnpaidAmount = await _context.Order
                .Where(o => o.PaymentStatus == "Unpaid")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0
        };

        return View(dashboard);
    }
}
