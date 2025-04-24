using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;
using System.Collections.ObjectModel;

namespace RestoGestApp.ViewModels;

public partial class ReportViewModel : BaseViewModel
{
    [ObservableProperty]
    private DateTime _startDate = DateTime.Now.AddDays(-30);
    
    [ObservableProperty]
    private DateTime _endDate = DateTime.Now;
    
    [ObservableProperty]
    private ReportType _reportType = ReportType.Sales;
    
    [ObservableProperty]
    private Report? _currentReport;
    
    [ObservableProperty]
    private ObservableCollection<string> _reportTypes = new ObservableCollection<string>
    {
        "Sales",
        "Inventory",
        "Customer Activity"
    };
    
    [ObservableProperty]
    private string _selectedReportType = "Sales";
    
    [ObservableProperty]
    private ObservableCollection<string> _chartData = new ObservableCollection<string>();
    
    public ReportViewModel(DataService dataService, NotificationService notificationService) 
        : base(dataService, notificationService)
    {
        Title = "Reports";
    }
    
    [RelayCommand]
    private async Task GenerateReportAsync()
    {
        if (IsBusy)
            return;
            
        try
        {
            IsBusy = true;
            
            // Convert selected report type string to enum
            ReportType reportType = (ReportType)Enum.Parse(typeof(ReportType), SelectedReportType.Replace(" ", ""));
            
            // Generate report
            CurrentReport = await DataService.GenerateReportAsync(StartDate, EndDate, reportType);
            
            // Update chart data based on report type
            ChartData.Clear();
            
            if (CurrentReport != null)
            {
                switch (reportType)
                {
                    case ReportType.Sales:
                        // Add top selling items to chart data
                        foreach (var item in CurrentReport.TopSellingItems)
                        {
                            ChartData.Add(item);
                        }
                        break;
                        
                    case ReportType.CustomerActivity:
                        // Add top customers to chart data
                        foreach (var customer in CurrentReport.TopCustomers)
                        {
                            ChartData.Add(customer);
                        }
                        break;
                        
                    case ReportType.Inventory:
                        // For inventory, we would add inventory data
                        ChartData.Add("Inventory data not available");
                        break;
                }
            }
            
            await NotificationService.ShowToastAsync("Report generated successfully!");
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
    
    [RelayCommand]
    private async Task ExportReportAsync()
    {
        if (CurrentReport == null)
        {
            await NotificationService.ShowAlertAsync("Error", "No report to export. Please generate a report first.");
            return;
        }
        
        try
        {
            // In a real app, this would export the report to a file
            await NotificationService.ShowAlertAsync("Export", "Report export functionality will be implemented in a future update.");
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to export report: {ex.Message}");
        }
    }
    
    partial void OnSelectedReportTypeChanged(string value)
    {
        // Reset current report when report type changes
        CurrentReport = null;
        ChartData.Clear();
    }
}
