using MauiChartApp.ModelView;

namespace MauiChartApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainPageModelView viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
           
        }
    }
}