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
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Username,Password,Role")] User user)
{
    if (ModelState.IsValid)
    {
        user.PasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<User>()
    .HashPassword(user, user.Password);

        _context.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    return View(user);
}


        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,Role")] User user)
{
    if (id != user.UserId)
    {
        return NotFound();
    }

    var existingUser = await _context.User.FindAsync(id);

    if (existingUser == null)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        existingUser.Username = user.Username;
        existingUser.Role = user.Role;

        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            existingUser.PasswordHash =
    new Microsoft.AspNetCore.Identity.PasswordHasher<User>()
        .HashPassword(existingUser, user.Password);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    return View(user);
}


        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
