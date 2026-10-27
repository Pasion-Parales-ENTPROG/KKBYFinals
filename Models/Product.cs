
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KKBYFinals.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int ServingSizeGrams { get; set; }
}
