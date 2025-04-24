using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;
using RestoGestApp.Helpers;
using System.Text;

namespace RestoGestApp.ViewModels;

public partial class UserViewModel : BaseViewModel
{
    [ObservableProperty]
    private User? _currentUser;
    
    [ObservableProperty]
    private User _newUser = new User();
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    private string _confirmPassword = string.Empty;
    
    [ObservableProperty]
    private bool _isLoggedIn;
    
    [ObservableProperty]
    private List<Order> _recentOrders = new List<Order>();
    
    public UserViewModel(DataService dataService, NotificationService notificationService) 
        : base(dataService, notificationService)
    {
        Title = "Profile";
        IsLoggedIn = false;
        InitializeNewUser();
        
        // Check if there's a saved user session
        LoadUserSession();
    }
    
    public void InitializeNewUser()
    {
        NewUser = new User
        {
            Username = string.Empty,
            Password = string.Empty,
            FullName = string.Empty,
            Email = string.Empty,
            Phone = string.Empty,
            ProfileImagePath = "profile_default.png",
            Role = UserRole.Client // Default role for new users
        };
        ConfirmPassword = string.Empty;
    }
    
    private void LoadUserSession()
    {
        try
        {
            // Check if we have a saved user ID in preferences
            var userId = Preferences.Get("CurrentUserId", 0);
            if (userId > 0)
            {
                // Load the user asynchronously
                Task.Run(async () =>
                {
                    var user = await DataService.GetUserAsync(userId);
                    if (user != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            CurrentUser = user;
                            IsLoggedIn = true;
                            
                            // Load recent orders
                            LoadRecentOrders(userId);
                        });
                    }
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user session: {ex.Message}");
        }
    }
    
    private void LoadRecentOrders(int userId)
    {
        Task.Run(async () =>
        {
            try
            {
                var orders = await DataService.GetOrdersByUserAsync(userId);
                
                // Get the most recent 5 orders
                var recentOrders = orders.OrderByDescending(o => o.OrderDate).Take(5).ToList();
                
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RecentOrders = recentOrders;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading recent orders: {ex.Message}");
            }
        });
    }
    
    private void SaveUserSession(User user)
    {
        try
        {
            Preferences.Set("CurrentUserId", user.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving user session: {ex.Message}");
        }
    }
    
    private void ClearUserSession()
    {
        try
        {
            Preferences.Remove("CurrentUserId");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing user session: {ex.Message}");
        }
    }
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;
            
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await NotificationService.ShowAlertAsync("Error", "Please enter both email and password.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            var user = await DataService.AuthenticateUserByEmailAsync(Email, Password);
            
            if (user != null)
            {
                CurrentUser = user;
                IsLoggedIn = true;
                
                // Save user session
                SaveUserSession(user);
                
                // Load recent orders
                LoadRecentOrders(user.Id);
                
                await NotificationService.ShowToastAsync($"Welcome, {user.FullName}!");
                
                // Navigate to main app
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                await NotificationService.ShowAlertAsync("Login Failed", "Invalid email or password.");
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to login: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task LogoutAsync()
    {
        CurrentUser = null;
        IsLoggedIn = false;
        Email = string.Empty;
        Password = string.Empty;
        RecentOrders.Clear();
        
        // Clear user session
        ClearUserSession();
        
        await NotificationService.ShowToastAsync("You have been logged out.");
        
        // Navigate to login
        await Shell.Current.GoToAsync("//login");
    }
    
    [RelayCommand]
    private async Task UpdateProfileAsync()
    {
        if (IsBusy || CurrentUser == null)
            return;
            
        try
        {
            IsBusy = true;
            
            await DataService.SaveUserAsync(CurrentUser);
            
            await NotificationService.ShowAlertAsync("Success", "Your profile has been updated successfully!");
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to update profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task ViewOrderDetailsAsync(Order order)
    {
        if (order == null)
            return;
            
        // Get the full order details including items
        var fullOrder = await DataService.GetOrderAsync(order.Id);
        
        if (fullOrder == null)
        {
            await NotificationService.ShowAlertAsync("Error", "Order details not found.");
            return;
        }
        
        // Build the order details message
        var message = new StringBuilder();
        message.AppendLine($"Order #{fullOrder.Id}");
        message.AppendLine($"Date: {fullOrder.OrderDate:MMMM d, yyyy}");
        message.AppendLine($"Status: {fullOrder.Status}");
        message.AppendLine();
        message.AppendLine("Items:");
        
        foreach (var item in fullOrder.Items)
        {
            message.AppendLine($"- {item.MenuItemName} x{item.Quantity} (€{item.UnitPrice:F2})");
        }
        
        message.AppendLine();
        message.AppendLine($"Total: €{fullOrder.TotalAmount:F2}");
        
        await NotificationService.ShowAlertAsync("Order Details", message.ToString());
    }
    
    [RelayCommand]
    private async Task SignupAsync()
    {
        if (IsBusy)
            return;
        
        // Validate input
        if (string.IsNullOrWhiteSpace(NewUser.FullName) || 
            string.IsNullOrWhiteSpace(NewUser.Email) ||
            string.IsNullOrWhiteSpace(NewUser.Phone) ||
            string.IsNullOrWhiteSpace(NewUser.Password))
        {
            await NotificationService.ShowAlertAsync("Error", "Please fill in all required fields.");
            return;
        }
        
        // Validate password confirmation
        if (NewUser.Password != ConfirmPassword)
        {
            await NotificationService.ShowAlertAsync("Error", "Passwords do not match.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            // Generate a username from email (before @ symbol)
            NewUser.Username = NewUser.Email.Split('@')[0];
            
            // Check if email already exists
            bool isEmailAvailable = await DataService.IsEmailAvailableAsync(NewUser.Email);
            if (!isEmailAvailable)
            {
                await NotificationService.ShowAlertAsync("Error", "Email already exists. Please use a different email address.");
                return;
            }
            
            // Check if username already exists and generate a unique one if needed
            bool isUsernameAvailable = await DataService.IsUsernameAvailableAsync(NewUser.Username);
            if (!isUsernameAvailable)
            {
                // Add a random number to make it unique
                Random random = new Random();
                NewUser.Username = $"{NewUser.Username}{random.Next(1000, 9999)}";
            }
            
            // Set default role for new users
            NewUser.Role = UserRole.Client;
            
            // Hash the password before saving
            string plainPassword = NewUser.Password;
            NewUser.Password = PasswordHelper.HashPassword(plainPassword);
            
            // Save the new user
            await DataService.SaveUserAsync(NewUser);
            
            // Auto-login the new user
            CurrentUser = NewUser;
            IsLoggedIn = true;
            
            // Save user session
            SaveUserSession(NewUser);
            
            await NotificationService.ShowAlertAsync("Success", "Your account has been created successfully!");
            
            // Navigate to main app
            await Shell.Current.GoToAsync("//main");
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to create account: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task NavigateToLoginAsync()
    {
        try
        {
            // Reset fields
            InitializeNewUser();
            
            // Navigate to login page
            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
        }
    }
    
    [RelayCommand]
    private async Task NavigateToSignupAsync()
    {
        try
        {
            // Reset fields
            Email = string.Empty;
            Password = string.Empty;
            
            // Navigate to signup page
            await Shell.Current.GoToAsync("//signup");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
        }
    }
    
    [RelayCommand]
    private async Task ViewAllOrdersAsync()
    {
        if (CurrentUser == null)
            return;
            
        // This would navigate to a dedicated orders page
        // For now, we'll just show a message
        await NotificationService.ShowAlertAsync("Coming Soon", "The full order history feature will be available in the next update.");
    }
}
