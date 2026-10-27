
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KKBYFinals.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "";
    public decimal TotalAmount { get; set; }
}
