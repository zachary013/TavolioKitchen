using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly UserViewModel _viewModel;
    
    public LoginPage(UserViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Reset fields when page appears
        _viewModel.Email = string.Empty;
        _viewModel.Password = string.Empty;
    }
    
    // Prevent back navigation if this is the initial page
    protected override bool OnBackButtonPressed()
    {
        // If we're on the login page and not logged in, don't allow back navigation
        if (!_viewModel.IsLoggedIn)
        {
            return true; // Cancel the back button
        }
        
        return base.OnBackButtonPressed();
    }
}
