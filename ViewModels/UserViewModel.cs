using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;
using RestoGestApp.Views;

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
            
            var user = await DataService.GetUserByUsernameAsync(Username);
            
            if (user != null && user.Password == Password)
            {
                CurrentUser = user;
                IsLoggedIn = true;
                await NotificationService.ShowToastAsync($"Welcome, {user.FullName}!");
                
                // Navigate back to profile page after successful login
                await Shell.Current.GoToAsync("//profile");
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
        
        await NotificationService.ShowToastAsync("You have been logged out.");
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
            var existingUser = await DataService.GetUserByUsernameAsync(NewUser.Username);
            if (existingUser != null)
            {
                await NotificationService.ShowAlertAsync("Error", "Username already exists. Please choose a different username.");
                return;
            }
            
            // Set default role for new users
            NewUser.Role = UserRole.Client;
            
            // Save the new user
            await DataService.SaveUserAsync(NewUser);
            
            // Auto-login the new user
            CurrentUser = NewUser;
            IsLoggedIn = true;
            
            await NotificationService.ShowAlertAsync("Success", "Your account has been created successfully!");
            
            // Navigate back to profile page
            await Shell.Current.GoToAsync("//profile");
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
