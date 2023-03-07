using KaiPanLaCommon;
using System;
using System.Configuration;

namespace KaiPanLaPlate
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

        public static Boolean DATABASE_CHECK_STATE = false;

        public static bool isBusinessDate(DateTime dt)
        {
            if (Common.getIgnoreBusinessDateCheck())
            {
                return true;
            }
            else
            {
                return (dt.DayOfWeek < DayOfWeek.Saturday && dt.DayOfWeek > DayOfWeek.Sunday);
            }

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

        public static bool isMorningSessionTime(DateTime dt)
        {
            DateTime startTime = DateTime.Parse("09:00:00");
            DateTime endTime = DateTime.Parse("11:30:30"); // 30秒延迟，处理延迟数据
            return isBusinessDate(dt)
                && (DateTime.Compare(dt, startTime) >= 0)
                && (DateTime.Compare(dt, endTime) <= 0);
        }

        public static bool isAfternoonSessionTime(DateTime dt)
        {
            DateTime startTime = DateTime.Parse("12:59:30"); // 30秒提前，处理提前数据
            DateTime endTime = DateTime.Parse("15:00:30"); // 30秒延迟，处理延迟数据
            return isBusinessDate(dt)
                && (DateTime.Compare(dt, startTime) >= 0)
                && (DateTime.Compare(dt, endTime) <= 0);
        }

        public static bool isSessionTime(DateTime dt)
        {
            return isMorningSessionTime(dt)
                || isAfternoonSessionTime(dt);
        }

        public static bool isFilterdTime(DateTime dt)
        {
            //筛选时间
            int sec = dt.Second;
            int val = (int)sec / 10;
            if (val == 0 || val == 3)
                return true;
            else
                return false;
        }


        public static string getPlateListApiRequestUrl(
            string c = "PCArrangeData",
            string a = "GetZSIndexPlate",
            string seltype = "2",
            string zstype = "7",
            string ptype = "1",
            string porder = "1",
            string pstart = "",
            string pend = "",
            string pindex = "0",
            string pst = "10",
            string userid = "",
            string token = "")
        {

            if (c.Equals("PCArrangeData"))
            {
                string url = Common.GetAppSettingByKey("PlateListUrl");
                return String.Format(c, a, seltype, zstype, ptype, porder, pstart, pend, pindex, pst, userid, token);
            }
            else
            {
                return null;
            }
        }

        public static string getPlateStocksListApiRequestUrl(
            string c = "PCArrangeData",
            string a = "GetZSIndexPlate",
            string seltype = "3",
            string ltype = "6",
            string lorder = "1",
            string lstart = "",
            string lend = "",
            string lindex = "0",
            string lst = "10",
            string plateid = "",
            string userid = "",
            string token = "")
        {

            if (c.Equals("PCArrangeData"))
            {
                string url = Common.GetAppSettingByKey("PlateStocksListUrl");
                return String.Format(c, a, seltype, ltype, lorder, lstart, lend, lindex, lst, plateid, userid, token);
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
        public static string getPlatePostRequestUrl()
        {
            return Common.GetAppSettingByKey("PlatePostUrl");
        }

        public static bool getShouldWriteDataToDB()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("WriteDataToDB"));
        }

        public static bool getShouldMarkUpSelectedRows()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("MarkUpSelectedRows"));
        }

        public static bool getShouldFriendlyRowDataColor()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("FriendlyRowDataColor"));
        }

        public static bool getIsLightWeightMode()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("LightWeightMode"));
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

        public static bool getIgnoreBusinessDateCheck()
        {
            return Boolean.Parse(Common.GetAppSettingByKey("IgnoreBusinessDateCheck"));
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
