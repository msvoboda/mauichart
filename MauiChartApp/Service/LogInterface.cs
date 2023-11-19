using MauiChartApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Service
{
    public interface ILogService
    {
        // Write section
        
        void Debug(string message, string tag = "");


        void Error(string message, string tag = "");


        void Info(string message, string tag = "");

        void Warning(string message, string tag = "");

        // Read section
        List<LogItem> LogItems();


    }

    public enum LogType
    {
        Info, Debug,Warning, Error
    }
}
