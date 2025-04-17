namespace RestoGestApp;

public partial class App : Application
{
    public App()
    {
        try
        {
            InitializeComponent();
            
            // Create a very simple main page with minimal dependencies
            MainPage = new ContentPage
            {
                BackgroundColor = Colors.White,
                Content = new Label
                {
                    Text = "TavolioKitchen",
                    FontSize = 32,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Green
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in App constructor: {ex}");
            
            // Create an even simpler fallback page
            MainPage = new ContentPage
            {
                BackgroundColor = Colors.White,
                Content = new Label
                {
                    Text = "Error",
                    FontSize = 32,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Red
                }
            };
        }
    }
}
