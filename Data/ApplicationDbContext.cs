using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KKBYFinals.Models;

namespace KKBYFinals.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{

public DbSet<KKBYFinals.Models.User> User { get; set; } = default!;
}
