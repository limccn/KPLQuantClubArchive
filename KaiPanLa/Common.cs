using KaiPanLaCommon;
using System;
using System.Configuration;

namespace KaiPanLa
{
    class Common
    {
        public static Logger logger = Logger._;

        public static string GetAppSettingByKey(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                return result;
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine(ex);
                logger.Warn(String.Format("获取配置失败,key={0}", key), ex);

                return null;
            }
        }

        public static String DATABASE_CONNECT_STRING = Common.GetDatabaseConnectString();

        public static bool isBusinessDate(DateTime dt)
        {
            return (dt.DayOfWeek < DayOfWeek.Saturday && dt.DayOfWeek > DayOfWeek.Sunday);
        }

        public static bool isBusinessTime(DateTime dt)
        {
            //  
            DateTime startTime = DateTime.Parse(Common.GetAppSettingByKey("BusinessStartTime"));
            DateTime endTime = DateTime.Parse(Common.GetAppSettingByKey("BusinessEndTime"));

            return isBusinessDate(dt)
                 && (DateTime.Compare(dt, startTime) >= 0)
                 && (DateTime.Compare(dt, endTime) <= 0);
        }


        public static string getApiRequestUrl1(
            string c = "StockRanking",
            string a = "RealRankingInfo",
            string date = "",
            string rstart = "",
            string rend = "",
            string ratio = "6",
            string type = "6",
            string order = "1",
            string index = "1",
            string st = "200",
            string userId = "",
            string token = "")
        {

            if (c.Equals("StockRanking"))
            {
                string url = Common.GetAppSettingByKey("StockRankingUrl");
                return String.Format(url, c, a, date, rstart, rend, ratio, type, order, index, st, userId, token);
            }
            else
            {
                return null;
            }
        }

        public static string getApplicationVersion()
        {
            return Common.GetAppSettingByKey("ApplicationVersion");
        }
        public static string getStockRankingApiRequestUrl()
        {
            return Common.GetAppSettingByKey("StockRankingPostUrl");
        }

        public static bool getShouldWriteDataToDB()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("WriteDataToDB"));
        }


        public static bool getShouldTruncateMemoryTable()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("TruncateMemoryTable"));
        }

        public static bool getShouldWriteLatestAnalyseToMemoryTable()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("WriteLatestAnalyseToMemoryTable"));
        }

        public static bool getShouldWatchSignalRealtimeUpdateTable()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("WatchSignalRealtimeUpdateTable"));
        }

        public static string GetDatabaseConnectString()
        {
            string connectionString = GetConnectionStringsConfig("sqlserver");
            if (connectionString == null)
            {
                Console.WriteLine("不正确的数据库连接字符串");
                throw new Exception("不正确的数据库连接字符串");
            }
            return connectionString;
        }

        public static string GetConnectionStringsConfig(string connectionName)
        {
            try
            {
                string connectionString =
                    ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();
                return connectionString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("不正确的数据库连接字符串配置", ex);
                return null;
            }
        }

        public static DateTime convertTimeStamp(Int64 timestamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000000);
            return dateTimeStart.Add(toNow);
        }
    }

}
