using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class MenuViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<string> _categories;
    
    [ObservableProperty]
    private ObservableCollection<MenuItemModel> _menuItems;
    
    [ObservableProperty]
    private string _selectedCategory = string.Empty;
    
    private readonly CartViewModel _cartViewModel;
    
    public MenuViewModel(DataService dataService, NotificationService notificationService, CartViewModel cartViewModel) 
        : base(dataService, notificationService)
    {
        Title = "Menu";
        _cartViewModel = cartViewModel;
        Categories = new ObservableCollection<string>();
        MenuItems = new ObservableCollection<MenuItemModel>();
    }
    
    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        if (IsBusy)
            return;
            
        try
        {
            IsBusy = true;
            
            var categories = await DataService.GetCategoriesAsync();
            
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
            
            if (Categories.Count > 0 && string.IsNullOrEmpty(SelectedCategory))
            {
                SelectedCategory = Categories[0];
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to load categories: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task LoadMenuItemsAsync()
    {
        if (IsBusy || string.IsNullOrEmpty(SelectedCategory))
            return;
            
        try
        {
            IsBusy = true;
            
            var items = await DataService.GetMenuItemsByCategoryAsync(SelectedCategory);
            
            MenuItems.Clear();
            foreach (var item in items)
            {
                MenuItems.Add(item);
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to load menu items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    partial void OnSelectedCategoryChanged(string value)
    {
        LoadMenuItemsCommand.Execute(null);
    }
    
    [RelayCommand]
    private async Task AddToCartAsync(MenuItemModel menuItem)
    {
        if (menuItem == null)
            return;
            
        try
        {
            await _cartViewModel.AddItemAsync(menuItem);
            await NotificationService.ShowToastAsync($"{menuItem.Name} added to cart");
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to add item to cart: {ex.Message}");
        }
    }
}
