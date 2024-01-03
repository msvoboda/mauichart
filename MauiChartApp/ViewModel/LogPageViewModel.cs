using Common.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiCommon.Entity;
using MauiCommon.Model;
using MauiCommon.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiChartApp.ViewModel
{
    public partial class LogPageViewModel : BaseViewModel
    {
        ILogService _logService;
        int _start = 0;

    #if __ANDROID__
                int _count =20;
    #else
            int _count = 120;
    #endif

        public LogPageViewModel(ILogService logService) 
        {
            this.Title = "Logs";
            _logService = logService;
            LoadMoreItemsCommand = new Command(ExecuteLoadMoreItemsCommand);
            GetLogList().Wait();
        }

        private void ExecuteLoadMoreItemsCommand()
        {
            GetLogList();
        }


        public ObservableCollection<LogItem> Items { get; set; }

        [ObservableProperty]
        bool isRefreshing;

        public ICommand LoadMoreItemsCommand { get; private set; }

        [RelayCommand]
        async Task GetLogList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;

                PageList<LogItem> addList = _logService.LogItems(_start, _count);
                _start = addList.Count;                
                if (Items == null)
                {
                    Items = new ObservableCollection<LogItem>(addList);
                }
                else
                {
                    foreach (LogItem item in addList)
                    { 
                        Items.Add(item);
                    }
                }
                RemainingItems = addList.TotalCount - addList.Count - 1;
                _logService.Debug($"Log Items: {Items.Count}, Remaining: {RemainingItems}");
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

        [ObservableProperty]
        public int remainingItems;
     

    }
}
