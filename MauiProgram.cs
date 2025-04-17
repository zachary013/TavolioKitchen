using Microsoft.Extensions.Logging;
using RestoGestApp.Services;
using RestoGestApp.ViewModels;
using RestoGestApp.Views;

namespace RestoGestApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<DataService>();
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddSingleton<PaymentService>();
        
        // Register view models
        builder.Services.AddSingleton<CartViewModel>();
        builder.Services.AddSingleton<MenuViewModel>();
        builder.Services.AddSingleton<ReservationViewModel>();
        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddSingleton<ReportViewModel>();
        
        // Register views
        builder.Services.AddSingleton<MenuPage>();
        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<ReservationPage>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<ReportPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
