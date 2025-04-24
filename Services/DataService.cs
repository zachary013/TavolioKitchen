using RestoGestApp.Helpers;
using RestoGestApp.Models;

namespace RestoGestApp.Services;

public class DataService
{
    private readonly DatabaseService _databaseService;
    
    public DataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    
    // Menu Item Operations
    public async Task<List<MenuItemModel>> GetMenuItemsAsync()
    {
        return await _databaseService.GetMenuItemsAsync();
    }
    
    public async Task<List<MenuItemModel>> GetMenuItemsByCategoryAsync(string category)
    {
        return await _databaseService.GetMenuItemsByCategoryAsync(category);
    }
    
    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _databaseService.GetCategoriesAsync();
    }
    
    public async Task<MenuItemModel?> GetMenuItemAsync(int id)
    {
        return await _databaseService.GetMenuItemAsync(id);
    }
    
    public async Task<int> SaveMenuItemAsync(MenuItemModel item)
    {
        return await _databaseService.SaveMenuItemAsync(item);
    }
    
    // User Operations
    public async Task<List<User>> GetUsersAsync()
    {
        return await _databaseService.GetUsersAsync();
    }
    
    public async Task<User?> GetUserAsync(int id)
    {
        return await _databaseService.GetUserAsync(id);
    }
    
    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        var user = await _databaseService.GetUserByUsernameAsync(username);
        
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            return user;
        }
        
        return null;
    }
    
    public async Task<User?> AuthenticateUserByEmailAsync(string email, string password)
    {
        var user = await _databaseService.GetUserByEmailAsync(email);
        
        if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
        {
            return user;
        }
        
        return null;
    }
    
    public async Task<bool> IsUsernameAvailableAsync(string username)
    {
        var user = await _databaseService.GetUserByUsernameAsync(username);
        return user == null;
    }
    
    public async Task<bool> IsEmailAvailableAsync(string email)
    {
        var user = await _databaseService.GetUserByEmailAsync(email);
        return user == null;
    }
    
    public async Task<int> SaveUserAsync(User user)
    {
        return await _databaseService.SaveUserAsync(user);
    }
    
    // Order Operations
    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _databaseService.GetOrdersAsync();
    }
    
    public async Task<List<Order>> GetOrdersByUserAsync(int userId)
    {
        return await _databaseService.GetOrdersByUserAsync(userId);
    }
    
    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _databaseService.GetOrderAsync(id);
    }
    
    public async Task<int> SaveOrderAsync(Order order)
    {
        return await _databaseService.SaveOrderAsync(order);
    }
    
    // OrderItem Operations
    public async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        return await _databaseService.GetOrderItemsAsync(orderId);
    }
    
    public async Task<int> SaveOrderItemAsync(OrderItem item)
    {
        return await _databaseService.SaveOrderItemAsync(item);
    }
    
    // Reservation Operations
    public async Task<List<Reservation>> GetReservationsAsync()
    {
        return await _databaseService.GetReservationsAsync();
    }
    
    public async Task<List<Reservation>> GetReservationsByUserAsync(int userId)
    {
        return await _databaseService.GetReservationsByUserAsync(userId);
    }
    
    public async Task<Reservation?> GetReservationAsync(int id)
    {
        return await _databaseService.GetReservationAsync(id);
    }
    
    public async Task<bool> IsTableAvailableAsync(int tableNumber, DateTime date, TimeSpan time)
    {
        var reservations = await _databaseService.GetReservationsAsync();
        
        // Check if there's any reservation for the same table, date and time
        return !reservations.Any(r => 
            r.TableNumber == tableNumber && 
            r.ReservationDate.Date == date.Date && 
            Math.Abs((r.ReservationTime - time).TotalMinutes) < 90); // Within 90 minutes
    }
    
    public async Task<int> SaveReservationAsync(Reservation reservation)
    {
        return await _databaseService.SaveReservationAsync(reservation);
    }
    
    public async Task<int> CancelReservationAsync(Reservation reservation)
    {
        reservation.Status = ReservationStatus.Cancelled;
        return await _databaseService.SaveReservationAsync(reservation);
    }
    
    // Report Operations
    public async Task<Report> GenerateReportAsync(DateTime startDate, DateTime endDate, ReportType reportType)
    {
        // Create a new report
        var report = new Report
        {
            StartDate = startDate,
            EndDate = endDate,
            Type = reportType,
            GeneratedDate = DateTime.Now
        };
        
        try
        {
            // Get all orders within the date range
            var orders = await _databaseService.GetOrdersAsync();
            var filteredOrders = orders.Where(o => 
                o.OrderDate >= startDate && 
                o.OrderDate <= endDate).ToList();
            
            // Calculate metrics based on report type
            switch (reportType)
            {
                case ReportType.Sales:
                    // Calculate total sales
                    report.TotalSales = filteredOrders.Sum(o => o.TotalAmount);
                    report.OrderCount = filteredOrders.Count;
                    report.AverageOrderValue = filteredOrders.Count > 0 
                        ? report.TotalSales / filteredOrders.Count 
                        : 0;
                    
                    // Get top selling items
                    var orderItems = new List<OrderItem>();
                    foreach (var order in filteredOrders)
                    {
                        var items = await _databaseService.GetOrderItemsAsync(order.Id);
                        orderItems.AddRange(items);
                    }
                    
                    var topItems = orderItems
                        .GroupBy(i => i.MenuItemId)
                        .Select(g => new { 
                            MenuItemId = g.Key, 
                            Name = g.First().MenuItemName,
                            Quantity = g.Sum(i => i.Quantity),
                            Revenue = g.Sum(i => i.Subtotal)
                        })
                        .OrderByDescending(x => x.Revenue)
                        .Take(5)
                        .ToList();
                    
                    report.TopSellingItems = topItems.Select(i => $"{i.Name} (€{i.Revenue:F2})").ToList();
                    break;
                    
                case ReportType.Inventory:
                    // For inventory report, we would need inventory data
                    // This is a placeholder since we don't have inventory tracking
                    report.Notes = "Inventory tracking not implemented yet.";
                    break;
                    
                case ReportType.CustomerActivity:
                    // Calculate customer metrics
                    var userOrders = filteredOrders
                        .GroupBy(o => o.UserId)
                        .Select(g => new {
                            UserId = g.Key,
                            OrderCount = g.Count(),
                            TotalSpent = g.Sum(o => o.TotalAmount)
                        })
                        .OrderByDescending(x => x.TotalSpent)
                        .Take(5)
                        .ToList();
                    
                    // Get user details
                    var topCustomers = new List<string>();
                    foreach (var userOrder in userOrders)
                    {
                        var user = await _databaseService.GetUserAsync(userOrder.UserId);
                        if (user != null)
                        {
                            topCustomers.Add($"{user.FullName} (€{userOrder.TotalSpent:F2}, {userOrder.OrderCount} orders)");
                        }
                    }
                    
                    report.TopCustomers = topCustomers;
                    break;
            }
            
            return report;
        }
        catch (Exception ex)
        {
            report.Notes = $"Error generating report: {ex.Message}";
            return report;
        }
    }
}
