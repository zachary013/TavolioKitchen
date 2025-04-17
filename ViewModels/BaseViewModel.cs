using CommunityToolkit.Mvvm.ComponentModel;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;
    
    [ObservableProperty]
    private string _title = string.Empty;
    
    protected readonly DataService DataService;
    protected readonly NotificationService NotificationService;
    
    public BaseViewModel(DataService dataService, NotificationService notificationService)
    {
        DataService = dataService;
        NotificationService = notificationService;
    }
}
