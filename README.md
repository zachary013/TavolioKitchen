# 🍽️ Tavolio Kitchen

![Restaurant Management](https://img.shields.io/badge/Restaurant-Management-orange)
![.NET MAUI](https://img.shields.io/badge/.NET-MAUI-512BD4)
![Version](https://img.shields.io/badge/Version-1.0-blue)

Tavolio Kitchen is a cross-platform restaurant management application built with .NET MAUI (Multi-platform App UI). This application is designed to help restaurant owners and staff manage their operations efficiently across multiple platforms including iOS and macOS (via Catalyst).

## Screenshots
<img width="363" alt="Screenshot 2025-04-24 at 00 36 14" src="https://github.com/user-attachments/assets/617ddf9d-2e13-48b3-ac3c-b2885596874b" />

<img width="383" alt="Screenshot 2025-04-24 at 00 34 12" src="https://github.com/user-attachments/assets/3af1ba8c-c5de-40da-bbf4-219f6547e52e" />

<img width="372" alt="Screenshot 2025-04-24 at 00 34 57" src="https://github.com/user-attachments/assets/716bbe08-636d-4870-a041-3885c444a9f8" />

<img width="370" alt="Screenshot 2025-04-24 at 00 35 17" src="https://github.com/user-attachments/assets/1d4d3e49-8351-4817-9213-d526c79c366f" />

<img width="368" alt="Screenshot 2025-04-24 at 00 35 49" src="https://github.com/user-attachments/assets/61c62f63-1a9d-485a-8972-6352a8afdb29" />

## ✨ Features

- 📋 **Menu Management** - Categories, dishes, prices, and images
- 🧾 **Order System** - Cart and checkout functionality 
- 🪑 **Table Reservations** - Manage seating and availability
- 👤 **User Profiles** - Different roles for clients and personnel
- 🔔 **Notifications** - Alert system for orders and reservations
- 💳 **Simulated Payments** - Process transactions 
- 📊 **Statistics/Reports** - Track revenue, orders, and reservations


## 🛠️ Technologies Used

- <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" width="16" height="16"/> **`.NET MAUI`**: Cross-platform framework for building native mobile and desktop apps
- <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" width="16" height="16"/> **`C#`**: Primary programming language
- <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dotnetcore/dotnetcore-original.svg" width="16" height="16"/> **`.NET 9`**: Latest .NET framework
- <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/xamarin/xamarin-original.svg" width="16" height="16"/> **`XAML`**: For designing the user interface
- <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/sqlite/sqlite-original.svg" width="16" height="16"/> **`SQLite`**: For local database storage
- 📐 **`MVVM Pattern`**: Using CommunityToolkit.Mvvm


## 📁 Project Structure

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


## ▶️ Run Instructions

### Prerequisites

- 📦 [.NET 9 SDK](https://dotnet.microsoft.com/download)
- 💻 [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/) with MAUI workload
- 🍎 For iOS/macOS development: Mac with Xcode 15+

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
---

## Prepared by :

| Avatar                                                                                                  | Name | GitHub |
|---------------------------------------------------------------------------------------------------------|------|--------|
| <img src="https://github.com/zachary013.png" width="50" height="50" style="border-radius: 50%"/>        | Zakariae Azarkan | [@zachary013](https://github.com/zachary013) |


## 🤝 Contributing
This is my uni work, so I’m not looking for pull requests—but feel free to fork it and tweak it for your own deep learning adventures! Got feedback? Hit me up via GitHub Issues.
