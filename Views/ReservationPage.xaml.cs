using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ReservationPage : ContentPage
{
    private readonly ReservationViewModel _viewModel;
    
    public ReservationPage(ReservationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadReservationsCommand.ExecuteAsync(null);
    }
}
