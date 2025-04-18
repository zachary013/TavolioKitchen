using SQLite;

namespace RestoGestApp.Models;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

public class Reservation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public string CustomerName { get; set; } = string.Empty;
    
    public DateTime ReservationDate { get; set; }
    
    public TimeSpan ReservationTime { get; set; }
    
    public int TableNumber { get; set; }
    
    public int NumberOfGuests { get; set; }
    
    public string SpecialRequests { get; set; } = string.Empty;
    
    public ReservationStatus Status { get; set; }
    
    public string ContactPhone { get; set; } = string.Empty;
}
