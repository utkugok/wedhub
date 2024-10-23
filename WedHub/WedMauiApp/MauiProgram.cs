using Microsoft.Extensions.Logging;
using WedMauiApp.ViewModels.Startup;
using WedMauiApp.Views.Startup;

namespace WedMauiApp
{
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //Views
            builder.Services.AddSingleton<LoginPage>();
            //builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<LoadingPage>();


            //View Models
            builder.Services.AddSingleton<LoginPageViewModel>();
            //builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();


            return builder.Build();
        }
    }
}
