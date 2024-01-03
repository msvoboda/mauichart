using MauiCommon.Entity;
using MauiCommon.Model;
using SQLite;


namespace MauiCommon.Service
{

    public static class LogOptions
    {
        public static char Delimiter = '>';        
        public static string DateTimeMask = "yyyy.MM.dd HH:mm:ss";

        public static string LogTime()
        {
            DateTime now = DateTime.Now;

            return now.ToString(DateTimeMask);
        }
    }

    public class LogService : ILogService   
    {
        SQLiteConnection conn;
        string _dbPath = "";

        public LogService(string dbPath) 
        {
            _dbPath = dbPath;
            Init();
        } 

        private void Init()
        {
            if (conn != null)
                return;
            
            conn = new SQLiteConnection(_dbPath);           
            conn.CreateTable<LogItem>();
            Info($"DB Inizialization. Path:{_dbPath}");

        }

        public PageList<LogItem> LogItems()
        {
            return LogItems(0, 128);
        }

        public PageList<LogItem> LogItems(int startFrom, int count)
        {
            int totalCount = conn.Table<LogItem>().Count();
            List<LogItem> readLines = conn.Table<LogItem>().OrderByDescending(msg => msg.DateTime).Skip(startFrom).Take(count).ToList<LogItem>();
            return new PageList<LogItem>(readLines, startFrom, totalCount);
        }

        public void Debug(string message, string tag = "")
        {
            WriteMessage(message, LogType.Debug, tag);
        }

        public void Error(string message, string tag = "")
        {
            WriteMessage(message, LogType.Error, tag);
        }

        public void Info(string message, string tag = "")
        {
            WriteMessage(message, LogType.Info, tag);
        }

        public void Warning(string message, string tag = "")
        {
            WriteMessage(message, LogType.Warning, tag);
        }

        public void Write(string message, string tag = "")
        {
            WriteMessage(message, LogType.Info, tag);
        }


        private void WriteMessage(string message, LogType typ, string tag = "")
        {
            LogItem msg = new LogItem()
            {
                Tag = tag,
                Message = message,
                DateTime = DateTime.Now,
                Type = typ.ToString()
            };

            try
            {
                conn.Insert(msg, typeof(LogItem));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
