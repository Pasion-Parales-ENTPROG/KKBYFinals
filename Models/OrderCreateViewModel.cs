
using System.ComponentModel.DataAnnotations;

namespace KKBYFinals.Models;

public class OrderCreateViewModel
{
    [Required(ErrorMessage = "Please select a staff member.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid staff member.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(50)]
    public string CustomerName { get; set; } = "";

    [Required(ErrorMessage = "Payment method is required.")]
    public string PaymentMethod { get; set; } = "Cash";

    public List<OrderProductInput> Items { get; set; } = new()
    {
        new OrderProductInput()
    };
}

public class OrderProductInput
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
    public int ProductId { get; set; }

    [Range(1, 999999, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;
}
