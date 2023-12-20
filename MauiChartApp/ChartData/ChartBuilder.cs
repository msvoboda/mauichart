using MauiChartApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.ChartData
{
    public class ChartBuilder
    {
        public ChartBuilder() 
        { 
        }

        public static TimeSeries buildFromOpenMeteo(OpenMeteoDTO openMeteo) 
        {
                TimeSeries result = new TimeSeries();
            int cnt = openMeteo?.hourly != null ? openMeteo.hourly.time.Count : 0;
                for (int i = 0; i < cnt; i++)
                {
                    DateTime dt = openMeteo.hourly.time[i];
                    double temperat = openMeteo.hourly.temperature_2m[i];
                    TimeValue value = new TimeValue(dt, (float)temperat);
                    result.Add(value);
                }

                return result;
        }
    }
}
