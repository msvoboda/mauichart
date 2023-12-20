using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.ChartData
{
    public class TimeSeries : List<TimeValue>
    {
        private float _min = Int32.MaxValue;
        private float _max = 0;

        public new void Add(TimeValue time)
        {
            base.Add(time);
            if (time.Value < _min)
            {
                _min = time.Value;
            }

            if (time.Value > _max)
            {
                _max = time.Value;
            }

        }

        public float Max
        {
            get
            {
                return _max;
            }
        }

        public float Min
        {
            get
            {
                return _min;
            }
        }
    }

    public class TimeValue
    {
        public TimeValue()
        {

        }
        public TimeValue(DateTime dateTime, float value)
        {
            DateTime = dateTime;
            Value = value;
        }   

        public DateTime DateTime
        {
            get;
            set;
        }

        public float Value
        {
            get;
            set;
        }

    }
}
