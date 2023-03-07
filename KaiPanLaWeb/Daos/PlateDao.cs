using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class PlateDao
    {

        public Logger logger = Logger._;

        public List<Plate> QueryPlates(DateTime date, int time, Int64 count)
        {
            DataTable dtInDb = QueryDataTableFromDB(date, time, count);
            DataTableToEntity<Plate> util = new DataTableToEntity<Plate>();
            List<Plate> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(DateTime date, int time, Int64 count)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(@TOP_COUNT) "
                            + "ROW_NUMBER() OVER(ORDER BY [ID] ASC) as RN"
                            + ", [DATE] "
                            + ", [TIME] "
                            + ", [CODE] "
                            + ", [NAME] "
                            + ", [QD] "
                            + ", [RATE] "
                            + ", [SPEED] "
                            + ", [CJE] "
                            + ", [ZLJE] "
                            + ", [BUY] "
                            + ", [SELL] "
                            + ", [LB] "
                            + ", [LTSZ] "
                            + ", [QJZF] "
                            + ", [LASTPRICE] "
                            + "FROM "
                            + "[dbo].[TBL_PLATE_LIST] "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND [DATE] = @DATE "
                            + "AND [TIME] = "
                            + "  (SELECT MAX([TIME]) FROM [dbo].[TBL_PLATE_LIST] "
                            + "    WHERE [DATE] = @DATE "
                            + "      AND [TIME] >= @TIME_FROM "
                            + "      AND [TIME] <= @TIME_TO) "
                            + "ORDER BY [ID] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));

                    int time_minus = (time > 0 && time % 100 == 0) ? time - 41 : time - 1;
                    string strTime = (time >= 1000 ? time.ToString() : "0" + time.ToString());
                    string strTimeMinus = (time_minus >= 1000 ? time_minus.ToString() : "0" + time.ToString());

                    cmdSelect.Parameters.Add(new SqlParameter("@TIME_FROM", strTimeMinus + "00")); //前一分钟
                    cmdSelect.Parameters.Add(new SqlParameter("@TIME_TO", strTime + "59"));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Plate数据查询失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }

                return null;
            }
        }
    }
}