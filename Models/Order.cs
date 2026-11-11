
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "User is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user.")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
    public string CustomerName { get; set; } = "";

    [Required(ErrorMessage = "Order date is required.")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Order status is required.")]
    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string Status { get; set; } = "Pending";

    [Range(typeof(decimal), "0.01", "999999.99",
        ErrorMessage = "Total amount must be greater than zero.")]
    public decimal TotalAmount { get; set; }

    // One order can contain multiple order items
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

   // Payment Information
    [Required]
    [StringLength(30)]
    public string PaymentMethod { get; set; } = "Cash";

    [Required]
    [StringLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid";

    public DateTime? PaymentDate { get; set; }
}
