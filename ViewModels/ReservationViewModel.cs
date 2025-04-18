using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestoGestApp.Models;
using RestoGestApp.Services;

namespace RestoGestApp.ViewModels;

public partial class ReservationViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Reservation> _reservations;
    
    [ObservableProperty]
    private Reservation _newReservation;
    
    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;
    
    [ObservableProperty]
    private TimeSpan _selectedTime = new TimeSpan(19, 0, 0); // Default to 7:00 PM
    
    [ObservableProperty]
    private string _customerName = string.Empty;
    
    [ObservableProperty]
    private int _tableNumber = 1;
    
    [ObservableProperty]
    private int _numberOfGuests = 2;
    
    [ObservableProperty]
    private string _specialRequests = string.Empty;
    
    [ObservableProperty]
    private string _contactPhone = string.Empty;
    
    public ReservationViewModel(DataService dataService, NotificationService notificationService) 
        : base(dataService, notificationService)
    {
        Title = "Reservations";
        Reservations = new ObservableCollection<Reservation>();
        NewReservation = new Reservation
        {
            UserId = 1, // In a real app, this would be the current user's ID
            ReservationDate = DateTime.Today,
            ReservationTime = new TimeSpan(19, 0, 0), // Default to 7:00 PM
            TableNumber = 1,
            NumberOfGuests = 2,
            Status = ReservationStatus.Pending,
            CustomerName = string.Empty,
            SpecialRequests = string.Empty,
            ContactPhone = string.Empty
        };
    }
    
    [RelayCommand]
    private async Task LoadReservationsAsync()
    {
        if (IsBusy)
            return;
            
        try
        {
            IsBusy = true;
            
            var reservations = await DataService.GetReservationsAsync();
            
            Reservations.Clear();
            foreach (var reservation in reservations)
            {
                Reservations.Add(reservation);
            }
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to load reservations: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task CreateReservationAsync()
    {
        if (IsBusy)
            return;
            
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            await NotificationService.ShowAlertAsync("Error", "Please enter your name.");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(ContactPhone))
        {
            await NotificationService.ShowAlertAsync("Error", "Please enter your phone number.");
            return;
        }
        
        try
        {
            IsBusy = true;
            
            var reservation = new Reservation
            {
                UserId = 1, // In a real app, this would be the current user's ID
                CustomerName = CustomerName,
                ReservationDate = SelectedDate,
                ReservationTime = SelectedTime,
                TableNumber = TableNumber,
                NumberOfGuests = NumberOfGuests,
                SpecialRequests = SpecialRequests,
                ContactPhone = ContactPhone,
                Status = ReservationStatus.Pending
            };
            
            await DataService.SaveReservationAsync(reservation);
            
            // Reset form
            CustomerName = string.Empty;
            SelectedDate = DateTime.Today;
            SelectedTime = new TimeSpan(19, 0, 0);
            TableNumber = 1;
            NumberOfGuests = 2;
            SpecialRequests = string.Empty;
            ContactPhone = string.Empty;
            
            await NotificationService.ShowAlertAsync("Success", "Your reservation has been created successfully!");
            
            // Reload reservations
            await LoadReservationsAsync();
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to create reservation: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task CancelReservationAsync(Reservation reservation)
    {
        if (IsBusy || reservation == null)
            return;
            
        var confirm = await NotificationService.ShowConfirmationAsync(
            "Cancel Reservation", 
            "Are you sure you want to cancel this reservation?");
            
        if (!confirm)
            return;
            
        try
        {
            IsBusy = true;
            
            reservation.Status = ReservationStatus.Cancelled;
            await DataService.SaveReservationAsync(reservation);
            
            await NotificationService.ShowAlertAsync("Success", "Your reservation has been cancelled.");
            
            // Reload reservations
            await LoadReservationsAsync();
        }
        catch (Exception ex)
        {
            await NotificationService.ShowAlertAsync("Error", $"Failed to cancel reservation: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
