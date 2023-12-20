using MauiChartApp.ChartData;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = Microsoft.Maui.Graphics.Font;

namespace MauiChartApp.Component
{

    public class ChartLineDrawable : IDrawable
    {
        private Color _BackgroundColor;
        private Color _ColorTitle = Colors.Red;
        private Color _ColorAxis;
        private Color _ColorLine;
        private Color _ColorText;
        //
        private String _Title = "Temperature chart";
        private String _TitleX = "Date";
        private String _TitleY = "Daily";
        private float leftMargin = 14f, topMargin = 10f, bottomMargin = 26, rightMargin = 20f;
        private float LineWidth = 1f;        
        //
        FontDraw fontDraw = new FontDraw(); 
        // DATA SERIES
        TimeSeries timeSeries = new TimeSeries();
        int dayHours = 128;    

        public ChartLineDrawable()
        {
            _ColorAxis = Colors.DarkGray;
            _ColorText = Colors.Black;
            _ColorLine = Colors.Black;
            _ColorTitle = Colors.IndianRed;
            _BackgroundColor = Colors.LightGray;
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            RectF rect = dirtyRect;
            float scaleFactor = 1f;

            canvas.Font = fontDraw;
            SizeF size = canvas.GetStringSize(_Title, new FontDraw(), 12);
            float hourWidth = rect.Width / dayHours;

            // fill rectangle
            canvas.FillColor = _BackgroundColor;
            canvas.SetShadow(new SizeF(4, 4), 4, Colors.Grey);
            canvas.FillRoundedRectangle(rect.Inflate(-6f, -6f), 4);

            canvas.Font = new Font("Arial");
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 18;
            canvas.SetShadow(new SizeF(6, 6), 4, Colors.Gray);
            canvas.DrawString(_Title, width/2-size.Width/2, (topMargin + size.Height) * scaleFactor, HorizontalAlignment.Center);
            canvas.ResetState();
            canvas.FontSize = 10;
            //Y Axis
            Point ptAxis = new Point(leftMargin + 2, topMargin - 4);
            canvas.DrawString(_TitleY, (float)ptAxis.X * scaleFactor, (float)ptAxis.Y * scaleFactor, HorizontalAlignment.Left);

            // X Axis
            ptAxis = new Point(dirtyRect.Right - rightMargin, dirtyRect.Bottom - bottomMargin);
            canvas.DrawString(_TitleX, (float)ptAxis.X * scaleFactor, ((float)ptAxis.Y-4) * scaleFactor, HorizontalAlignment.Left);

            // draw axis
            canvas.StrokeColor = _ColorLine;
            //x
            Point pt1 = new Point((rect.Left + leftMargin) * scaleFactor, (rect.Height - bottomMargin)*scaleFactor);
            Point pt2 = new Point((rect.Width-rightMargin) * scaleFactor, (rect.Height - bottomMargin) * scaleFactor);
            canvas.StrokeSize = LineWidth;
            canvas.StrokeColor = Colors.Red;
            canvas.DrawLine((float)pt1.X, (float)pt1.Y, (float)pt2.X, (float)pt2.Y);

            //y
            pt1 = new Point((rect.Left + leftMargin)*scaleFactor, (rect.Bottom - bottomMargin)*scaleFactor);
            pt2 = new Point((rect.Left + leftMargin)*scaleFactor, (rect.Top + topMargin)*scaleFactor);
            canvas.DrawLine((float)pt1.X,(float) pt1.Y, (float)pt2.X, (float)pt2.Y);

            int idx = 0;
            if (timeSeries.Count > 0)
            {
                float tempHeight = ((rect.Bottom-bottomMargin)-(rect.Top-topMargin)) * scaleFactor;
                float tepmRange = (timeSeries.Max-timeSeries.Min);
                float tempRatio = (tempHeight/tepmRange);
                // zero
                float valZeroY = (float) Math.Abs(timeSeries.Min * tempRatio);
                Point zeroX = new Point((rect.Left+leftMargin) * scaleFactor, (rect.Bottom - bottomMargin - valZeroY) * scaleFactor);
                Point zeroY = new Point((rect.Right - rightMargin) * scaleFactor, (rect.Bottom - bottomMargin - valZeroY) * scaleFactor);
                
                canvas.StrokeDashPattern = new float[] { 2, 2 };
                canvas.StrokeColor = Colors.Blue;
                canvas.DrawLine(zeroX, zeroY);
                canvas.DrawString("0", (float)zeroX.X, (float)zeroX.Y, HorizontalAlignment.Right);
                for (float temp = 1.0f; temp < timeSeries.Max; temp+=1.0f)
                {
                    float valGrad = temp * tempRatio;
                    Point ptG1 = new Point(zeroX.X , zeroX.Y-valGrad);
                    Point ptG2 = new Point(zeroY.X , zeroY.Y-valGrad);
                    canvas.StrokeColor = Colors.Red;
                    canvas.DrawLine(ptG1, ptG2);
                    canvas.DrawString(temp.ToString(), (float)zeroX.X,(float)ptG1.Y, HorizontalAlignment.Right);
                }
                for (float temp = -1.0f; temp > timeSeries.Min; temp -= 1.0f)
                {
                    float valGrad = temp * tempRatio;
                    Point ptG1 = new Point(zeroX.X * scaleFactor, (zeroX.Y - valGrad) * scaleFactor);
                    Point ptG2 = new Point(zeroY.X * scaleFactor, (zeroY.Y - valGrad) * scaleFactor);
                    canvas.StrokeColor = Colors.Blue;
                    canvas.DrawLine(ptG1, ptG2);
                }

                canvas.StrokeDashPattern = null;
                DateTime last = DateTime.Now.Subtract(new TimeSpan(1,0,0,0));
                for (float hrx = leftMargin; hrx <= (rect.Width - rightMargin); hrx += hourWidth)
                {
                    canvas.SetShadow(new SizeF(4, 4), 4, Colors.Grey);
                    TimeValue value = timeSeries[idx++];                   
                    if (value.Value < 0)
                    {
                        canvas.StrokeColor = Colors.Blue;
                        canvas.FillColor = Colors.Blue;
                        float y = ((rect.Bottom - bottomMargin)  - Math.Abs(timeSeries.Min-value.Value)*tempRatio);                        
                        canvas.FillCircle(hrx, y, 2f);
                    }
                    else
                    {
                        canvas.StrokeColor = Colors.Red;
                        canvas.FillColor = Colors.Red;
                        float y = (float)zeroY.Y * scaleFactor - (value.Value * tempRatio) ;
                        canvas.FillCircle(hrx, y, 2f);
                    }

                    if (value.DateTime.Hour % 4 == 0)
                    {
                        canvas.DrawString(value.DateTime.ToString("HH"), hrx * scaleFactor, (dirtyRect.Bottom - bottomMargin + size.Height) * scaleFactor, HorizontalAlignment.Left);
                    }

                    if (value.DateTime.Date.Equals(last.Date) == false)
                    {
                        canvas.DrawString(value.DateTime.ToString("dd.MM."), hrx * scaleFactor, (dirtyRect.Bottom - bottomMargin + 1.5f*size.Height) * scaleFactor, HorizontalAlignment.Left);
                        last = value.DateTime;                    
                    }

                }
            }
           
        }

        public void setDataSeries(TimeSeries timeValues) {
            this.timeSeries = timeValues;
        }
    }

    public class FontDraw : IFont
    {
        public FontDraw() 
        {
            Weight = 12;
            StyleType = FontStyleType.Normal;
        }

        public string Name {
            get; set;
        }
        public int Weight { get; set; }
        public FontStyleType StyleType { get; set; }
    }

    public class AttributeText : IAttributedText
    {
        public string Text {
            get; set;
        }

        public IReadOnlyList<IAttributedTextRun> Runs {
            get; set;
        }
    }
}
