namespace RestoGestApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        try
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute(nameof(Views.MenuPage), typeof(Views.MenuPage));
            Routing.RegisterRoute(nameof(Views.CartPage), typeof(Views.CartPage));
            Routing.RegisterRoute(nameof(Views.ReservationPage), typeof(Views.ReservationPage));
            Routing.RegisterRoute(nameof(Views.ProfilePage), typeof(Views.ProfilePage));
            Routing.RegisterRoute(nameof(Views.ReportPage), typeof(Views.ReportPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AppShell constructor: {ex.Message}");
        }
    }
}
