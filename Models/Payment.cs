using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Required(ErrorMessage = "Order is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid order.")]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [Required(ErrorMessage = "Payment method is required.")]
    [StringLength(30, ErrorMessage = "Payment method cannot exceed 30 characters.")]
    public string Method { get; set; } = "";

    [Required(ErrorMessage = "Payment status is required.")]
    [StringLength(20, ErrorMessage = "Payment status cannot exceed 20 characters.")]
    public string Status { get; set; } = "";

    [Range(typeof(decimal), "0.01", "999999.99",
        ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment date is required.")]
    public DateTime PaymentDate { get; set; }
}
