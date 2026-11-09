using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be 4 to 30 characters.")]
    public string Username { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Role is required.")]
    public string Role { get; set; } = "";
}