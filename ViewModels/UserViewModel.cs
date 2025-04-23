using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;
using RestoGestApp.Helpers;

namespace RestoGestApp.ViewModels;

public partial class UserViewModel : BaseViewModel
{
    [ObservableProperty]
    private User? _currentUser;
    
    [ObservableProperty]
    private User _newUser = new User();
    
    [ObservableProperty]
    private string _username = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    private bool _isLoggedIn;
    
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
            Role = UserRole.Client // Default role for new users
        };
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
                            
                            // Notify the shell about authentication state
                            if (Application.Current?.MainPage is AppShell shell)
                            {
                                _ = shell.HandleAuthenticationChanged(true);
                            }
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
            
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            await NotificationService.ShowAlertAsync("Error", "Please enter both username and password.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            var user = await DataService.AuthenticateUserAsync(Username, Password);
            
            if (user != null)
            {
                CurrentUser = user;
                IsLoggedIn = true;
                
                // Save user session
                SaveUserSession(user);
                
                await NotificationService.ShowToastAsync($"Welcome, {user.FullName}!");
                
                // Notify the shell about authentication state
                if (Application.Current?.MainPage is AppShell shell)
                {
                    await shell.HandleAuthenticationChanged(true);
                }
            }
            else
            {
                await NotificationService.ShowAlertAsync("Login Failed", "Invalid username or password.");
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
        Username = string.Empty;
        Password = string.Empty;
        
        // Clear user session
        ClearUserSession();
        
        await NotificationService.ShowToastAsync("You have been logged out.");
        
        // Notify the shell about authentication state
        if (Application.Current?.MainPage is AppShell shell)
        {
            await shell.HandleAuthenticationChanged(false);
        }
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
    private async Task SignupAsync()
    {
        if (IsBusy)
            return;
        
        // Validate input
        if (string.IsNullOrWhiteSpace(NewUser.Username) || 
            string.IsNullOrWhiteSpace(NewUser.Password) || 
            string.IsNullOrWhiteSpace(NewUser.FullName) || 
            string.IsNullOrWhiteSpace(NewUser.Email))
        {
            await NotificationService.ShowAlertAsync("Error", "Please fill in all required fields.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            // Check if username already exists
            bool isUsernameAvailable = await DataService.IsUsernameAvailableAsync(NewUser.Username);
            if (!isUsernameAvailable)
            {
                await NotificationService.ShowAlertAsync("Error", "Username already exists. Please choose a different username.");
                return;
            }
            
            // Check if email already exists
            bool isEmailAvailable = await DataService.IsEmailAvailableAsync(NewUser.Email);
            if (!isEmailAvailable)
            {
                await NotificationService.ShowAlertAsync("Error", "Email already exists. Please use a different email address.");
                return;
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
            
            // Notify the shell about authentication state
            if (Application.Current?.MainPage is AppShell shell)
            {
                await shell.HandleAuthenticationChanged(true);
            }
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
        await Shell.Current.GoToAsync("//login");
    }
    
    [RelayCommand]
    private async Task NavigateToSignupAsync()
    {
        await Shell.Current.GoToAsync("//signup");
    }
}
