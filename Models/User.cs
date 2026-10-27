
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KKBYFinals.Models;

public class User
{

    [Key]
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
}