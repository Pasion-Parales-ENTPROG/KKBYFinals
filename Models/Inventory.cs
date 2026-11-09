using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class Inventory
{
    [Key]
    public int InventoryId { get; set; }

    [Required(ErrorMessage = "Please select a product.")]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [Range(0, 999999, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    public DateTime LastUpdated { get; set; }

    [Range(0, 999999, ErrorMessage = "Minimum stock level cannot be negative.")]
    public int MinStockLevel { get; set; }
}