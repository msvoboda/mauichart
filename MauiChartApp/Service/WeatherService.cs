using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiChartApp.DTO;
using MauiCommon.Service;
using Newtonsoft.Json;

namespace MauiChartApp.Service
{
    public interface IWeatherService
    {
        Task<OpenMeteoDTO> getOpenMeteo(String latitude, String longitude);
    }

    public class WeatherService : IWeatherService
    {
        ILogService log;

        HttpClientService _client = new HttpClientService();
        // API
        String openMeteo= "https://api.open-meteo.com/v1/forecast";
        Uri _uri;

        public WeatherService()
        {            
        }

        public async Task<OpenMeteoDTO> getOpenMeteo(String latitude, String longitude)
        {
            string parameters = $"?latitude={latitude}&longitude={longitude}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";
            _uri = new Uri(openMeteo+parameters);

            return await _client.Get(_uri, log, async response =>
            {
                var content = await response.Content.ReadAsStringAsync();
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                var result = JsonConvert.DeserializeObject<OpenMeteoDTO>(content, microsoftDateFormatSettings);
                return result;
            });
        }
    }
}
