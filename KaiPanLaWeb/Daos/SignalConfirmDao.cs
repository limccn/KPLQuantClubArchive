using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class SignalConfirmDao
    {

        public Logger logger = Logger._;
        public List<SignalDetail> QuerySignalDetails(DateTime date, Int64 ttl, Int64 count)
        {
            DataTable dtInDb = QueryDetailDataTableFromDB(date, ttl, count);
            DataTableToEntity<SignalDetail> util = new DataTableToEntity<SignalDetail>();
            List<SignalDetail> result = util.FillModel(dtInDb);

            return result;
        }

        public List<Signal> QuerySignals(DateTime date, Int64 ttl, Int64 count)
        {
            DataTable dtInDb = QueryDataTableFromDB(date, ttl, count);
            DataTableToEntity<Signal> util = new DataTableToEntity<Signal>();
            List<Signal> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(DateTime date, Int64 ttl, Int64 count)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(@TOP_COUNT) "
                            + "ROW_NUMBER() OVER(ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC) as RN"
                            + ", TS.[DATE] "
                            + ", TS.[TIME] "
                            + ", TS.[SBSJ] "
                            + ", TS.[XHQRSJ] "
                            //+ ", TS.[XHGXSJ] "
                            + ", TS.[CODE] "
                            + ", TS.[NAME] "
                            + ", TS.[RATE] "
                            + ", TS.[PRICE] "
                            + ", TS.[CJE] "
                            + ", TS.[RATIO] "
                            + ", TS.[SPEED] "
                            + ", TS.[SJLTP] "
                            + ", TS.[TUDE] "
                            + ", TS.[BUY] "
                            + ", TS.[SELL] "
                            + ", TS.[ZLJE] "
                            //+ ", TS.[QJZF] "
                            //+ ", TS.[TAG] "
                            //+ ", TS.[JCF] "
                            //+ ", TS.[JEF] "
                            //+ ", TS.[JEZH] "
                            //+ ", TS.[JEZH2] "
                            + ", TS.[TL] "
                            + ", TS.[QD] "
                            + ", TL.[RATE] as ZX_RATE  "
                            //+ ", TL.[PRICE] as ZX_PRICE "
                            //+ ", TL.[CJE] as ZX_CJE "
                            //+ ", TL.[RATIO] as ZX_RATIO "
                            //+ ", TL.[SPEED] as ZX_SPEED "
                            //+ ", TL.[SJLTP] as ZX_SJLTP "
                            + ", TL.[BUY] as ZX_BUY "
                            + ", TL.[SELL] as ZX_SELL "
                            + ", TL.[ZLJE] as ZX_ZLJE "
                            //+ ", TL.[QJZF] as ZX_QJZF "
                            //+ ", TL.[JCF] as ZX_JCF "
                            //+ ", TL.[JEF] as ZX_JEF "
                            //+ ", TL.[JEZH] as ZX_JEZH "
                            //+ ", TL.[JEZH2] as ZX_JEZH2 "
                            + ",TL.[TL] as ZX_TL "
                            + ",TL.[QD] as ZX_QD "
                            + "FROM "
                            + "[dbo].[TBL_STOCK_SIGNAL_CONFIRM] as TS "
                            + "LEFT JOIN "
                            + "[dbo].[TBL_STOCK_LATEST_ANALYSE] as TL "
                            + "ON TS.DATE = TL.DATE "
                            + "AND TS.CODE = TL.CODE "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND TS.[DATE] = @DATE "
                            + "AND TS.[XHGXSJ] > @XHGXSJ "
                            + "ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    String time_to_ttl = "093000"; // 全部9:30
                    if (ttl > 0)
                    {
                        time_to_ttl = date.AddSeconds(-ttl).ToString("HHmmss");
                    }
                    cmdSelect.Parameters.Add(new SqlParameter("@XHGXSJ", time_to_ttl));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Signal数据查询失败", ex);
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


        private DataTable QueryDetailDataTableFromDB(DateTime date, Int64 ttl, Int64 count)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(@TOP_COUNT) "
                            + "ROW_NUMBER() OVER(ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC) as RN"
                            + ", TS.[DATE] "
                            + ", TS.[TIME] "
                            + ", TS.[SBSJ] "
                            + ", TS.[XHGXSJ] "
                            + ", TS.[XHQRSJ] "
                            + ", TS.[CODE] "
                            + ", TS.[NAME] "
                            + ", TS.[RATE] "
                            + ", TS.[PRICE] "
                            + ", TS.[CJE] "
                            + ", TS.[RATIO] "
                            + ", TS.[SPEED] "
                            + ", TS.[SJLTP] "
                            + ", TS.[TUDE] "
                            + ", TS.[BUY] "
                            + ", TS.[SELL] "
                            + ", TS.[ZLJE] "
                            + ", TS.[QJZF] "
                            + ", TS.[TAG] "
                            + ", TS.[JCF] "
                            + ", TS.[JEF] "
                            + ", TS.[JEZH] "
                            + ", TS.[JEZH2] "
                            + ", TS.[TL] "
                            + ", TS.[QD] "
                            + ", TL.[RATE] as ZX_RATE  "
                            + ", TL.[PRICE] as ZX_PRICE "
                            + ", TL.[CJE] as ZX_CJE "
                            + ", TL.[RATIO] as ZX_RATIO "
                            + ", TL.[SPEED] as ZX_SPEED "
                            + ", TL.[SJLTP] as ZX_SJLTP "
                            + ", TL.[BUY] as ZX_BUY "
                            + ", TL.[SELL] as ZX_SELL "
                            + ", TL.[ZLJE] as ZX_ZLJE "
                            + ", TL.[QJZF] as ZX_QJZF "
                            + ", TL.[JCF] as ZX_JCF "
                            + " ,TL.[JEF] as ZX_JEF "
                            + ",TL.[JEZH] as ZX_JEZH "
                            + " ,TL.[JEZH2] as ZX_JEZH2 "
                            + ",TL.[TL] as ZX_TL "
                            + ",TL.[QD] as ZX_QD "
                            + "FROM "
                            + "[dbo].[TBL_STOCK_SIGNAL_CONFIRM] as TS "
                            + "LEFT JOIN "
                            + "[dbo].[TBL_STOCK_LATEST_ANALYSE] as TL "
                            + "ON TS.DATE = TL.DATE "
                            + "AND TS.CODE = TL.CODE "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND TS.[DATE] = @DATE "
                            + "AND TS.[XHGXSJ] > @XHGXSJ "
                            + "ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    String time_to_ttl = "093000"; // 全部9:30
                    if (ttl > 0)
                    {
                        time_to_ttl = date.AddSeconds(-ttl).ToString("HHmmss");
                    }
                    cmdSelect.Parameters.Add(new SqlParameter("@XHGXSJ", time_to_ttl));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Signal数据查询失败", ex);
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