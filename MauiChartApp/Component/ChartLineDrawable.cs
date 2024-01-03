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
        private float leftMargin = 26f, topMargin = 14f, bottomMargin = 32, rightMargin = 20f;
        private float LineWidth = 1f;        
        //
        IFont fontDraw = new FontDraw(); 
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
            float scaleFactor = canvas.DisplayScale;
            

            canvas.Font = fontDraw;            
            float hourWidth = rect.Width / dayHours;

            // fill rectangle
            canvas.FillColor = _BackgroundColor;
            canvas.SetShadow(new SizeF(4, 4), 4, Colors.Grey);
            canvas.FillRoundedRectangle(rect.Inflate(-6f, -6f), 4);
            fontDraw = new Font("Calibri");
            canvas.Font = fontDraw;
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 18;
            canvas.SetShadow(new SizeF(6, 6), 4, Colors.Gray);
            SizeF size = canvas.GetStringSize(_Title, fontDraw, 18);
            canvas.DrawString(_Title, (width/2-size.Width/2), (topMargin + size.Height) , HorizontalAlignment.Left);
            canvas.ResetState();
            canvas.FontSize = 12;
            //Y Axis
            Point ptAxis = new Point(leftMargin + 2, topMargin);
            canvas.DrawString(_TitleY, (float)(ptAxis.X-size.Width/2) * scaleFactor, (float)ptAxis.Y * scaleFactor, HorizontalAlignment.Left);

            // X Axis
            size = canvas.GetStringSize(_TitleX, new FontDraw(), 12);
            ptAxis = new Point((float)(width-size.Width)*scaleFactor, dirtyRect.Bottom - bottomMargin);
            canvas.DrawString(_TitleX, (float)ptAxis.X * scaleFactor, ((float)ptAxis.Y-4) * scaleFactor, HorizontalAlignment.Right);

            // draw axis
            canvas.StrokeColor = _ColorLine;
            //x
            Point pt1 = new Point((rect.Left + leftMargin), (rect.Height - bottomMargin));
            Point pt2 = new Point((rect.Width-rightMargin), (rect.Height - bottomMargin));
            canvas.StrokeSize = LineWidth;
            canvas.StrokeColor = Colors.Red;
            canvas.DrawLine((float)pt1.X * scaleFactor, (float)pt1.Y * scaleFactor, (float)pt2.X * scaleFactor, (float)pt2.Y * scaleFactor);

            //y
            pt1 = new Point((rect.Left + leftMargin), (rect.Bottom - bottomMargin));
            pt2 = new Point((rect.Left + leftMargin), (rect.Top + topMargin));
            canvas.DrawLine((float)pt1.X * scaleFactor,(float)pt1.Y * scaleFactor, (float)pt2.X * scaleFactor, (float)pt2.Y * scaleFactor);

            int idx = 0;
            if (timeSeries.Count > 0)
            {
                float tempHeight = ((rect.Bottom-bottomMargin)-(rect.Top-topMargin));
                float tepmRange = (timeSeries.Max-timeSeries.Min);
                float tempRatio = (tempHeight/tepmRange);
                // zero
                float valZeroY = (float) Math.Abs(timeSeries.Min * tempRatio);
                Point zeroX = new Point((rect.Left+leftMargin), (rect.Bottom - bottomMargin - valZeroY));
                Point zeroY = new Point((rect.Right - rightMargin), (rect.Bottom - bottomMargin - valZeroY));
                
                canvas.StrokeDashPattern = new float[] { 2, 2 };
                canvas.StrokeColor = Colors.Blue;
                canvas.DrawLine((float)zeroX.X*scaleFactor, (float)zeroX.Y*scaleFactor, (float)zeroY.X*scaleFactor, (float)zeroY.Y*scaleFactor);
                canvas.DrawString("0", (float)zeroX.X*scaleFactor, (float)zeroX.Y*scaleFactor, HorizontalAlignment.Right);
                for (float temp = 1.0f; temp < timeSeries.Max; temp+=1.0f)
                {
                    float valGrad = temp * tempRatio;
                    Point ptG1 = new Point(zeroX.X , zeroX.Y-valGrad);
                    Point ptG2 = new Point(zeroY.X , zeroY.Y-valGrad);
                    canvas.StrokeColor = Colors.Red;
                    canvas.DrawLine((float)ptG1.X*scaleFactor, (float)ptG1.Y*scaleFactor, (float)ptG2.X*scaleFactor, (float)ptG2.Y*scaleFactor);
                    canvas.DrawString(temp.ToString(), (float)zeroX.X*scaleFactor,(float)ptG1.Y*scaleFactor, HorizontalAlignment.Right);
                }
                for (float temp = -1.0f; temp > timeSeries.Min; temp -= 1.0f)
                {
                    float valGrad = temp * tempRatio;
                    Point ptG1 = new Point(zeroX.X, (zeroX.Y - valGrad));
                    Point ptG2 = new Point(zeroY.X , (zeroY.Y - valGrad));
                    canvas.StrokeColor = Colors.Blue;
                    canvas.DrawLine((float)ptG1.X*scaleFactor, (float)ptG1.Y*scaleFactor, (float)ptG2.X*scaleFactor, (float)ptG2.Y*scaleFactor);
                    canvas.DrawString(temp.ToString(), (float)zeroX.X * scaleFactor, (float)ptG1.Y * scaleFactor, HorizontalAlignment.Right);
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
                        canvas.FillCircle(hrx*scaleFactor, y*scaleFactor, 2f);
                    }
                    else
                    {
                        canvas.StrokeColor = Colors.Red;
                        canvas.FillColor = Colors.Red;
                        float y = (float)zeroY.Y - (value.Value * tempRatio) ;
                        canvas.FillCircle(hrx * scaleFactor, y * scaleFactor, 2f);
                    }

                    if (value.DateTime.Hour % 4 == 0)
                    {
                        canvas.DrawString(value.DateTime.ToString("HH"), hrx * scaleFactor, (dirtyRect.Bottom - bottomMargin + size.Height) * scaleFactor, HorizontalAlignment.Left);
                    }

                    if (value.DateTime.Date.Equals(last.Date) == false)
                    {
                        canvas.DrawString(value.DateTime.ToString("dd.MM."), hrx * scaleFactor, (dirtyRect.Bottom - 1.2f*size.Height) * scaleFactor, HorizontalAlignment.Left);
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
            Weight = 18;
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
