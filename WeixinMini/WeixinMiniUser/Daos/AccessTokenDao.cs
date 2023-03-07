using WeixinMiniUser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WeixinMiniUser.Daos
{
    public class AccessTokenDao
    {

        public void QueryTopOne(UserAccessToken wxtoken)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(1) [OPENID],[UNIONID],[ACCESS_TOKEN],[EXPIRE] " +
                    "FROM [TBL_ACCESS_TOKEN] WHERE [OPENID] = @OPENID";
                try
                {
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@OPENID", wxtoken.open_id));
                    SqlDataReader dr = cmdSelect.ExecuteReader();//调用ExecuteReader()方法执行SELECT 语句
                    UserAccessToken token = null;
                    if (dr.Read())
                    {
                        token = new UserAccessToken();
                        token.open_id = dr["OPENID"].ToString();
                        token.union_id = dr["UNIONID"].ToString();
                        token.access_token = dr["ACCESS_TOKEN"].ToString();
                        token.expire = Convert.ToInt64(dr["EXPIRE"].ToString());
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }
        }

        public UserAccessToken Create(UserAccessToken wxtoken)
        {
            UserAccessToken token = null;

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String insertString = "INSERT INTO [TBL_ACCESS_TOKEN]("
                                    + "[OPENID],[UNIONID],[ACCESS_TOKEN],[EXPIRE]"
                                    + ") VALUES ("
                                    + "@OPENID,@UNIONID,@ACCESS_TOKEN,@EXPIRE"
                                    + ")";
                try
                {
                    SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection);

                    token = new UserAccessToken();
                    token.open_id = wxtoken.open_id;
                    token.union_id = wxtoken.union_id;
                    token.access_token = Guid.NewGuid().ToString("N");

                    TimeSpan ts = DateTime.Now.AddDays(60) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    token.expire = Convert.ToInt64(ts.TotalSeconds); //60天有效 

                    cmdInsert.Parameters.Add(new SqlParameter("@OPENID", token.open_id));
                    cmdInsert.Parameters.Add(new SqlParameter("@UNIONID", token.union_id));
                    cmdInsert.Parameters.Add(new SqlParameter("@ACCESS_TOKEN", token.access_token));
                    cmdInsert.Parameters.Add(new SqlParameter("@EXPIRE", token.expire));
                    // 执行操作
                    int row = cmdInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }

            return token;
        }

        public UserAccessToken Update(UserAccessToken wxtoken)
        {
            UserAccessToken token = null;

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String updateString = "UPDATE [TBL_ACCESS_TOKEN] SET"
                                    + "[ACCESS_TOKEN] = @NEW_ACCESS_TOKEN,　"
                                    + "[EXPIRE] = @EXPIRE"
                                    + "WHERE [OPENID] = @OPENID "
                                    + "AND [ACCESS_TOKEN] = @ACCESS_TOKEN";
                try
                {
                    SqlCommand cmdUpdate = new SqlCommand(updateString, destinationConnection);

                    token = new UserAccessToken();
                    token.open_id = wxtoken.open_id;
                    token.union_id = wxtoken.union_id;
                    token.access_token = Guid.NewGuid().ToString("N");

                    TimeSpan ts = DateTime.Now.AddDays(60) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    token.expire = Convert.ToInt64(ts.TotalSeconds); //60天有效 

                    cmdUpdate.Parameters.Add(new SqlParameter("@OPENID", wxtoken.open_id));
                    cmdUpdate.Parameters.Add(new SqlParameter("@NEW_ACCESS_TOKEN", token.access_token));
                    cmdUpdate.Parameters.Add(new SqlParameter("@ACCESS_TOKEN", wxtoken.access_token));
                    cmdUpdate.Parameters.Add(new SqlParameter("@EXPIRE", token.expire));
                    // 执行操作
                    int row = cmdUpdate.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
            }

            return token;
        }
    }
}