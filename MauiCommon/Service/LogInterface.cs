using MauiCommon.Entity;
using MauiCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCommon.Service
{
    public interface ILogService
    {
        // Write section
        
        void Debug(string message, string tag = "");


        void Error(string message, string tag = "");


        void Info(string message, string tag = "");

        void Warning(string message, string tag = "");

        // Read section
        PageList<LogItem> LogItems();
        PageList<LogItem> LogItems(int start, int count);


    }

    public enum LogType
    {
        Info, Debug,Warning, Error
    }
}
