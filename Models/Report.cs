using SQLite;

namespace RestoGestApp.Models;

public enum ReportType
{
    Sales,
    Inventory,
    CustomerActivity
}

public class Report
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public DateTime GeneratedDate { get; set; }
    
    public ReportType Type { get; set; }
    
    public decimal TotalSales { get; set; }
    
    public int OrderCount { get; set; }
    
    public decimal AverageOrderValue { get; set; }
    
    public List<string> TopSellingItems { get; set; } = new List<string>();
    
    public List<string> TopCustomers { get; set; } = new List<string>();
    
    public string Notes { get; set; } = string.Empty;
}
