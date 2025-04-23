using RestoGestApp.Views;

namespace RestoGestApp;

public partial class AppShell : Shell
{
    private bool _isLoggedIn;
    
    public AppShell(bool isLoggedIn)
    {
        InitializeComponent();
        
        _isLoggedIn = isLoggedIn;
        
        // Register routes for navigation
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("signup", typeof(SignupPage));
        
        // Navigate to the appropriate starting page
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!_isLoggedIn)
            {
                // If not logged in, navigate to login page
                await GoToAsync("//login");
            }
        });
    }
    
    // Method to handle login state changes
    public async Task HandleAuthenticationChanged(bool isLoggedIn)
    {
        _isLoggedIn = isLoggedIn;
        
        if (isLoggedIn)
        {
            // Navigate to main app when logged in
            await GoToAsync("//main");
        }
        else
        {
            // Navigate to login when logged out
            await GoToAsync("//login");
        }
    }
}
