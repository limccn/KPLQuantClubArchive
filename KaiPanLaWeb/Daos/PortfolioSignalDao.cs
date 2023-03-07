using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class PortfolioSignalDao
    {

        public Logger logger = Logger._;

        public List<PortfolioSignal> QueryPortfolioSignals(DateTime date, Int64 ttl, Int64 count)
        {
            DataTable dtInDb = QueryDataTableFromDB(date, ttl, count);
            DataTableToEntity<PortfolioSignal> util = new DataTableToEntity<PortfolioSignal>();
            List<PortfolioSignal> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(DateTime date, Int64 ttl, Int64 count)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(@TOP_COUNT) "
                            + "ROW_NUMBER() OVER(ORDER BY [DATE] DESC, [XHQRSJ] ASC) as RN"
                            + ", [DATE] "
                            + ", [TIME] "
                            + ", [XHQRSJ] "
                            + ", [XHGXSJ] "
                            + ", [CODE] "
                            + ", [NAME] "
                            + ", [RATE] "
                            + ", [PRICE] "
                            + ", [CJE] "
                            + ", [RATIO] "
                            + ", [SPEED] "
                            + ", [SJLTP] "
                            + ", [TUDE] "
                            + ", [BUY] "
                            + ", [SELL] "
                            + ", [ZLJE] "
                            + ", [QJZF] "
                            + ", [TAG] "
                            + "FROM "
                            + "[dbo].[TBL_PORTFOLIO_SIGNAL] "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND [DATE] = @DATE "
                            //+ "AND [XHGXSJ] > @XHGXSJ "
                            + "AND [XHQRSJ] > '000000' "
                            + "ORDER BY [DATE] DESC, [XHQRSJ] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    //String time_to_ttl = "093000"; // 全部9:30
                    //if (ttl > 0)
                    //{
                    //    time_to_ttl = date.AddSeconds(-ttl).ToString("HHmmss");
                    //}
                    //cmdSelect.Parameters.Add(new SqlParameter("@XHGXSJ", time_to_ttl));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("PortfolioSiganl数据查询失败", ex);
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