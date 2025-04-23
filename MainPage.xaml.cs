using RestoGestApp.Services;
using RestoGestApp.Models;
using RestoGestApp.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace RestoGestApp;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly AuthGuardService _authGuard;
    private readonly CartViewModel _cartViewModel;
    private List<string> _categories = new List<string>();
    private List<MenuItemModel> _menuItems = new List<MenuItemModel>();
    private string _selectedCategory = "All";

    public MainPage(DatabaseService databaseService, AuthGuardService authGuard, CartViewModel cartViewModel)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _authGuard = authGuard;
        _cartViewModel = cartViewModel;

        // Load data when the page appears
        this.Appearing += OnPageAppearing;
    }
    
    private async void OnPageAppearing(object sender, EventArgs e)
    {
        // Check if user is authenticated
        if (!await _authGuard.CheckAuthenticationAsync())
            return;
            
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            // Load categories
            _categories = await _databaseService.GetCategoriesAsync();
            
            // Add "All" category at the beginning
            _categories.Insert(0, "All");
            
            // Clear existing categories
            CategoriesLayout.Clear();
            
            // Add category buttons
            foreach (var category in _categories)
            {
                var categoryButton = new Button
                {
                    Text = category,
                    BackgroundColor = category == _selectedCategory ? Color.Parse("#FEBE10") : Colors.Transparent,
                    TextColor = category == _selectedCategory ? Colors.White : Color.Parse("#FEBE10"),
                    BorderColor = Color.Parse("#FEBE10"),
                    BorderWidth = 1,
                    CornerRadius = 20,
                    Padding = new Thickness(15, 8),
                    Margin = new Thickness(0, 5)
                };
                
                categoryButton.Clicked += async (sender, e) => 
                {
                    // Update selected category
                    _selectedCategory = category;
                    
                    // Update button styles
                    foreach (var child in CategoriesLayout.Children)
                    {
                        if (child is Button btn)
                        {
                            btn.BackgroundColor = btn.Text == _selectedCategory ? Color.Parse("#FEBE10") : Colors.Transparent;
                            btn.TextColor = btn.Text == _selectedCategory ? Colors.White : Color.Parse("#FEBE10");
                        }
                    }
                    
                    // Load menu items for selected category
                    await LoadMenuItemsAsync();
                };
                
                CategoriesLayout.Add(categoryButton);
            }
            
            // Load menu items
            await LoadMenuItemsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
    }
    
    private async Task LoadMenuItemsAsync()
    {
        try
        {
            // Get menu items based on selected category
            if (_selectedCategory == "All")
            {
                _menuItems = await _databaseService.GetMenuItemsAsync();
            }
            else
            {
                _menuItems = await _databaseService.GetMenuItemsByCategoryAsync(_selectedCategory);
            }
            
            // Clear existing menu items
            MenuItemsLayout.Clear();
            
            // Add menu items
            foreach (var menuItem in _menuItems)
            {
                var border = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    Stroke = Color.Parse("#f0f0f0"),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 5),
                    Shadow = new Shadow
                    {
                        Brush = Brush.Black,
                        Offset = new Point(2, 2),
                        Opacity = 0.1f,
                        Radius = 4
                    }
                };
                
                var grid = new Grid
                {
                    ColumnDefinitions = 
                    {
                        new ColumnDefinition { Width = new GridLength(70) },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    RowSpacing = 10,
                    ColumnSpacing = 15
                };
                
                // Image
                var image = new Image
                {
                    Source = menuItem.ImagePath,
                    HeightRequest = 60,
                    WidthRequest = 60,
                    Aspect = Aspect.AspectFill
                };
                
                var imageContainer = new Border
                {
                    Content = image,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(30) },
                    // IsClippedToBounds is not available in Border, but we can use StrokeShape to clip
                    Padding = new Thickness(0),
                    HeightRequest = 60,
                    WidthRequest = 60,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                
                // Item details
                var detailsLayout = new VerticalStackLayout
                {
                    Spacing = 5
                };
                
                var nameLabel = new Label
                {
                    Text = menuItem.Name,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16
                };
                
                var descriptionLabel = new Label
                {
                    Text = menuItem.Description,
                    FontSize = 12,
                    TextColor = Colors.Gray,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    MaxLines = 2
                };
                
                detailsLayout.Add(nameLabel);
                detailsLayout.Add(descriptionLabel);
                
                // Price and add button
                var priceLayout = new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 5
                };
                
                var priceLabel = new Label
                {
                    Text = $"€{menuItem.Price:F2}",
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Parse("#FEBE10"),
                    HorizontalOptions = LayoutOptions.End
                };
                
                var addButton = new Button
                {
                    Text = "Add",
                    BackgroundColor = Color.Parse("#FEBE10"),
                    TextColor = Colors.White,
                    CornerRadius = 20,
                    HeightRequest = 35,
                    WidthRequest = 70,
                    FontSize = 12,
                    Padding = new Thickness(5, 0)
                };
                
                // Store the menu item as a binding context for the button
                addButton.BindingContext = menuItem;
                
                addButton.Clicked += async (sender, e) => 
                {
                    if (sender is Button button && button.BindingContext is MenuItemModel item)
                    {
                        await _cartViewModel.AddItemAsync(item);
                        await DisplayAlert("Added to Cart", $"{item.Name} added to cart", "OK");
                    }
                };
                
                priceLayout.Add(priceLabel);
                priceLayout.Add(addButton);
                
                // Add elements to grid
                grid.Add(imageContainer, 0, 0);
                grid.Add(detailsLayout, 1, 0);
                grid.Add(priceLayout, 2, 0);
                
                border.Content = grid;
                MenuItemsLayout.Add(border);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load menu items: {ex.Message}", "OK");
        }
    }
}
