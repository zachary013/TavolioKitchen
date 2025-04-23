using SQLite;

namespace RestoGestApp.Models;

public enum UserRole
{
    Client,
    Staff,
    Manager,
    Admin
}

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Unique]
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string FullName { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Bio { get; set; } = string.Empty;
    
    public string ProfileImagePath { get; set; } = "profile_default.png";
}
