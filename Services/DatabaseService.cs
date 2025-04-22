using SQLite;
using System.Diagnostics;

namespace RestoGestApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;
    private bool _isInitialized = false;
    
    public async Task InitializeAsync()
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
            
            var databasePath = Path.Combine(dataDir, "tavoliokitchen.db");
            Debug.WriteLine($"Database path: {databasePath}");
            
            _database = new SQLiteAsyncConnection(databasePath);
            
            // Create tables for all our models
            await _database.CreateTableAsync<Models.MenuItemModel>();
            await _database.CreateTableAsync<Models.User>();
            await _database.CreateTableAsync<Models.Order>();
            await _database.CreateTableAsync<Models.OrderItem>();
            await _database.CreateTableAsync<Models.Reservation>();
            
            // Seed the database with initial data
            await SeedDatabaseAsync();
            
            _isInitialized = true;
            Debug.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }
    
    private async Task SeedDatabaseAsync()
    {
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
            
        try
        {
            // Check if we already have data
            var menuItemCount = await _database.Table<Models.MenuItemModel>().CountAsync();
            var userCount = await _database.Table<Models.User>().CountAsync();
            
            if (menuItemCount == 0)
            {
                // Seed menu items
                var menuItems = new List<Models.MenuItemModel>
                {
                    // Pizza items with pizza.png image
                    new Models.MenuItemModel { Name = "Margherita Pizza", Description = "Tomato sauce, mozzarella and basil", Price = 8.90m, Category = "Pizza", ImagePath = "pizza.png", IsAvailable = true },
                    new Models.MenuItemModel { Name = "Pepperoni Pizza", Description = "Tomato sauce, mozzarella and pepperoni", Price = 9.90m, Category = "Pizza", ImagePath = "pizza.png", IsAvailable = true },
                    new Models.MenuItemModel { Name = "Vegetarian Pizza", Description = "Tomato sauce, mozzarella and mixed vegetables", Price = 10.50m, Category = "Pizza", ImagePath = "pizza.png", IsAvailable = true },
                    
                    // Other items with default image
                    new Models.MenuItemModel { Name = "Spaghetti Bolognese", Description = "Spaghetti with beef ragù", Price = 9.50m, Category = "Pasta", ImagePath = "pasta.png", IsAvailable = true },
                    new Models.MenuItemModel { Name = "Lasagna", Description = "Layers of pasta with beef ragù and béchamel sauce", Price = 11.90m, Category = "Pasta", ImagePath = "pasta.png", IsAvailable = true },
                    new Models.MenuItemModel { Name = "Carbonara", Description = "Spaghetti with eggs, pecorino cheese, guanciale and black pepper", Price = 10.90m, Category = "Pasta", ImagePath = "pasta.png", IsAvailable = true },
                };
                
                foreach (var item in menuItems)
                {
                    await _database.InsertAsync(item);
                }
                
                Debug.WriteLine($"Seeded {menuItems.Count} menu items");
            }
            
            if (userCount == 0)
            {
                // Seed users
                var users = new List<Models.User>
                {
                    new Models.User { Username = "admin", Password = "admin123", FullName = "Admin User", Role = Models.UserRole.Admin, Email = "admin@tavoliokitchen.com", Phone = "123-456-7890" },
                    new Models.User { Username = "manager", Password = "manager123", FullName = "Manager User", Role = Models.UserRole.Manager, Email = "manager@tavoliokitchen.com", Phone = "123-456-7891" },
                    new Models.User { Username = "staff", Password = "staff123", FullName = "Staff User", Role = Models.UserRole.Staff, Email = "staff@tavoliokitchen.com", Phone = "123-456-7892" },
                    new Models.User { Username = "client", Password = "client123", FullName = "John Doe", Role = Models.UserRole.Client, Email = "john.doe@example.com", Phone = "123-456-7893" }
                };
                
                foreach (var user in users)
                {
                    await _database.InsertAsync(user);
                }
                
                Debug.WriteLine($"Seeded {users.Count} users");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
    
    // Menu Item Operations
    public async Task<List<Models.MenuItemModel>> GetMenuItemsAsync()
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.MenuItemModel>().ToListAsync();
    }
    
    public async Task<List<Models.MenuItemModel>> GetMenuItemsByCategoryAsync(string category)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.MenuItemModel>().Where(m => m.Category == category).ToListAsync();
    }
    
    public async Task<List<string>> GetCategoriesAsync()
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        var items = await _database.Table<Models.MenuItemModel>().ToListAsync();
        return items.Select(m => m.Category).Distinct().ToList();
    }
    
    public async Task<Models.MenuItemModel?> GetMenuItemAsync(int id)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.MenuItemModel>().Where(m => m.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveMenuItemAsync(Models.MenuItemModel item)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }
        else
        {
            return await _database.InsertAsync(item);
        }
    }
    
    public async Task<int> DeleteMenuItemAsync(Models.MenuItemModel item)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.DeleteAsync(item);
    }
    
    // User Operations
    public async Task<List<Models.User>> GetUsersAsync()
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.User>().ToListAsync();
    }
    
    public async Task<Models.User?> GetUserAsync(int id)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.User>().Where(u => u.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<Models.User?> GetUserByUsernameAsync(string username)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.User>().Where(u => u.Username == username).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveUserAsync(Models.User user)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        if (user.Id != 0)
        {
            return await _database.UpdateAsync(user);
        }
        else
        {
            return await _database.InsertAsync(user);
        }
    }
    
    // Order Operations
    public async Task<List<Models.Order>> GetOrdersAsync()
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.Order>().ToListAsync();
    }
    
    public async Task<List<Models.Order>> GetOrdersByUserAsync(int userId)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.Order>().Where(o => o.UserId == userId).ToListAsync();
    }
    
    public async Task<Models.Order?> GetOrderAsync(int id)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        var order = await _database.Table<Models.Order>().Where(o => o.Id == id).FirstOrDefaultAsync();
        if (order != null)
        {
            order.Items = await GetOrderItemsAsync(order.Id);
        }
        return order;
    }
    
    public async Task<int> SaveOrderAsync(Models.Order order)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        if (order.Id != 0)
        {
            return await _database.UpdateAsync(order);
        }
        else
        {
            return await _database.InsertAsync(order);
        }
    }
    
    // OrderItem Operations
    public async Task<List<Models.OrderItem>> GetOrderItemsAsync(int orderId)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.OrderItem>().Where(oi => oi.OrderId == orderId).ToListAsync();
    }
    
    public async Task<int> SaveOrderItemAsync(Models.OrderItem item)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        if (item.Id != 0)
        {
            return await _database.UpdateAsync(item);
        }
        else
        {
            return await _database.InsertAsync(item);
        }
    }
    
    // Reservation Operations
    public async Task<List<Models.Reservation>> GetReservationsAsync()
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.Reservation>().ToListAsync();
    }
    
    public async Task<List<Models.Reservation>> GetReservationsByUserAsync(int userId)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.Reservation>().Where(r => r.UserId == userId).ToListAsync();
    }
    
    public async Task<Models.Reservation?> GetReservationAsync(int id)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.Table<Models.Reservation>().Where(r => r.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task<int> SaveReservationAsync(Models.Reservation reservation)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        if (reservation.Id != 0)
        {
            return await _database.UpdateAsync(reservation);
        }
        else
        {
            return await _database.InsertAsync(reservation);
        }
    }
    
    public async Task<int> DeleteReservationAsync(Models.Reservation reservation)
    {
        await InitializeAsync();
        if (_database == null)
            throw new InvalidOperationException("Database not initialized");
        return await _database.DeleteAsync(reservation);
    }
}
