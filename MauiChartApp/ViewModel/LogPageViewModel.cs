using Common.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiChartApp.Entity;
using MauiChartApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace MauiChartApp.ViewModel
{
    public partial class LogPageViewModel : BaseViewModel
    {
        ILogService _logService;

        public LogPageViewModel(ILogService logService) 
        {
            this.Title = "Logs";
            _logService = logService;
            GetLogList().Wait();
        }

       
        public ObservableCollection<LogItem> Items { get; set; }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetLogList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;

                Items = new ObservableCollection<LogItem>(_logService.LogItems());
                _logService.Debug($"Log Items: {Items.Count}");
            }
            catch(Exception ex)
            {                
                _logService.Error(ex.ToString());
            }
            finally { 
                IsLoading = false;
                IsRefreshing = false;
            }
        }

    }
}
