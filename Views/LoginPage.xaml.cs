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
        _viewModel.Username = string.Empty;
        _viewModel.Password = string.Empty;
    }
}
