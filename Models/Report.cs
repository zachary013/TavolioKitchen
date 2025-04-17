namespace RestoGestApp.Models;

public class Report
{
    public DateTime Date { get; set; }
    
    public decimal TotalRevenue { get; set; }
    
    public int TotalOrders { get; set; }
    
    public int TotalReservations { get; set; }
    
    public Dictionary<string, decimal> RevenueByCategory { get; set; } = new Dictionary<string, decimal>();
    
    public Dictionary<string, int> OrdersByStatus { get; set; } = new Dictionary<string, int>();
    
    public List<MenuItemModel> TopSellingItems { get; set; } = new List<MenuItemModel>();
}
