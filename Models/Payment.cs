using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KKBYFinals.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
    public string Method { get; set; } = "";
    public string Status { get; set; } = "";
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}