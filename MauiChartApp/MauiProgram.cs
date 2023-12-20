using MauiChartApp.ModelView;
using MauiCommon.Service;
using MauiChartApp.View;
using MauiChartApp.ViewModel;
using Microsoft.Extensions.Logging;
using MauiChartApp.Service;

namespace MauiChartApp
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
                    fonts.AddFont("OpenSans-Medium.ttf", "sans-serif-medium");
                });            

#if DEBUG
        builder.Logging.AddDebug();
#endif
            string dbLogPath = Path.Combine(FileSystem.AppDataDirectory, "logs.db3");
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "appdb.db3");
            ILogService log = new LogService(dbLogPath);
            builder.Services.AddSingleton<ILogService>(log);
            builder.Services.AddSingleton<IMonitorService>(new MonitorService(dbPath,log));
            builder.Services.AddSingleton<IGpsService>(new GpsService());
            builder.Services.AddSingleton<IWeatherService>(new WeatherService());   
            
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<LogPage>();

            builder.Services.AddSingleton<MainPageModelView>();
            builder.Services.AddSingleton<LogPageViewModel>();
            
            return builder.Build();
        }
    }
}