using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class SubscribeDao
    {
        public Logger logger = Logger._;
        public UserSubscribe QueryOrCreate(UserSubscribe param, string appid)
        {
            UserSubscribe result = this.Query(param, appid);
            if (result == null || result.open_id == null)
            {
                if (this.Create(param, appid) == 0)
                {
                    return null;
                }
                result = this.Query(param, appid);
            }
            return result;
        }
        public int Create(UserSubscribe wxsub, string appid)
        {
            logger.Info("UserInfo数据插入，" +
                " APPID: " + appid +
                " OPENID：" + wxsub.open_id +
                " UNIONID：" + wxsub.union_id
                );
            string connectionString = Common.GetDatabaseConnectString();
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String insertString = "INSERT INTO [TBL_SUBSCRIBE]("
                                    + "[APPID],[OPENID],[UNIONID],[SUB_TYPE],[SUB_EXPIRE]"
                                    + ") VALUES ("
                                    + "@APPID,@OPENID,@UNIONID,@SUB_TYPE,@SUB_EXPIRE"
                                    + ")";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection);
                    TimeSpan ts = DateTime.Now.AddDays(Common.getWXSubscribeFreeDays()) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    Int64 sub_expire = Convert.ToInt64(ts.TotalSeconds);

                    cmdInsert.Parameters.Add(new SqlParameter("@APPID", appid));
                    cmdInsert.Parameters.Add(new SqlParameter("@OPENID", wxsub.open_id));
                    cmdInsert.Parameters.Add(new SqlParameter("@UNIONID", wxsub.union_id));
                    cmdInsert.Parameters.Add(new SqlParameter("@SUB_TYPE", "free"));
                    cmdInsert.Parameters.Add(new SqlParameter("@SUB_EXPIRE", sub_expire));
                    // 执行操作
                    int row = cmdInsert.ExecuteNonQuery();
                    logger.Info("Subscribe数据插入，PARAM：" + cmdInsert.Parameters);

                    return row;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Subscribe数据插入失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }
            return 0;
        }

        public UserSubscribe Query(UserSubscribe wxsub, string appId)
        {
            UserSubscribe sub = null;
            string connectionString = Common.GetDatabaseConnectString();
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(1) [OPENID],[UNIONID],[SUB_TYPE],[SUB_EXPIRE] " +
                                       "FROM [TBL_SUBSCRIBE] " +
                                       "WHERE 1=1 AND " +
                                       " [APPID] = @APPID AND" +
                                       " [OPENID] = @OPENID ";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@APPID", appId));
                    cmdSelect.Parameters.Add(new SqlParameter("@OPENID", wxsub.open_id));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        sub = new UserSubscribe();
                        sub.open_id = dr["OPENID"].ToString();
                        sub.union_id = dr["UNIONID"].ToString();
                        sub.sub_type = dr["SUB_TYPE"].ToString();
                        sub.sub_expire = Convert.ToInt64(dr["SUB_EXPIRE"].ToString());
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Subscribe数据查询失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }
            return sub;
        }
    }
}