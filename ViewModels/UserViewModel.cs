using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class UserViewModel : BaseViewModel
{
    [ObservableProperty]
    private User? _currentUser;
    
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
    private void Logout()
    {
        CurrentUser = null;
        IsLoggedIn = false;
        Username = string.Empty;
        Password = string.Empty;
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
}
