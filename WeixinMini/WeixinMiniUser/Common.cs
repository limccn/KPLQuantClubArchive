using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Security.Policy;
using WeixinCommon;

namespace WeixinMiniUser
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

        public static string getWXAppId()
        {
            return Common.GetAppSettingByKey("WXMiniProgramAppId");
        }

        public static string getWXAppSecret()
        {
            return Common.GetAppSettingByKey("WXMiniProgramAppSecret");
        }

        public static int getWXSubscribeFreeDays()
        {
            int days = 120;
            string config_days = Common.GetAppSettingByKey("WXSubscribeFreeDays");
            if (Int32.TryParse(config_days,out days))
            {
                return days > 0 ? days: 120;
            }
            return days ;
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
    }

}
