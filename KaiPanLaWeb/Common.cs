using KaiPanLaCommon;
using System;
using System.Configuration;

namespace KaiPanLaWeb
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

        public static bool isBusinessTime(DateTime dt)
        {
            DateTime startTime = DateTime.Parse(Common.GetAppSettingByKey("BusinessStartTime"));
            DateTime endTime = DateTime.Parse(Common.GetAppSettingByKey("BusinessEndTime"));

            return (DateTime.Compare(dt, startTime) >= 0)
                 && (DateTime.Compare(dt, endTime) <= 0);
        }

        public static string getWXAppId()
        {
            return Common.GetAppSettingByKey("WXMiniProgramAppId");
        }

        public static string getWXAppSecret()
        {
            return Common.GetAppSettingByKey("WXMiniProgramAppSecret");
        }

        public static int getAntiHijackDelay()
        {
            int delay = 30;
            string config_delay = Common.GetAppSettingByKey("AntiHijackDelay");
            if (Int32.TryParse(config_delay, out delay))
            {
                return delay > 0 ? delay : 30;
            }
            return delay;
        }

        public static int getWXSubscribeFreeDays()
        {
            int days = 7;
            string config_days = Common.GetAppSettingByKey("WXSubscribeFreeDays");
            if (Int32.TryParse(config_days, out days))
            {
                return days > 0 ? days : 7;
            }
            return days;
        }


        public static DateTime convertTimeStamp(Int64 timestamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = new TimeSpan(timestamp * 10000000);
            return dateTimeStart.Add(toNow);
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

        public static DateTime ComputeDate(Int32 date)
        {
            DateTime dtToday = DateTime.Now;
            DateTime qDate = DateTime.Now;
            if (date >= 20201001 && date <= 20491231)
            {
                if (DateTime.TryParseExact(date.ToString(),
                                        "yyyyMMdd",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None,
                                        out qDate))
                {

                    if (DateTime.Compare(qDate, dtToday) > 0)
                    {
                        //晚于今天
                        qDate = dtToday;
                    }
                    else if (DateTime.Compare(qDate, dtToday.AddYears(-2)) < 0)
                    {
                        //早于2年前
                        qDate = dtToday.AddYears(-2);
                    }
                    else
                    {
                        // no up
                    }
                }
                else
                {
                    qDate = dtToday;
                }
            }
            else
            {
                //不正常时间
                qDate = dtToday;
            }

            return qDate;
        }

        public static int ComputeTime(Int32 time)
        {
            int qTime = 1500;
            if (time >= 0 && time < 920)
            {
                qTime = 1500;
            }
            else if (time >= 920 && time < 1000)
            {
                qTime = time;
            }
            else if (time >= 1000 && time < 1130)
            {
                qTime = time;
            }
            else if (time >= 1130 && time < 1300)
            {
                qTime = 1130;
            }
            else if (time >= 1300 && time < 1500)
            {
                qTime = time;
            }
            else if (time >= 1500 && time < 9999)
            {
                qTime = 1500;
            }
            else
            {
                qTime = 1500;
            }
            return qTime;
        }
    }

}
