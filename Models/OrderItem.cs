using SQLite;

namespace RestoGestApp.Models;

public class OrderItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Indexed]
    public int OrderId { get; set; }
    
    public int MenuItemId { get; set; }
    
    public string MenuItemName { get; set; } = string.Empty;
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal Subtotal => Quantity * UnitPrice;
    
    public string Notes { get; set; } = string.Empty;
}
