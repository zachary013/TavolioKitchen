# RestoGestApp

RestoGestApp is a cross-platform restaurant management application built with .NET MAUI (Multi-platform App UI). This application is designed to help restaurant owners and staff manage their operations efficiently across multiple platforms including iOS, macOS (via Catalyst), and Windows.

## Features

- Cross-platform support (iOS, macOS, Windows)
- Modern UI with XAML-based interface
- Responsive design that works across different device form factors
- Dark/Light theme support

## Technologies Used

- **.NET MAUI**: Cross-platform framework for building native mobile and desktop apps
- **C#**: Primary programming language
- **.NET 9**: Latest .NET framework
- **XAML**: For designing the user interface

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/) with MAUI workload
- For iOS/macOS development: Mac with Xcode 15+
- For Windows development: Windows 10/11 with Windows App SDK

### Installation

1. Clone the repository
   ```
   git clone https://github.com/yourusername/RestoGestApp.git
   cd RestoGestApp
   ```

2. Restore dependencies
   ```
   dotnet restore
   ```

3. Build the application
   ```
   dotnet build
   ```

4. Run the application for your target platform
   ```
   dotnet run -f net9.0-ios            # For iOS
   dotnet run -f net9.0-maccatalyst    # For macOS
   dotnet run -f net9.0-windows        # For Windows
   ```

## Usage

RestoGestApp provides an intuitive interface for restaurant management. The application is currently in development, with more features to be added soon.

## Project Structure

```
RestoGestApp/
├── App.xaml                  # Application definition
├── AppShell.xaml             # Shell navigation container
├── MainPage.xaml             # Main application page
├── MauiProgram.cs            # MAUI application entry point
├── Platforms/                # Platform-specific code
│   ├── iOS/                  # iOS-specific code
│   ├── MacCatalyst/          # macOS-specific code
│   └── Windows/              # Windows-specific code
├── Resources/                # Application resources
│   ├── AppIcon/              # Application icons
│   ├── Fonts/                # Custom fonts
│   ├── Images/               # Image assets
│   ├── Raw/                  # Raw assets
│   ├── Splash/               # Splash screen assets
│   └── Styles/               # XAML styles
└── RestoGestApp.csproj       # Project file
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request
