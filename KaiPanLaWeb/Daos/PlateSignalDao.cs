using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class PlateSignalDao
    {

        public Logger logger = Logger._;
        public List<PlateSignalDetail> QueryPlateSignalDetails(DateTime date, int time, Int64 count)
        {
            DataTable dtInDb = QueryDetailDataTableFromDB(date, time, count);
            DataTableToEntity<PlateSignalDetail> util = new DataTableToEntity<PlateSignalDetail>();
            List<PlateSignalDetail> result = util.FillModel(dtInDb);

            return result;
        }

        public List<PlateSignal> QueryPlateSignals(DateTime date, int time, Int64 count)
        {
            DataTable dtInDb = QueryDataTableFromDB(date, time, count);
            DataTableToEntity<PlateSignal> util = new DataTableToEntity<PlateSignal>();
            List<PlateSignal> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(DateTime date, int time, Int64 count)
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
                            + ", TS.[PLATEID] "
                            + ", TS.[PLATENAME] "
                            + ", CONVERT(INT,TS.[PLATEQD]) as [PLATEQD] "
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
                            + ", TS.[LBCS] "
                            + ", TS.[LT] "
                            + ", TS.[SPFD] "
                            + ", TS.[ZDFD] "
                            + ", TS.[EJBK] "
                            + ", TS.[LZCS] "
                            + ", TS.[DDJE300W] "
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
                            + "[dbo].[TBL_PLATE_STOCK_SIGNAL] as TS "
                            + "LEFT JOIN "
                            + "[dbo].[TBL_PLATE_STOCK_LATEST_ANALYSE] as TL "
                            + "ON TS.DATE = TL.DATE "
                            + "AND TS.CODE = TL.CODE "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND TS.[DATE] = @DATE "
                            + "AND TS.[SBSJ] > @SBSJ_FROM "
                            + "AND TS.[SBSJ] <= @SBSJ_TO "
                            + "ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    cmdSelect.Parameters.Add(new SqlParameter("@SBSJ_FROM", "093000"));

                    string strTime = (time >= 1000 ? time.ToString() : "0" + time.ToString());
                    cmdSelect.Parameters.Add(new SqlParameter("@SBSJ_TO", strTime + "59"));

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


        private DataTable QueryDetailDataTableFromDB(DateTime date, int time, Int64 count)
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
                            + ", TS.[PLATEID] "
                            + ", TS.[PLATENAME] "
                            + ", CONVERT(INT,TS.[PLATEQD]) as [PLATEQD] "
                            + ", TS.[CODE] "
                            + ", TS.[NAME] "
                            + ", TS.[RATE] "
                            + ", TS.[PRICE] "
                            + ", TS.[CJE] "
                            + ", TS.[RATIO] "
                            + ", TS.[SPEED] "
                            + ", TS.[SJLTP] "
                            + ", TS.[TUDE] "
                            + ", TS.[LBCS] "
                            + ", TS.[LT] "
                            + ", TS.[SPFD] "
                            + ", TS.[ZDFD] "
                            + ", TS.[EJBK] "
                            + ", TS.[LZCS] "
                            + ", TS.[DDJE300W] "
                            + ", TS.[BUY] "
                            + ", TS.[SELL] "
                            + ", TS.[ZLJE] "
                            + ", TS.[QJZF] "
                            //+ ", TS.[TAG] "
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
                            + "[dbo].[TBL_PLATE_STOCK_SIGNAL] as TS "
                            + "LEFT JOIN "
                            + "[dbo].[TBL_PLATE_STOCK_LATEST_ANALYSE] as TL "
                            + "ON TS.DATE = TL.DATE "
                            + "AND TS.CODE = TL.CODE "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND TS.[DATE] = @DATE "
                            + "AND TS.[SBSJ] >= @SBSJ_FROM "
                            + "AND TS.[SBSJ] <= @SBSJ_TO "
                            + "ORDER BY TS.[DATE] DESC, TS.[SBSJ] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));

                    cmdSelect.Parameters.Add(new SqlParameter("@SBSJ_FROM", "093000"));

                    string strTime = (time >= 1000 ? time.ToString() : "0" + time.ToString());
                    cmdSelect.Parameters.Add(new SqlParameter("@SBSJ_TO", strTime + "59"));

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