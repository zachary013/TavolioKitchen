# RestoGestApp

RestoGestApp is a cross-platform restaurant management application built with .NET MAUI (Multi-platform App UI). This application is designed to help restaurant owners and staff manage their operations efficiently across multiple platforms including iOS and macOS (via Catalyst).

## Features

- Menu management with categories, dishes, prices, and images
- Order system with cart and checkout
- Table reservations
- User profiles (clients/personnel)
- Notifications via alerts
- Simulated payments
- Statistics/reports (total revenue, orders, reservations)

## Technologies Used

- **.NET MAUI**: Cross-platform framework for building native mobile and desktop apps
- **C#**: Primary programming language
- **.NET 9**: Latest .NET framework
- **XAML**: For designing the user interface
- **SQLite**: For local database storage
- **MVVM Pattern**: Using CommunityToolkit.Mvvm

## Project Structure

```
RestoGestApp/
├── Models/                # Data models
│   ├── MenuItem.cs        # Menu item model
│   ├── Order.cs           # Order model
│   ├── OrderItem.cs       # Order item model
│   ├── Reservation.cs     # Reservation model
│   ├── User.cs            # User model
│   └── Report.cs          # Report model
├── Services/              # Business logic services
│   ├── DataService.cs     # SQLite database service
│   ├── NotificationService.cs # Notification service
│   └── PaymentService.cs  # Payment processing service
├── ViewModels/            # MVVM ViewModels
│   ├── BaseViewModel.cs   # Base ViewModel
│   ├── MenuViewModel.cs   # Menu ViewModel
│   ├── CartViewModel.cs   # Cart ViewModel
│   ├── ReservationViewModel.cs # Reservation ViewModel
│   ├── UserViewModel.cs   # User ViewModel
│   └── ReportViewModel.cs # Report ViewModel
├── Views/                 # XAML UI pages
│   ├── MenuPage.xaml      # Menu page
│   ├── CartPage.xaml      # Cart page
│   ├── ReservationPage.xaml # Reservation page
│   ├── ProfilePage.xaml   # Profile page
│   └── ReportPage.xaml    # Report page
├── Converters/            # Value converters
│   └── BooleanConverters.cs # Boolean converters
├── Resources/             # App resources
│   └── Images/            # Image assets
├── App.xaml               # Application definition
├── AppShell.xaml          # Shell navigation container
└── MauiProgram.cs         # MAUI application entry point
```

## Run Instructions

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/) with MAUI workload
- For iOS/macOS development: Mac with Xcode 15+

### Running on iOS

1. Open the solution in Rider or Visual Studio
2. Select the iOS target
3. Build and run the application
4. Alternatively, use the command line:
   ```
   dotnet build -t:Run -f net9.0-ios
   ```

### Running on MacCatalyst

1. Open the solution in Rider or Visual Studio
2. Select the MacCatalyst target
3. Build and run the application
4. Alternatively, use the command line:
   ```
   dotnet build -t:Run -f net9.0-maccatalyst
   ```

## Default Login Credentials

The application is seeded with the following user accounts:

- **Admin**: Username: `admin`, Password: `admin123`
- **Manager**: Username: `manager`, Password: `manager123`
- **Staff**: Username: `staff1`, Password: `staff123`
- **Client**: Username: `client1`, Password: `client123`
