using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Converters
{
    public class LogItemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Black;

            if (value.Equals("Error"))
            {
                return Colors.Red;
            }
            else if (value.Equals("Warning"))
            {
                return Colors.Gold;
            }
            else if (value.Equals("Debug"))
            {
                return Colors.Blue;
            }
            else
            {
                return Colors.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Colors.Black;  
        }
    }
}
