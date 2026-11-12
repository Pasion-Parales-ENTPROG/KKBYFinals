namespace KKBYFinals.Models.ViewModels;

public class LiveStockViewModel
{
    public string ProductName { get; set; } = "";

    public decimal Price { get; set; }

    public int RemainingQuantity { get; set; }

    public string Availability { get; set; } = "";
}
