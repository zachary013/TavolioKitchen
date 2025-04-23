using RestoGestApp.Models;
using RestoGestApp.Helpers;

namespace RestoGestApp.Services;

public class DataService
{
    private readonly DatabaseService _databaseService;
    
    public DataService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    
    // Menu Item Operations
    public Task<List<MenuItemModel>> GetMenuItemsAsync()
    {
        return _databaseService.GetMenuItemsAsync();
    }
    
    public Task<List<MenuItemModel>> GetMenuItemsByCategoryAsync(string category)
    {
        return _databaseService.GetMenuItemsByCategoryAsync(category);
    }
    
    public Task<List<string>> GetCategoriesAsync()
    {
        return _databaseService.GetCategoriesAsync();
    }
    
    public Task<MenuItemModel> GetMenuItemAsync(int id)
    {
        return _databaseService.GetMenuItemAsync(id);
    }
    
    public Task<int> SaveMenuItemAsync(MenuItemModel item)
    {
        return _databaseService.SaveMenuItemAsync(item);
    }
    
    // User Operations
    public Task<List<User>> GetUsersAsync()
    {
        return _databaseService.GetUsersAsync();
    }
    
    public Task<User> GetUserAsync(int id)
    {
        return _databaseService.GetUserAsync(id);
    }
    
    public Task<User> GetUserByUsernameAsync(string username)
    {
        return _databaseService.GetUserByUsernameAsync(username);
    }
    
    public Task<User> GetUserByEmailAsync(string email)
    {
        return _databaseService.GetUserByEmailAsync(email);
    }
    
    public Task<int> SaveUserAsync(User user)
    {
        return _databaseService.SaveUserAsync(user);
    }
    
    // Order Operations
    public Task<List<Order>> GetOrdersAsync()
    {
        return _databaseService.GetOrdersAsync();
    }
    
    public Task<List<Order>> GetOrdersByUserAsync(int userId)
    {
        return _databaseService.GetOrdersByUserAsync(userId);
    }
    
    public Task<Order> GetOrderAsync(int id)
    {
        return _databaseService.GetOrderAsync(id);
    }
    
    public Task<int> SaveOrderAsync(Order order)
    {
        return _databaseService.SaveOrderAsync(order);
    }
    
    // OrderItem Operations
    public Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        return _databaseService.GetOrderItemsAsync(orderId);
    }
    
    public Task<int> SaveOrderItemAsync(OrderItem item)
    {
        return _databaseService.SaveOrderItemAsync(item);
    }
    
    // Reservation Operations
    public Task<List<Reservation>> GetReservationsAsync()
    {
        return _databaseService.GetReservationsAsync();
    }
    
    public Task<List<Reservation>> GetReservationsByUserAsync(int userId)
    {
        return _databaseService.GetReservationsByUserAsync(userId);
    }
    
    public Task<Reservation> GetReservationAsync(int id)
    {
        return _databaseService.GetReservationAsync(id);
    }
    
    public Task<int> SaveReservationAsync(Reservation reservation)
    {
        return _databaseService.SaveReservationAsync(reservation);
    }
    
    public Task<int> DeleteReservationAsync(Reservation reservation)
    {
        return _databaseService.DeleteReservationAsync(reservation);
    }
    
    // Report Operations
    public async Task<Report> GenerateReportAsync(DateTime startDate, DateTime endDate)
    {
        var report = new Report
        {
            Date = DateTime.Now
        };
        
        // Get all orders in the date range
        var orders = await _databaseService.GetOrdersAsync();
        orders = orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList();
        
        report.TotalOrders = orders.Count;
        report.TotalRevenue = orders.Sum(o => o.TotalAmount);
        
        // Get all reservations in the date range
        var reservations = await _databaseService.GetReservationsAsync();
        reservations = reservations.Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate).ToList();
        
        report.TotalReservations = reservations.Count;
        
        // Get revenue by category
        var orderItems = new List<OrderItem>();
        foreach (var order in orders)
        {
            var items = await _databaseService.GetOrderItemsAsync(order.Id);
            orderItems.AddRange(items);
        }
        
        var menuItems = await _databaseService.GetMenuItemsAsync();
        var menuItemsDict = menuItems.ToDictionary(m => m.Id);
        
        var revenueByCategory = new Dictionary<string, decimal>();
        foreach (var item in orderItems)
        {
            if (menuItemsDict.TryGetValue(item.MenuItemId, out var menuItem))
            {
                var category = menuItem.Category;
                var revenue = item.Quantity * item.UnitPrice;
                
                if (revenueByCategory.ContainsKey(category))
                {
                    revenueByCategory[category] += revenue;
                }
                else
                {
                    revenueByCategory[category] = revenue;
                }
            }
        }
        
        report.RevenueByCategory = revenueByCategory;
        
        // Get top selling items
        var topSellingItemIds = orderItems
            .GroupBy(oi => oi.MenuItemId)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Take(5)
            .Select(g => g.Key);
            
        foreach (var id in topSellingItemIds)
        {
            var menuItem = menuItems.FirstOrDefault(m => m.Id == id);
            if (menuItem != null)
            {
                report.TopSellingItems.Add(menuItem);
            }
        }
        
        return report;
    }
    
    // Enhanced authentication methods
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _databaseService.GetUserByUsernameAsync(username);
        
        if (user != null)
        {
            // First try direct comparison (for users created before hashing)
            if (user.Password == password)
            {
                // Update to hashed password
                user.Password = PasswordHelper.HashPassword(password);
                await _databaseService.SaveUserAsync(user);
                return user;
            }
            // Then try hashed comparison
            else if (PasswordHelper.VerifyPassword(password, user.Password))
            {
                return user;
            }
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
    
    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        var orderItems = await _databaseService.GetOrderItemsAsync(orderId);
        return orderItems.Sum(item => item.Quantity * item.UnitPrice);
    }
    
    public async Task<bool> IsTableAvailableAsync(int tableNumber, DateTime date, TimeSpan time)
    {
        var reservations = await _databaseService.GetReservationsAsync();
        return !reservations.Any(r => 
            r.TableNumber == tableNumber && 
            r.ReservationDate.Date == date.Date && 
            Math.Abs((r.ReservationTime - time).TotalHours) < 2 &&
            r.Status != ReservationStatus.Cancelled);
    }
}
