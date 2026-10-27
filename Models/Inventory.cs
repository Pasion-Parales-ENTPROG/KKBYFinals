
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KKBYFinals.Models;

public class Inventory
{
    [Key]
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
    public int MinStockLevel { get; set; }
}
