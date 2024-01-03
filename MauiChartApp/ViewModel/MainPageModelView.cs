using Common.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiChartApp.ChartData;
using MauiChartApp.DTO;
using MauiChartApp.Service;
using MauiCommon.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.ModelView
{
    public partial class MainPageModelView : BaseViewModel
    {
        IGpsService service;
        ILogService logService;
        IWeatherService weatherService;

        public MainPageModelView(IGpsService service, ILogService log, IWeatherService weather)
        {
            this.service = service;
            this.logService = log;
            this.weatherService = weather;
            this.Title = "Weather";
            _ = GetLocationAsync();
        }

        [RelayCommand]
        public async Task GetLocationAsync()
        {
            await service.GetCurrentLocation().ContinueWith(locResult =>
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";

                Latitude =  Math.Round((double)locResult?.Result.Latitude, 2).ToString(nfi);
                Longitude = Math.Round((double)locResult?.Result.Longitude, 2).ToString(nfi);
                logService.Info($"Latitude: {Latitude}; Longitude: {Longitude}");
                _ = getMeteoDataAsync(Latitude, Longitude);
            });
        }

        [ObservableProperty]
        public string latitude;
        [ObservableProperty]
        public string longitude;
        [ObservableProperty]
        public OpenMeteoDTO openMeteo;
        [ObservableProperty]
        public TimeSeries timeSeries;
        [ObservableProperty]
        public string temperature;


        public async Task getMeteoDataAsync(string lat, string lon)
        {
            await weatherService.getOpenMeteo(lat, lon).ContinueWith(meteo =>
            {
                if (meteo != null) {
                    OpenMeteo = meteo?.Result;
                    TimeSeries = ChartBuilder.buildFromOpenMeteo(OpenMeteo);
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";
                    Temperature = $"{openMeteo?.current.temperature_2m.ToString(nfi)} Cº";
                    logService.Info(meteo?.Result?.ToString());   
                }
            });
        }
    }
}
