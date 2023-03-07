using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class PlateStockDao
    {

        public Logger logger = Logger._;


        public List<PlateStock> QueryPlateStocks(string plateId, DateTime date, int time, Int64 count)
        {
            DataTable dtInDb = QueryDataTableFromDB(plateId, date, time, count);
            DataTableToEntity<PlateStock> util = new DataTableToEntity<PlateStock>();
            List<PlateStock> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(string plateId, DateTime date, int time, Int64 count)
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
                            + ", [PLATEID] "
                            + ", [PLATENAME] "
                            + ", [PLATEQD] "
                            + ", [RATE] "
                            + ", [PRICE] "
                            + ", [CJE] "
                            + ", [RATIO] "
                            + ", [SPEED] "
                            + ", [SJLTP] "
                            + ", [TUDE] "
                            + ", [LBCS] "
                            + ", [LT] "
                            + ", [SPFD] "
                            + ", [ZDFD] "
                            + ", [EJBK] "
                            + ", [LZCS] "
                            + ", [DDJE300W] "
                            + ", [BUY] "
                            + ", [SELL] "
                            + ", [ZLJE] "
                            + "FROM "
                            + "[dbo].[TBL_PLATE_STOCK] "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND [DATE] = @DATE "
                            + "AND [PLATEID] = @PLATEID "
                            + "AND [TIME] = "
                            + "  (SELECT MAX([TIME]) FROM [dbo].[TBL_PLATE_STOCK] "
                            + "    WHERE [DATE] = @DATE "
                            + "      AND [PLATEID] = @PLATEID "
                            + "      AND [TIME] >= @TIME_FROM "
                            + "      AND [TIME] <= @TIME_TO) "
                            + "ORDER BY [ID] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    cmdSelect.Parameters.Add(new SqlParameter("@PLATEID", plateId));

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
                    logger.Warn("PlateStock数据查询失败", ex);
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