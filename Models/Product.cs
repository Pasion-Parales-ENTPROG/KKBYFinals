using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KKBYFinals.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(50, ErrorMessage = "Product name cannot exceed 50 characters.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = "";

    [Range(0.01, 999999, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    public int ServingSizeGrams { get; set; }
}