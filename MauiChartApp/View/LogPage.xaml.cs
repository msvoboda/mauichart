using MauiChartApp.ViewModel;

namespace MauiChartApp.View;

public partial class LogPage : ContentPage
{
	

	public LogPage(LogPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}