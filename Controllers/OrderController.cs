using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KKBYFinals.Data;
using KKBYFinals.Models;

namespace KKBYFinals.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

               var order = await _context.Order
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .FirstOrDefaultAsync(o => o.OrderId == id);

    if (order == null)
    {
        return NotFound();
    }

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = new SelectList(
            await _context.User.ToListAsync(),
             "UserId", 
             "Username"
             );

              ViewBag.Products = await _context.Product
        .OrderBy(p => p.Name)
        .ToListAsync();

            return View(new OrderCreateViewModel());
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(OrderCreateViewModel model)
{
    ViewData["UserId"] = new SelectList(
        await _context.User.ToListAsync(),
        "UserId",
        "Username",
        model.UserId
    );

    ViewBag.Products = await _context.Product
        .OrderBy(p => p.Name)
        .ToListAsync();

    if (model.Items == null || !model.Items.Any())
    {
        ModelState.AddModelError("", "Please add at least one product.");
        return View(model);
    }

    var productIds = model.Items
        .Select(i => i.ProductId)
        .Distinct()
        .ToList();

    var products = await _context.Product
        .Where(p => productIds.Contains(p.ProductId))
        .ToDictionaryAsync(p => p.ProductId);

    foreach (var item in model.Items)
    {
        if (!products.ContainsKey(item.ProductId))
        {
            ModelState.AddModelError("", "Please select valid products.");
            break;
        }
    }

    if (!ModelState.IsValid)
    {
        return View(model);
    }

    var requestedItems = model.Items
        .GroupBy(i => i.ProductId)
        .Select(g => new
        {
            ProductId = g.Key,
            Quantity = g.Sum(i => i.Quantity)
        })
        .ToList();

    foreach (var item in requestedItems)
    {
        var inventory = await _context.Inventory
            .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

        if (inventory == null || inventory.Quantity < item.Quantity)
        {
            ModelState.AddModelError(
                "",
                $"Insufficient stock for {products[item.ProductId].Name}."
            );
        }
    }

    if (!ModelState.IsValid)
    {
        return View(model);
    }

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var orderItems = model.Items.Select(item => new OrderItem
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Subtotal = products[item.ProductId].Price * item.Quantity
        }).ToList();

        var order = new Order
        {
            UserId = model.UserId,
            CustomerName = model.CustomerName,
            OrderDate = DateTime.Now,
            Status = "Pending",
            PaymentMethod = model.PaymentMethod,
            PaymentStatus = "Unpaid",
            PaymentDate = null,
            TotalAmount = orderItems.Sum(i => i.Subtotal),
            OrderItems = orderItems
        };

        _context.Order.Add(order);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return RedirectToAction(nameof(Details), new { id = order.OrderId });
    }
    catch
    {
        await transaction.RollbackAsync();

        ModelState.AddModelError(
            "",
            "Unable to save the order. Please try again."
        );

        return View(model);
    }
}





[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateStatus(int id, string newStatus)
{
    var allowedStatuses = new[]
    {
        "Pending", "Preparing", "Ready", "Completed"
    };

    if (!allowedStatuses.Contains(newStatus))
    {
        return BadRequest("Invalid order status.");
    }

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var order = await _context.Order
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
        {
            return NotFound();
        }

        if (order.Status == "Completed")
        {
            TempData["Error"] = "This order is already completed.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (newStatus == "Completed")
        {
            foreach (var item in order.OrderItems)
            {
                var stock = await _context.Inventory
                    .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                if (stock == null || stock.Quantity < item.Quantity)
                {
                    TempData["Error"] =
                        $"Insufficient stock for Product ID {item.ProductId}. Required: {item.Quantity}.";

                    await transaction.RollbackAsync();
                    return RedirectToAction(nameof(Details), new { id });
                }

                stock.Quantity -= item.Quantity;
                stock.LastUpdated = DateTime.Now;
            }
        }

        order.Status = newStatus;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["Success"] = newStatus == "Completed"
            ? "Order completed and all product inventories successfully updated."
            : $"Order status updated to {newStatus}.";

        return RedirectToAction(nameof(Details), new { id });
    }
    catch
    {
        await transaction.RollbackAsync();

        TempData["Error"] = "Unable to update the order status. Please try again.";
        return RedirectToAction(nameof(Details), new { id });
    }
}



[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> MarkAsPaid(int id)
{
    var order = await _context.Order
        .FirstOrDefaultAsync(o => o.OrderId == id);

    if (order == null)
    {
        return NotFound();
    }

    if (order.PaymentStatus == "Paid")
    {
        TempData["Success"] = "This order is already Paid.";
        return RedirectToAction(nameof(Details), new { id });
    }

    order.PaymentStatus = "Paid";
    order.PaymentDate = DateTime.Now;

    await _context.SaveChangesAsync();

    TempData["Success"] = "Payment successfully marked as Paid.";

    return RedirectToAction(nameof(Details), new { id });
}


        // GET: Order/Edit/5
        
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var order = await _context.Order.FindAsync(id);

    if (order == null)
    {
        return NotFound();
    }

    ViewData["UserId"] = new SelectList(
        _context.User, "UserId", "Username", order.UserId);

    return View(order);
}

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,CustomerName,OrderDate,Status,TotalAmount")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(
    _context.User, "UserId", "Username", order.UserId);


            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
