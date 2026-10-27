using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KKBYFinals.Models;

namespace KKBYFinals.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{

public DbSet<KKBYFinals.Models.User> User { get; set; } = default!;

public DbSet<KKBYFinals.Models.Product> Product { get; set; } = default!;

public DbSet<KKBYFinals.Models.Inventory> Inventory { get; set; } = default!;

public DbSet<KKBYFinals.Models.Order> Order { get; set; } = default!;
}
