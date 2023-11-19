using Common.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
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


        async Task GetLogList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;

                Items = new ObservableCollection<LogItem>(_logService.LogItems());
            }
            catch(Exception ex)
            {
                IsLoading=false;
                _logService.Error(ex.ToString());
            }
            finally { IsLoading = false; }
        }

    }
}
