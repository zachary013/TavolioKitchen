using RestoGestApp.Services;
using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class CartPage : ContentPage
{
    private readonly CartViewModel _viewModel;
    private readonly AuthGuardService _authGuard;
    
    public CartPage(CartViewModel viewModel, AuthGuardService authGuard)
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
            
        // Load cart items
        await _viewModel.LoadCartItemsCommand.ExecuteAsync(null);
    }
}
