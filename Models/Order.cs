using SQLite;

namespace RestoGestApp.Models;

public enum OrderStatus
{
    Created,
    Processing,
    Ready,
    Delivered,
    Completed,
    Cancelled
}

public class Order
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    public bool IsPaid { get; set; }
    
    [Ignore]
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}
