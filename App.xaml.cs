using RestoGestApp.Services;
using RestoGestApp.Views;

namespace RestoGestApp;

public partial class App : Application
{
    private readonly DatabaseService _databaseService;
    
    public App(DatabaseService databaseService)
    {
        try
        {
            InitializeComponent();
            
            _databaseService = databaseService;
            
            // Initialize the database
            Task.Run(async () => await InitializeDatabaseAsync());
            
            // Check if user is logged in
            bool isLoggedIn = Preferences.Get("CurrentUserId", 0) > 0;
            
            // Use AppShell for navigation
            MainPage = new AppShell(isLoggedIn);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in App constructor: {ex}");
            
            // Create a fallback page if AppShell fails
            MainPage = new ContentPage
            {
                BackgroundColor = Colors.White,
                Content = new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text = "TavolioKitchen",
                            FontSize = 32,
                            TextColor = Color.Parse("#FEBE10"),
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Label
                        {
                            Text = "Modern Restaurant Management",
                            FontSize = 18,
                            TextColor = Colors.Gray,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0, 10, 0, 20)
                        },
                        new Label
                        {
                            Text = $"Error: {ex.Message}",
                            TextColor = Colors.Red,
                            HorizontalOptions = LayoutOptions.Center
                        }
                    }
                }
            };
        }
    }
    
    private async Task InitializeDatabaseAsync()
    {
        try
        {
            await _databaseService.InitializeAsync();
            Console.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
        }
    }
}
