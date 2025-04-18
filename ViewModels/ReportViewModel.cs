using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class ReportViewModel : BaseViewModel
{
    [ObservableProperty]
    private Report? _currentReport;
    
    [ObservableProperty]
    private DateTime _startDate = DateTime.Today.AddDays(-30);
    
    [ObservableProperty]
    private DateTime _endDate = DateTime.Today;
    
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, decimal>> _revenueByCategoryItems;
    
    [ObservableProperty]
    private ObservableCollection<KeyValuePair<string, int>> _ordersByStatusItems;
    
    [ObservableProperty]
    private ObservableCollection<MenuItemModel> _topSellingItems;
    
    public ReportViewModel(DataService dataService, NotificationService notificationService) 
        : base(dataService, notificationService)
    {
        Title = "Reports";
        RevenueByCategoryItems = new ObservableCollection<KeyValuePair<string, decimal>>();
        OrdersByStatusItems = new ObservableCollection<KeyValuePair<string, int>>();
        TopSellingItems = new ObservableCollection<MenuItemModel>();
    }
    
    [RelayCommand]
    private async Task GenerateReportAsync()
    {
        if (IsBusy)
            return;
            
        if (EndDate < StartDate)
        {
            await NotificationService.ShowAlertAsync("Error", "End date cannot be before start date.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            CurrentReport = await DataService.GenerateReportAsync(StartDate, EndDate);
            
            // Update collections for UI
            RevenueByCategoryItems.Clear();
            foreach (var item in CurrentReport.RevenueByCategory)
            {
                RevenueByCategoryItems.Add(item);
            }
            
            OrdersByStatusItems.Clear();
            foreach (var item in CurrentReport.OrdersByStatus)
            {
                OrdersByStatusItems.Add(item);
            }
            
            TopSellingItems.Clear();
            foreach (var item in CurrentReport.TopSellingItems)
            {
                TopSellingItems.Add(item);
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to generate report: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
