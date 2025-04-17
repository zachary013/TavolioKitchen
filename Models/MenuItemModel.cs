using SQLite;

namespace RestoGestApp.Models;

public class MenuItemModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public string Category { get; set; } = string.Empty;
    
    public string ImagePath { get; set; } = string.Empty;
    
    public bool IsAvailable { get; set; } = true;
}
