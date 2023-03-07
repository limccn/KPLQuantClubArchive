using WeixinMiniUser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using WeixinCommon;

namespace WeixinMiniUser.Daos
{
    public class UserInfoDao
    {
        public Logger logger = Logger._;
        public int Create(WXUserInfo wxuserinfo, string appId)
        {
            logger.Info("UserInfo数据插入，" +
                " APPID: "+ appId +
                " OPENID：" + wxuserinfo.openId +
                " UNIONID：" + wxuserinfo.unionId +
                " NICK_NAME：" + wxuserinfo.nickName +
                " GENDER：" + wxuserinfo.gender +
                " CITY：" + wxuserinfo.city +
                " PROVINCE：" + wxuserinfo.province +
                " COUNTRY：" + wxuserinfo.country +
                " AVATAR_URL：" + wxuserinfo.avatarUrl
                );

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String insertString = "INSERT INTO [TBL_USER_INFO]("
                                    + "[APPID],[OPENID],[UNIONID],[NAME],[NICK_NAME],[GENDER],[CITY],[PROVINCE],[COUNTRY],[AVATAR_URL]"
                                    + ") VALUES ("
                                    + "@APPID,@OPENID,@UNIONID,@NAME,@NICK_NAME,@GENDER,@CITY,@PROVINCE,@COUNTRY,@AVATAR_URL"
                                    + ")";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection);
                    cmdInsert.Parameters.Add(new SqlParameter("@APPID", appId));
                    cmdInsert.Parameters.Add(new SqlParameter("@OPENID", wxuserinfo.openId));
                    cmdInsert.Parameters.Add(new SqlParameter("@UNIONID", wxuserinfo.unionId == null ? "" : wxuserinfo.unionId));
                    cmdInsert.Parameters.Add(new SqlParameter("@NAME", Guid.NewGuid().ToString("N")));
                    cmdInsert.Parameters.Add(new SqlParameter("@NICK_NAME", wxuserinfo.nickName == null ? "" : wxuserinfo.nickName));
                    cmdInsert.Parameters.Add(new SqlParameter("@GENDER", wxuserinfo.gender == null ? "" : wxuserinfo.gender));
                    cmdInsert.Parameters.Add(new SqlParameter("@CITY", wxuserinfo.city == null ? "" : wxuserinfo.city));
                    cmdInsert.Parameters.Add(new SqlParameter("@PROVINCE", wxuserinfo.province == null ? "" : wxuserinfo.province));
                    cmdInsert.Parameters.Add(new SqlParameter("@COUNTRY", wxuserinfo.country == null ? "" : wxuserinfo.country));
                    cmdInsert.Parameters.Add(new SqlParameter("@AVATAR_URL", wxuserinfo.avatarUrl == null ? "" : wxuserinfo.avatarUrl));
                    // 执行操作
                    int row = cmdInsert.ExecuteNonQuery();
                    logger.Info("UserInfo数据插入，UserInfo：" + wxuserinfo);
                    return row;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("UserInfo数据插入失败", ex);
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


        public int UpdateUnionID(WXUserInfo wxuserinfo, string appId)
        {
            logger.Info("UserInfo数据更新UNIONID，" + 
                " APPID: " + appId +
                " OPENID：" + wxuserinfo.openId +
                " UNIONID：" + wxuserinfo.unionId
                );
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String updateString = "UPDATE [TBL_USER_INFO] SET "
                                    + " [UNIONID] = @UNIONID, "
                                    + " [UPDATE_TIME] = @UPDATE_TIME "
                                    + "WHERE 1=1 AND "
                                    + " [APPID] = @APPID AND"
                                    + " [OPENID] = @OPENID";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdUpdate = new SqlCommand(updateString, destinationConnection);
                    cmdUpdate.Parameters.Add(new SqlParameter("@APPID", appId));
                    cmdUpdate.Parameters.Add(new SqlParameter("@OPENID", wxuserinfo.openId));
                    cmdUpdate.Parameters.Add(new SqlParameter("@UNIONID", wxuserinfo.unionId == null? "": wxuserinfo.unionId));
                    cmdUpdate.Parameters.Add(new SqlParameter("@UPDATE_TIME", DateTime.Now));

                    int row = cmdUpdate.ExecuteNonQuery();

                    return row;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("UserInfo数据更新失败", ex);
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

        public int UpdateDetail(WXUserInfo wxuserinfo, string appId)
        {
            logger.Info("UserInfo数据更新Detail，" +
                " APPID: " + appId +
                " OPENID：" + wxuserinfo.openId +
                " UNIONID：" + wxuserinfo.unionId +
                " NICK_NAME：" + wxuserinfo.nickName +
                " GENDER：" + wxuserinfo.gender +
                " CITY：" + wxuserinfo.city +
                " PROVINCE：" + wxuserinfo.province +
                " COUNTRY：" + wxuserinfo.country +
                " AVATAR_URL：" + wxuserinfo.avatarUrl
                );
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String updateString = "UPDATE [TBL_USER_INFO] SET "
                                    + " [NICK_NAME] = @NICK_NAME, "
                                    + " [GENDER] = @GENDER, "
                                    + " [CITY] = @CITY, "
                                    + " [PROVINCE] = @PROVINCE, "
                                    + " [COUNTRY] = @COUNTRY, "
                                    + " [AVATAR_URL] = @AVATAR_URL, "
                                    + " [UPDATE_TIME] = @UPDATE_TIME, "
                                    + " [COUNT] = [COUNT] + 1 "
                                    + "WHERE 1=1 AND "
                                    + " [APPID] = @APPID AND"
                                    + " [OPENID] = @OPENID";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdUpdate = new SqlCommand(updateString, destinationConnection);
                    cmdUpdate.Parameters.Add(new SqlParameter("@APPID", appId));
                    cmdUpdate.Parameters.Add(new SqlParameter("@OPENID", wxuserinfo.openId));
                    cmdUpdate.Parameters.Add(new SqlParameter("@NICK_NAME", wxuserinfo.nickName == null ? "" : wxuserinfo.nickName));
                    cmdUpdate.Parameters.Add(new SqlParameter("@GENDER", wxuserinfo.gender == null ? "" : wxuserinfo.gender));
                    cmdUpdate.Parameters.Add(new SqlParameter("@CITY", wxuserinfo.city == null ? "" : wxuserinfo.city));
                    cmdUpdate.Parameters.Add(new SqlParameter("@PROVINCE", wxuserinfo.province == null ? "" : wxuserinfo.province));
                    cmdUpdate.Parameters.Add(new SqlParameter("@COUNTRY", wxuserinfo.country == null ? "" : wxuserinfo.country));
                    cmdUpdate.Parameters.Add(new SqlParameter("@AVATAR_URL", wxuserinfo.avatarUrl == null ? "" : wxuserinfo.avatarUrl));
                    cmdUpdate.Parameters.Add(new SqlParameter("@UPDATE_TIME", DateTime.Now));

                    int row = cmdUpdate.ExecuteNonQuery();

                    return row;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("UserInfo数据更新失败", ex);
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

        public WXUserInfo Query(string openid, string appid)
        {
            WXUserInfo wxuser = null;
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(1) [OPENID],[UNIONID],[NICK_NAME],[GENDER],[CITY],[PROVINCE],[COUNTRY],[AVATAR_URL] FROM [TBL_USER_INFO] WHERE [OPENID] = @OPENID AND [APPID]=@APPID";
                try
                {
                    destinationConnection.Open();
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@APPID", appid));
                    cmdSelect.Parameters.Add(new SqlParameter("@OPENID", openid));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        wxuser = new WXUserInfo();
                        wxuser.openId = dr["OPENID"].ToString();
                        wxuser.unionId = dr["UNIONID"].ToString();
                        wxuser.nickName = dr["NICK_NAME"].ToString();
                        wxuser.gender = dr["GENDER"].ToString();
                        wxuser.city = dr["CITY"].ToString();
                        wxuser.province = dr["PROVINCE"].ToString();
                        wxuser.country = dr["COUNTRY"].ToString();
                        wxuser.avatarUrl = dr["AVATAR_URL"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("UserInfo数据查询失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }
            return wxuser;
        }

        public WXUserInfo CreateOrUpdateUnionID(WXUserInfo wxuserinfo, string appid)
        {
            WXUserInfo result = this.Query(wxuserinfo.openId ,appid);
            if (result != null && result.openId != null)
            {
                if (this.UpdateUnionID(wxuserinfo,appid) == 0)
                {
                    return null;
                }
            }
            else
            {
                if(this.Create(wxuserinfo,appid) == 0)
                {
                    return null;
                }
            }
            return this.Query(wxuserinfo.openId, appid);
        }


        public WXUserInfo CreateOrUpdateDetail(WXUserInfo wxuserinfo,string appid)
        {
            WXUserInfo result = this.Query(wxuserinfo.openId, appid);
            if (result != null && result.openId != null)
            {
                if (this.UpdateDetail(wxuserinfo,appid) == 0)
                {
                    return null;
                }
            }
            else
            {
                if (this.Create(wxuserinfo,appid) == 0)
                {
                    return null;
                }
            }
            return this.Query(wxuserinfo.openId, appid);
        }
    }
}