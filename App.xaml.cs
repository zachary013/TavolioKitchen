using RestoGestApp.Services;

namespace RestoGestApp;

public partial class App : Application
{
    private readonly DatabaseService _databaseService;
    
    public App(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        
        // Initialize the database
        InitializeDatabaseAsync();
        
        // Set the main page
        MainPage = new AppShell();
    }
    
    private async void InitializeDatabaseAsync()
    {
        try
        {
            await _databaseService.InitializeAsync();
        }
        catch (Exception ex)
        {
            // In a real app, you would log this error
            Console.WriteLine($"Error initializing database: {ex.Message}");
            
            // Show an error message to the user
            MainPage = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = "Error initializing the app", FontSize = 20, HorizontalOptions = LayoutOptions.Center },
                        new Label { Text = ex.Message, FontSize = 16, HorizontalOptions = LayoutOptions.Center }
                    },
                    VerticalOptions = LayoutOptions.Center
                }
            };
        }
    }
}
