using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Required(ErrorMessage = "Order is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid order.")]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [Required(ErrorMessage = "Product is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product.")]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [Range(1, 999999, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "999999.99",
        ErrorMessage = "Subtotal must be greater than zero.")]
    public decimal Subtotal { get; set; }
}
