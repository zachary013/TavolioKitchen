namespace RestoGestApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MainPage constructor: {ex.Message}");
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            // Navigate to the AppShell
            CounterBtn.IsEnabled = false;
            CounterBtn.Text = "Loading...";
            
            // Simple delay to show the loading state
            await Task.Delay(500);
            
            // Set the main page to AppShell
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error navigating to AppShell: {ex.Message}");
            CounterBtn.Text = "Error. Try Again";
            CounterBtn.IsEnabled = true;
        }
    }
}
