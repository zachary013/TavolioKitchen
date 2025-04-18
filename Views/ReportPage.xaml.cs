using RestoGestApp.ViewModels;

namespace RestoGestApp.Views;

public partial class ReportPage : ContentPage
{
    public ReportPage(ReportViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
