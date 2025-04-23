using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class SignupPage : ContentPage
{
    private readonly UserViewModel _viewModel;
    
    public SignupPage(UserViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Initialize a new user object when the page appears
        _viewModel.InitializeNewUser();
    }
}
