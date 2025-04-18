using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class MenuPage : ContentPage
{
    private readonly MenuViewModel _viewModel;
    
    public MenuPage(MenuViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCategoriesCommand.ExecuteAsync(null);
    }
}
