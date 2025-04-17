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
        // This is a simulated payment process
        await Task.Delay(2000); // Simulate processing time
        
        // Simulate a successful payment (in a real app, this would integrate with a payment gateway)
        var isSuccessful = true;
        
        if (isSuccessful)
        {
            await _notificationService.ShowAlertAsync("Payment Successful", 
                $"Your payment of ${order.TotalAmount} has been processed successfully.");
            
            // Update the order status
            order.IsPaid = true;
            order.Status = OrderStatus.Completed;
            
            return true;
        }
        else
        {
            await _notificationService.ShowAlertAsync("Payment Failed", 
                "There was an error processing your payment. Please try again.");
            
            return false;
        }
    }
    
    public async Task<bool> RefundPaymentAsync(Order order)
    {
        // This is a simulated refund process
        await Task.Delay(2000); // Simulate processing time
        
        // Simulate a successful refund (in a real app, this would integrate with a payment gateway)
        var isSuccessful = true;
        
        if (isSuccessful)
        {
            await _notificationService.ShowAlertAsync("Refund Successful", 
                $"Your refund of ${order.TotalAmount} has been processed successfully.");
            
            // Update the order status
            order.IsPaid = false;
            order.Status = OrderStatus.Cancelled;
            
            return true;
        }
        else
        {
            await _notificationService.ShowAlertAsync("Refund Failed", 
                "There was an error processing your refund. Please try again.");
            
            return false;
        }
    }
}
