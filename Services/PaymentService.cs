using RestoGestApp.Models;

namespace RestoGestApp.Services;

public class PaymentService
{
    private readonly NotificationService _notificationService;
    
    public PaymentService(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public async Task<bool> ProcessPaymentAsync(Order order, string paymentMethod)
    {
        // This is a mock implementation for demonstration purposes
        // In a real app, you would integrate with a payment gateway
        
        try
        {
            // Simulate payment processing delay
            await Task.Delay(1000);
            
            // Simulate payment success (90% of the time)
            var isSuccessful = new Random().NextDouble() < 0.9;
            
            if (isSuccessful)
            {
                order.IsPaid = true;
                await _notificationService.ShowToastAsync($"Payment of {order.TotalAmount:C} processed successfully via {paymentMethod}");
                return true;
            }
            else
            {
                await _notificationService.ShowAlertAsync("Payment Failed", "Your payment could not be processed. Please try again or use a different payment method.");
                return false;
            }
        }
        catch (Exception ex)
        {
            await _notificationService.ShowAlertAsync("Payment Error", $"An error occurred while processing your payment: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> RefundPaymentAsync(Order order)
    {
        // This is a mock implementation for demonstration purposes
        
        try
        {
            // Simulate refund processing delay
            await Task.Delay(1000);
            
            // Simulate refund success (95% of the time)
            var isSuccessful = new Random().NextDouble() < 0.95;
            
            if (isSuccessful)
            {
                order.IsPaid = false;
                await _notificationService.ShowToastAsync($"Refund of {order.TotalAmount:C} processed successfully");
                return true;
            }
            else
            {
                await _notificationService.ShowAlertAsync("Refund Failed", "Your refund could not be processed. Please contact customer support.");
                return false;
            }
        }
        catch (Exception ex)
        {
            await _notificationService.ShowAlertAsync("Refund Error", $"An error occurred while processing your refund: {ex.Message}");
            return false;
        }
    }
}
