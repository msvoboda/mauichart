using MauiChartApp.ChartData;

namespace MauiChartApp.Component;

public partial class ChartView : ContentView
{
    ChartLineDrawable drawable;
    public ChartView()
	{
		InitializeComponent();    
        drawable =  new ChartLineDrawable();
        graphicsView.Drawable = drawable;
    }

    // Vytvoření BindableProperty pro vlastnost s možností vázání
    public static readonly BindableProperty TimeSeriesPropertyProperty =
        BindableProperty.Create(
            nameof(TimeSeriesProperty), // Název vlastnosti
            typeof(TimeSeries), // Datový typ vlastnosti
            typeof(ChartView), // Typ, který obsahuje vlastnost
            defaultValue: new TimeSeries(), // Výchozí hodnota
            propertyChanged: TimeSeriesPropertyChanged // Metoda, která se zavolá při změně hodnoty
        );
    
    public TimeSeries TimeSeriesProperty
    {
        get { return (TimeSeries)GetValue(TimeSeriesPropertyProperty); }
        set { SetValue(TimeSeriesPropertyProperty, value); }
    }
    
    private static void TimeSeriesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue != null)
        {
            ((ChartView)bindable).drawable.setDataSeries((TimeSeries)newValue);
            ((ChartView)bindable).InvalidateLayout();
        }
        ((ChartView)bindable).graphicsView.Invalidate();
    }


}