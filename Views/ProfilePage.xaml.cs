using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(UserViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
