using MauiChartApp.ViewModel;

namespace MauiChartApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new ShellViewModel();
        }
    }
}