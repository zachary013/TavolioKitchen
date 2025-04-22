using RestoGestApp.Services;
using RestoGestApp.Models;
using Microsoft.Maui.Controls.Shapes;

namespace RestoGestApp;

public partial class MainPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private List<string> _categories = new List<string>();
    private List<MenuItemModel> _menuItems = new List<MenuItemModel>();
    private string _selectedCategory = "All";
    
    public MainPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        
        // Load data when the page appears
        this.Appearing += async (sender, e) => await LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        try
        {
            // Show loading indicator
            IsBusy = true;
            
            // Load categories
            _categories = await _databaseService.GetCategoriesAsync();
            
            // Load all menu items
            _menuItems = await _databaseService.GetMenuItemsAsync();
            
            // Display categories
            DisplayCategories();
            
            // Display menu items
            DisplayMenuItems();
            
            // Output to console for testing
            Console.WriteLine($"Loaded {_categories.Count} categories and {_menuItems.Count} menu items");
            
            foreach (var category in _categories)
            {
                Console.WriteLine($"Category: {category}");
            }
            
            foreach (var item in _menuItems)
            {
                Console.WriteLine($"Menu Item: {item.Name}, Price: {item.Price:C}, Category: {item.Category}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            await DisplayAlert("Error", "Failed to load menu data", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void DisplayCategories()
    {
        // Clear existing categories
        CategoriesLayout.Children.Clear();
        
        // Add "All" category
        var allCategoryBorder = new Border
        {
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(20) },
            Padding = new Thickness(15, 8),
            BackgroundColor = _selectedCategory == "All" ? Color.Parse("#FEBE10") : Colors.Transparent,
            Stroke = new SolidColorBrush(Color.Parse("#FEBE10"))
        };
        
        var allCategoryLabel = new Label
        {
            Text = "All",
            TextColor = _selectedCategory == "All" ? Colors.White : Color.Parse("#FEBE10"),
            FontAttributes = FontAttributes.Bold
        };
        
        allCategoryBorder.Content = allCategoryLabel;
        
        var allCategoryTapGesture = new TapGestureRecognizer();
        allCategoryTapGesture.Tapped += (s, e) => 
        {
            _selectedCategory = "All";
            DisplayCategories();
            DisplayMenuItems();
        };
        allCategoryBorder.GestureRecognizers.Add(allCategoryTapGesture);
        
        CategoriesLayout.Children.Add(allCategoryBorder);
        
        // Add other categories
        foreach (var category in _categories)
        {
            var categoryBorder = new Border
            {
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(20) },
                Padding = new Thickness(15, 8),
                BackgroundColor = _selectedCategory == category ? Color.Parse("#FEBE10") : Colors.Transparent,
                Stroke = new SolidColorBrush(Color.Parse("#FEBE10"))
            };
            
            var categoryLabel = new Label
            {
                Text = category,
                TextColor = _selectedCategory == category ? Colors.White : Color.Parse("#FEBE10"),
                FontAttributes = FontAttributes.Bold
            };
            
            categoryBorder.Content = categoryLabel;
            
            var categoryTapGesture = new TapGestureRecognizer();
            var currentCategory = category; // Capture the current category
            categoryTapGesture.Tapped += (s, e) => 
            {
                _selectedCategory = currentCategory;
                DisplayCategories();
                DisplayMenuItems();
            };
            categoryBorder.GestureRecognizers.Add(categoryTapGesture);
            
            CategoriesLayout.Children.Add(categoryBorder);
        }
    }
    
    private void DisplayMenuItems()
    {
        // Clear existing menu items
        MenuItemsLayout.Children.Clear();
        
        // Filter menu items by selected category
        var filteredItems = _selectedCategory == "All" 
            ? _menuItems 
            : _menuItems.Where(item => item.Category == _selectedCategory).ToList();
        
        // Display menu items
        foreach (var item in filteredItems)
        {
            var itemBorder = new Border
            {
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(10) },
                Padding = new Thickness(15),
                BackgroundColor = Colors.White,
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
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions = 
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnSpacing = 15
            };
            
            var image = new Image
            {
                Source = item.ImagePath,
                WidthRequest = 80,
                HeightRequest = 80
            };
            Grid.SetRowSpan(image, 3);
            Grid.SetColumn(image, 0);
            
            var nameLabel = new Label
            {
                Text = item.Name,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };
            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 1);
            
            var descriptionLabel = new Label
            {
                Text = item.Description,
                TextColor = Colors.Gray,
                FontSize = 14
            };
            Grid.SetRow(descriptionLabel, 1);
            Grid.SetColumn(descriptionLabel, 1);
            
            var priceAndButtonStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };
            
            var priceLabel = new Label
            {
                Text = $"€{item.Price:F2}",
                TextColor = Color.Parse("#FEBE10"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center
            };
            
            var addToCartButton = new Button
            {
                Text = "Add to Cart",
                BackgroundColor = Color.Parse("#FEBE10"),
                TextColor = Colors.White,
                CornerRadius = 20,
                HeightRequest = 35,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.End
            };
            
            addToCartButton.Clicked += (s, e) => 
            {
                DisplayAlert("Add to Cart", $"{item.Name} added to cart", "OK");
            };
            
            priceAndButtonStack.Children.Add(priceLabel);
            priceAndButtonStack.Children.Add(addToCartButton);
            
            Grid.SetRow(priceAndButtonStack, 2);
            Grid.SetColumn(priceAndButtonStack, 1);
            
            grid.Children.Add(image);
            grid.Children.Add(nameLabel);
            grid.Children.Add(descriptionLabel);
            grid.Children.Add(priceAndButtonStack);
            
            itemBorder.Content = grid;
            
            MenuItemsLayout.Children.Add(itemBorder);
        }
    }
}
