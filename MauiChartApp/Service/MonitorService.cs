using MauiChartApp.Entity;
using MauiCommon.Entity;
using MauiCommon.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiChartApp.Service
{
    public interface IMonitorService
    {

    }

    public class MonitorService : IMonitorService
    {
        SQLiteConnection conn;
        string _dbPath = "";
        ILogService _log;

        public MonitorService(string dbPath, ILogService log) 
        {
            _dbPath = dbPath;
            _log = log;
            Init();
        }


        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<MonitorData>();
            _log.Info($"App DB Inizialization. Path:{_dbPath}");

        }
        public List<MonitorData> fatchData()
        {
            return conn.Table<MonitorData>().OrderByDescending(data => data.DateTime).Take(10).ToList<MonitorData>();
        }

    }
}
