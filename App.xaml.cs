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
                            TextColor = Colors.Green,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Label
                        {
                            Text = "Modern Restaurant Management",
                            FontSize = 18,
                            TextColor = Colors.Gray,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0, 10, 0, 20)
                        }
                    }
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
