namespace KKBYFinals.Models.ViewModels;

public class DashboardViewModel
{
    public int PendingOrders { get; set; }
    public int PreparingOrders { get; set; }
    public int ReadyOrders { get; set; }

    public int LowStockItems { get; set; }

    public int UnpaidOrders { get; set; }
    public decimal UnpaidAmount { get; set; }
}
