using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ProfilePage : ContentPage
{
    private readonly UserViewModel _viewModel;
    
    public ProfilePage(UserViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Reset fields when page appears if not logged in
        if (!_viewModel.IsLoggedIn)
        {
            _viewModel.Email = string.Empty;
            _viewModel.Password = string.Empty;
        }
    }
    
    // Prevent back navigation if this is the initial page
    protected override bool OnBackButtonPressed()
    {
        // If we're on the profile page and not logged in, don't allow back navigation
        if (!_viewModel.IsLoggedIn)
        {
            return true; // Cancel the back button
        }
        
        return base.OnBackButtonPressed();
    }
}
