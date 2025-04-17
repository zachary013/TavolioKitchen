using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class CartViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _cartItems;
    
    [ObservableProperty]
    private decimal _totalAmount;
    
    private readonly PaymentService _paymentService;
    
    public CartViewModel(DataService dataService, NotificationService notificationService, PaymentService paymentService) 
        : base(dataService, notificationService)
    {
        Title = "Cart";
        _paymentService = paymentService;
        CartItems = new ObservableCollection<OrderItem>();
    }
    
    public Task AddItemAsync(MenuItemModel menuItem)
    {
        // Check if the item is already in the cart
        var existingItem = CartItems.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
        
        if (existingItem != null)
        {
            // Increment quantity
            existingItem.Quantity++;
            UpdateTotalAmount();
        }
        else
        {
            // Add new item
            var orderItem = new OrderItem
            {
                MenuItemId = menuItem.Id,
                MenuItemName = menuItem.Name,
                Quantity = 1,
                UnitPrice = menuItem.Price,
                Notes = string.Empty
            };
            
            CartItems.Add(orderItem);
            UpdateTotalAmount();
        }
        
        return Task.CompletedTask;
    }
    
    [RelayCommand]
    private void RemoveItem(OrderItem item)
    {
        if (item == null)
            return;
            
        CartItems.Remove(item);
        UpdateTotalAmount();
    }
    
    [RelayCommand]
    private void IncreaseQuantity(OrderItem item)
    {
        if (item == null)
            return;
            
        item.Quantity++;
        UpdateTotalAmount();
    }
    
    [RelayCommand]
    private void DecreaseQuantity(OrderItem item)
    {
        if (item == null)
            return;
            
        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            CartItems.Remove(item);
        }
        
        UpdateTotalAmount();
    }
    
    [RelayCommand]
    private void ClearCart()
    {
        CartItems.Clear();
        UpdateTotalAmount();
    }
    
    private void UpdateTotalAmount()
    {
        TotalAmount = CartItems.Sum(i => i.Subtotal);
    }
    
    [RelayCommand]
    private async Task CheckoutAsync()
    {
        if (CartItems.Count == 0)
        {
            await NotificationService.ShowAlertAsync("Empty Cart", "Your cart is empty. Add some items before checking out.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            // Create a new order
            var order = new Order
            {
                UserId = 1, // In a real app, this would be the current user's ID
                OrderDate = DateTime.Now,
                Status = OrderStatus.Created,
                TotalAmount = TotalAmount,
                IsPaid = false,
                Notes = string.Empty
            };
            
            // Save the order to get an ID
            var orderId = await DataService.SaveOrderAsync(order);
            
            // Save order items
            foreach (var item in CartItems)
            {
                item.OrderId = orderId;
                await DataService.SaveOrderItemAsync(item);
            }
            
            // Process payment
            var paymentMethod = await NotificationService.ShowActionSheetAsync(
                "Select Payment Method", 
                "Cancel", 
                destruction: null, 
                "Credit Card", 
                "PayPal", 
                "Apple Pay");
                
            if (paymentMethod != "Cancel")
            {
                var isPaymentSuccessful = await _paymentService.ProcessPaymentAsync(order, paymentMethod);
                
                if (isPaymentSuccessful)
                {
                    // Update order status
                    order.Status = OrderStatus.Processing;
                    await DataService.SaveOrderAsync(order);
                    
                    // Clear the cart
                    CartItems.Clear();
                    UpdateTotalAmount();
                    
                    await NotificationService.ShowAlertAsync("Order Placed", "Your order has been placed successfully!");
                }
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to checkout: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
