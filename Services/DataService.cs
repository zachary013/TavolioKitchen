using SQLite;
using RestoGestApp.Models;
using System.Linq;

namespace RestoGestApp.Services;

public class DataService
{
    private SQLiteAsyncConnection _database = null!;
    private bool _isInitialized = false;
    
    public DataService()
    {
    }
    
    private async Task InitializeAsync()
    {
        if (_isInitialized)
            return;
        
        try
        {
            // Ensure the directory exists
            var dataDir = FileSystem.AppDataDirectory;
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            
            var databasePath = Path.Combine(dataDir, "restogest.db");
            _database = new SQLiteAsyncConnection(databasePath);
            
            await _database.CreateTableAsync<MenuItemModel>();
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Order>();
            await _database.CreateTableAsync<OrderItem>();
            await _database.CreateTableAsync<Reservation>();
            
            await SeedDataAsync();
            
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization error: {ex.Message}");
            throw;
        }
    }
    
    private async Task SeedDataAsync()
    {
        try
        {
            // Check if we already have data
            var menuItemCount = await _database.Table<MenuItemModel>().CountAsync();
            var userCount = await _database.Table<User>().CountAsync();
            
            if (menuItemCount == 0)
            {
                // Seed menu items
                var menuItems = new List<MenuItemModel>
                {
                    new MenuItemModel { Name = "Wakame Salad", Description = "Fresh seaweed salad with sesame dressing", Price = 3.90m, Category = "Salads", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Caesar Salad", Description = "Romaine lettuce with Caesar dressing, croutons and parmesan", Price = 5.50m, Category = "Salads", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Greek Salad", Description = "Tomatoes, cucumbers, olives, feta cheese and olive oil", Price = 4.90m, Category = "Salads", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    
                    new MenuItemModel { Name = "Margherita Pizza", Description = "Tomato sauce, mozzarella and basil", Price = 8.90m, Category = "Pizzas", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Pepperoni Pizza", Description = "Tomato sauce, mozzarella and pepperoni", Price = 9.90m, Category = "Pizzas", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Vegetarian Pizza", Description = "Tomato sauce, mozzarella and mixed vegetables", Price = 10.50m, Category = "Pizzas", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    
                    new MenuItemModel { Name = "Spaghetti Bolognese", Description = "Spaghetti with beef ragù", Price = 9.50m, Category = "Pasta", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Lasagna", Description = "Layers of pasta with beef ragù and béchamel sauce", Price = 11.90m, Category = "Pasta", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Carbonara", Description = "Spaghetti with eggs, pecorino cheese, guanciale and black pepper", Price = 10.90m, Category = "Pasta", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    
                    new MenuItemModel { Name = "Tiramisu", Description = "Coffee-flavoured Italian dessert", Price = 5.90m, Category = "Desserts", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Cheesecake", Description = "New York style cheesecake", Price = 5.50m, Category = "Desserts", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Ice Cream", Description = "Vanilla, chocolate and strawberry ice cream", Price = 4.50m, Category = "Desserts", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    
                    new MenuItemModel { Name = "Coca Cola", Description = "330ml can", Price = 2.50m, Category = "Drinks", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Mineral Water", Description = "500ml bottle", Price = 1.90m, Category = "Drinks", ImagePath = "dotnet_bot.png", IsAvailable = true },
                    new MenuItemModel { Name = "Orange Juice", Description = "Fresh orange juice", Price = 3.50m, Category = "Drinks", ImagePath = "dotnet_bot.png", IsAvailable = true }
                };
                
                foreach (var item in menuItems)
                {
                    await _database.InsertAsync(item);
                }
            }
            
            if (userCount == 0)
            {
                // Seed users
                var users = new List<User>
                {
                    new User { Username = "admin", Password = "admin123", FullName = "Admin User", Role = UserRole.Admin, Email = "admin@restogest.com", Phone = "123-456-7890" },
                    new User { Username = "manager", Password = "manager123", FullName = "Manager User", Role = UserRole.Manager, Email = "manager@restogest.com", Phone = "123-456-7891" },
                    new User { Username = "staff1", Password = "staff123", FullName = "Staff User", Role = UserRole.Staff, Email = "staff@restogest.com", Phone = "123-456-7892" },
                    new User { Username = "client1", Password = "client123", FullName = "Client User", Role = UserRole.Client, Email = "client@example.com", Phone = "123-456-7893" }
                };
                
                foreach (var user in users)
                {
                    await _database.InsertAsync(user);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding error: {ex.Message}");
            throw;
        }
    }
    
    // MenuItemModel operations
    public async Task<List<MenuItemModel>> GetMenuItemsAsync()
    {
        await InitializeAsync();
        return await _database.Table<MenuItemModel>().ToListAsync();
    }
    
    public async Task<List<MenuItemModel>> GetMenuItemsByCategoryAsync(string category)
    {
        await InitializeAsync();
        return await _database.Table<MenuItemModel>().Where(m => m.Category == category).ToListAsync();
    }
    
    public async Task<List<string>> GetCategoriesAsync()
    {
        await InitializeAsync();
        var items = await _database.Table<MenuItemModel>().ToListAsync();
        return items.Select(m => m.Category).Distinct().ToList();
    }
    
    public async Task<MenuItemModel?> GetMenuItemAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<MenuItemModel>().Where(m => m.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveMenuItemAsync(MenuItemModel item)
    {
        await InitializeAsync();
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }
        else
        {
            return await _database.InsertAsync(item);
        }
    }
    
    public async Task<int> DeleteMenuItemAsync(MenuItemModel item)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(item);
    }
    
    // User operations
    public async Task<List<User>> GetUsersAsync()
    {
        await InitializeAsync();
        return await _database.Table<User>().ToListAsync();
    }
    
    public async Task<User?> GetUserAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        await InitializeAsync();
        return await _database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveUserAsync(User user)
    {
        await InitializeAsync();
        if (user.Id != 0)
        {
            return await _database.UpdateAsync(user);
        }
        else
        {
            return await _database.InsertAsync(user);
        }
    }
    
    public async Task<int> DeleteUserAsync(User user)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(user);
    }
    
    // Order operations
    public async Task<List<Order>> GetOrdersAsync()
    {
        await InitializeAsync();
        return await _database.Table<Order>().ToListAsync();
    }
    
    public async Task<List<Order>> GetOrdersByUserAsync(int userId)
    {
        await InitializeAsync();
        return await _database.Table<Order>().Where(o => o.UserId == userId).ToListAsync();
    }
    
    public async Task<Order?> GetOrderAsync(int id)
    {
        await InitializeAsync();
        var order = await _database.Table<Order>().Where(o => o.Id == id).FirstOrDefaultAsync();
        if (order != null)
        {
            order.Items = await GetOrderItemsAsync(order.Id);
        }
        return order;
    }
    
    public async Task<int> SaveOrderAsync(Order order)
    {
        await InitializeAsync();
        if (order.Id != 0)
        {
            return await _database.UpdateAsync(order);
        }
        else
        {
            return await _database.InsertAsync(order);
        }
    }
    
    public async Task<int> DeleteOrderAsync(Order order)
    {
        await InitializeAsync();
        // First delete all order items
        var orderItems = await GetOrderItemsAsync(order.Id);
        foreach (var item in orderItems)
        {
            await _database.DeleteAsync(item);
        }
        
        // Then delete the order
        return await _database.DeleteAsync(order);
    }
    
    // OrderItem operations
    public async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        await InitializeAsync();
        return await _database.Table<OrderItem>().Where(oi => oi.OrderId == orderId).ToListAsync();
    }
    
    public async Task<OrderItem?> GetOrderItemAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<OrderItem>().Where(oi => oi.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveOrderItemAsync(OrderItem item)
    {
        await InitializeAsync();
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }
        else
        {
            return await _database.InsertAsync(item);
        }
    }
    
    public async Task<int> DeleteOrderItemAsync(OrderItem item)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(item);
    }
    
    // Reservation operations
    public async Task<List<Reservation>> GetReservationsAsync()
    {
        await InitializeAsync();
        return await _database.Table<Reservation>().ToListAsync();
    }
    
    public async Task<List<Reservation>> GetReservationsByUserAsync(int userId)
    {
        await InitializeAsync();
        return await _database.Table<Reservation>().Where(r => r.UserId == userId).ToListAsync();
    }
    
    public async Task<Reservation?> GetReservationAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<Reservation>().Where(r => r.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveReservationAsync(Reservation reservation)
    {
        await InitializeAsync();
        if (reservation.Id != 0)
        {
            return await _database.UpdateAsync(reservation);
        }
        else
        {
            return await _database.InsertAsync(reservation);
        }
    }
    
    public async Task<int> DeleteReservationAsync(Reservation reservation)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(reservation);
    }
    
    // Report operations
    public async Task<Report> GenerateReportAsync(DateTime startDate, DateTime endDate)
    {
        await InitializeAsync();
        
        var report = new Report
        {
            Date = DateTime.Now
        };
        
        // Get all orders in the date range
        var orders = await _database.Table<Order>()
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .ToListAsync();
            
        report.TotalOrders = orders.Count;
        report.TotalRevenue = orders.Sum(o => o.TotalAmount);
        
        // Get all reservations in the date range
        var reservations = await _database.Table<Reservation>()
            .Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate)
            .ToListAsync();
            
        report.TotalReservations = reservations.Count;
        
        // Get revenue by category
        foreach (var order in orders)
        {
            var orderItems = await GetOrderItemsAsync(order.Id);
            foreach (var item in orderItems)
            {
                var menuItem = await GetMenuItemAsync(item.MenuItemId);
                if (menuItem != null)
                {
                    if (!report.RevenueByCategory.ContainsKey(menuItem.Category))
                    {
                        report.RevenueByCategory[menuItem.Category] = 0;
                    }
                    
                    report.RevenueByCategory[menuItem.Category] += item.Subtotal;
                }
            }
        }
        
        // Get orders by status
        foreach (var status in Enum.GetValues(typeof(OrderStatus)))
        {
            var statusString = status.ToString();
            var count = orders.Count(o => o.Status == (OrderStatus)status);
            if (statusString != null)
            {
                report.OrdersByStatus[statusString] = count;
            }
        }
        
        // Get top selling items
        var allOrderItems = new List<OrderItem>();
        foreach (var order in orders)
        {
            allOrderItems.AddRange(await GetOrderItemsAsync(order.Id));
        }
        
        var topSellingItemIds = allOrderItems
            .GroupBy(oi => oi.MenuItemId)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Take(5)
            .Select(g => g.Key);
            
        foreach (var id in topSellingItemIds)
        {
            var menuItem = await GetMenuItemAsync(id);
            if (menuItem != null)
            {
                report.TopSellingItems.Add(menuItem);
            }
        }
        
        return report;
    }
}
