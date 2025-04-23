namespace RestoGestApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("signup", typeof(Views.SignupPage));
        Routing.RegisterRoute("login", typeof(Views.LoginPage));
        Routing.RegisterRoute("menu", typeof(Views.MenuPage));
        Routing.RegisterRoute("cart", typeof(Views.CartPage));
        Routing.RegisterRoute("profile", typeof(Views.ProfilePage));
        Routing.RegisterRoute("reservation", typeof(Views.ReservationPage));
        Routing.RegisterRoute("report", typeof(Views.ReportPage));
    }
}
