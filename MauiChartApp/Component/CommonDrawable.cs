using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Component
{
    public class RandomLines : IDrawable
    {

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Microsoft.Maui.Graphics.Color.FromArgb("#003366");
            canvas.FillRectangle(dirtyRect);

            canvas.StrokeSize = 1;
            canvas.StrokeColor = Color.FromRgba(255, 255, 255, 100);
            Random Rand = new();
            for (int i = 0; i < 1000; i++)
            {
                canvas.DrawLine(
                    x1: (float)Rand.NextDouble() * dirtyRect.Width,
                    y1: (float)Rand.NextDouble() * dirtyRect.Height,
                    x2: (float)Rand.NextDouble() * dirtyRect.Width,
                    y2: (float)Rand.NextDouble() * dirtyRect.Height);
            }
        }
    }
}
