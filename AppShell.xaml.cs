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
            try
            {
                if (!_isLoggedIn)
                {
                    // If not logged in, navigate to login page
                    await GoToAsync("//login");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
        });
    }
    
    // Method to handle login state changes
    public async Task HandleAuthenticationChanged(bool isLoggedIn)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication change error: {ex.Message}");
        }
    }
}
