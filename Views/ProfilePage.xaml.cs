using RestoGestApp.Services;
using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly UserViewModel _viewModel;
    private readonly AuthGuardService _authGuard;
    
    public ProfilePage(UserViewModel viewModel, AuthGuardService authGuard)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _authGuard = authGuard;
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Check if user is authenticated
        if (!await _authGuard.CheckAuthenticationAsync())
            return;
            
        // If not logged in, reset fields
        if (!_viewModel.IsLoggedIn)
        {
            _viewModel.Username = string.Empty;
            _viewModel.Password = string.Empty;
        }
    }
}
