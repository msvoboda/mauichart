using MauiChartApp.ModelView;
using MauiChartApp.Service;
using MauiChartApp.View;
using MauiChartApp.ViewModel;
using Microsoft.Extensions.Logging;

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
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "chartapp.db3");
            builder.Services.AddSingleton<ILogService>(new LogService(dbPath));         

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<LogPage>();

            builder.Services.AddSingleton<MainPageModelView>();
            builder.Services.AddSingleton<LogPageViewModel>();

            return builder.Build();
        }
    }
}