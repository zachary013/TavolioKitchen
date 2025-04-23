using RestoGestApp.Services;
using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ReservationPage : ContentPage
{
    private readonly ReservationViewModel _viewModel;
    private readonly AuthGuardService _authGuard;
    
    public ReservationPage(ReservationViewModel viewModel, AuthGuardService authGuard)
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
            
        // Load reservations
        await _viewModel.LoadReservationsCommand.ExecuteAsync(null);
    }
}
