using KaiPanLaCommon;
using System;
using System.Configuration;

namespace KaiPanLaPortfolio
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

        public static bool IsBusinessDate(DateTime dt)
        {
            return (dt.DayOfWeek < DayOfWeek.Saturday && dt.DayOfWeek > DayOfWeek.Sunday);
        }

        public static bool IsMonitorTime(DateTime dt)
        {
            //  
            DateTime startTime = DateTime.Parse(Common.GetAppSettingByKey("MonitorStartTime"));
            DateTime endTime = DateTime.Parse(Common.GetAppSettingByKey("MonitorEndTime"));

            return IsBusinessDate(dt)
                 && (DateTime.Compare(dt, startTime) >= 0)
                 && (DateTime.Compare(dt, endTime) <= 0);
        }

        public static string GetApplicationVersion()
        {
            return Common.GetAppSettingByKey("ApplicationVersion");
        }
        public static string GetPortfolioApiRequestUrl()
        {
            return Common.GetAppSettingByKey("PortfolioPostUrl");
        }

        public static string GetPortfolioHistoryTableName()
        {
            return Common.GetAppSettingByKey("PortfolioHistoryTableName");
        }

        public static string GetPortfolioLatestTableName()
        {
            return Common.GetAppSettingByKey("PortfolioLatestTableName");
        }

        public static string GetPortfolioSignalTableName()
        {
            return Common.GetAppSettingByKey("PortfolioSignalTableName");
        }

        public static bool GetShouldWriteDataToDB()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("WriteDataToDB"));
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

        public static DateTime ConvertTimeStamp(Int64 timestamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000000);
            return dateTimeStart.Add(toNow);
        }
    }

}
