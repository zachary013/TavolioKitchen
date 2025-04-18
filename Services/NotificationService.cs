namespace RestoGestApp.Services;

public class NotificationService
{
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        try
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page != null)
            {
                await window.Page.DisplayAlert(title, message, cancel);
            }
            else
            {
                Console.WriteLine($"Alert: {title} - {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing alert: {ex.Message}");
        }
    }
    
    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        try
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page != null)
            {
                return await window.Page.DisplayAlert(title, message, accept, cancel);
            }
            Console.WriteLine($"Confirmation: {title} - {message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing confirmation: {ex.Message}");
            return false;
        }
    }
    
    public async Task<string> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
    {
        try
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page != null)
            {
                return await window.Page.DisplayActionSheet(title, cancel, destruction, buttons);
            }
            Console.WriteLine($"ActionSheet: {title}");
            return cancel ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing action sheet: {ex.Message}");
            return cancel ?? string.Empty;
        }
    }
    
    public async Task ShowToastAsync(string message)
    {
        try
        {
            // Simple toast implementation using alert
            await ShowAlertAsync("Notification", message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing toast: {ex.Message}");
        }
    }
}
