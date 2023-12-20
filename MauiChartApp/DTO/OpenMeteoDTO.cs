using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.DTO
{
    public class OpenMeteoDTO
    {
        public string latitude { get; set; }

        public string longitude { get; set; }

        public float elevation { get; set; }   
        
        public string timezone { get; set; }

        public CurrentTempDTO current { get; set; }

        public HourlySeriesDTO hourly { get; set; }
    }

    public class CurrentTempDTO
    {
        public string time { get; set; } = string.Empty;
        public double temperature_2m { get; set; } 
        public double wind_speed_10m { get; set; }

    }

    public class HourlySeriesDTO
    {
        public List<DateTime> time { get; set; }

        public List<double> temperature_2m { get; set; }

        public List<int> relative_humidity_2m { get; set; }

        public List<double> wind_speed_10m { get; set; }

    }
}
