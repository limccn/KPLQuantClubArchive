using KaiPanLaCommon;
using KaiPanLaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KaiPanLaWeb.Daos
{
    public class SimulateTradeDao
    {
        public Logger logger = Logger._;


        public List<SimulateTradeDetail> QuerySimulateTrade(string appid, string channelid, string type, DateTime date, Int64 count, string trade_type)
        {
            DataTable dtInDb = QueryDataTableFromDB(appid, channelid, type, date, count, trade_type);
            DataTableToEntity<SimulateTradeDetail> util = new DataTableToEntity<SimulateTradeDetail>();
            List<SimulateTradeDetail> result = util.FillModel(dtInDb);

            return result;
        }

        private DataTable QueryDataTableFromDB(string appid, string channelid, string type, DateTime date, Int64 count, string trade_type)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT TOP(@TOP_COUNT) "
                            + "ROW_NUMBER() OVER(ORDER BY TS.[DATE] DESC, TS.[TIME] ASC) as RN"
                            + ", TS.[DATE] "
                            + ", TS.[TIME] "
                            + ", TS.[TIME] as SBSJ"
                            + ", TS.[TIME] as XHQRSJ"
                            + ", TS.[APP] "
                            + ", TS.[CHANNEL] "
                            + ", TS.[TYPE] "
                            + ", TS.[NONCE] "
                            + ", TS.[CODE] "
                            + ", TL.[NAME] "
                            + ", TL.[TUDE] "
                            + ", TS.[SIDE] "
                            + ", TS.[ACTION] "
                            + ", TS.[TRADE_TYPE] "
                            + ", TS.[PRICE] AS ORDER_PRICE "
                            + ", TS.[NUMBER] "
                            + ", TS.[AMOUNT] "
                            + ", TS.[FEE] "
                            + ", TS.[BALANCE] "
                            + ", TL.[RATE] as RATE  "
                            + ", TL.[PRICE] as PRICE "
                            + ", TL.[CJE] as CJE "
                            + ", TL.[RATIO] as RATIO "
                            + ", TL.[SPEED] as SPEED "
                            + ", TL.[SJLTP] as SJLTP "
                            + ", TL.[BUY] as BUY "
                            + ", TL.[SELL] as SELL "
                            + ", TL.[ZLJE] as ZLJE "
                            + ", TL.[ZLJE] as QJZF "
                            + ", TL.[TL] as TL "
                            + ", TL.[QD] as QD "
                            + "FROM "
                            + "[dbo].[TBL_SIMULATE_ORDER] as TS "
                            + "LEFT JOIN "
                            + "[dbo].[TBL_STOCK_SIGNAL] as TL "
                            + "ON TS.DATE = TL.DATE "
                            + "AND TS.CODE = TL.CODE "
                            + "WHERE "
                            + "1 = 1 "
                            + "AND TS.[DATE] = @DATE "
                            + "AND TS.[APP] = @APP "
                            + "AND TS.[CHANNEL] = @CHANNEL "
                            + "AND TS.[TYPE] = @TYPE "
                            + "AND TS.[TRADE_TYPE] = @TRADE_TYPE "
                            + "ORDER BY TS.[DATE] DESC, TS.[TIME] ASC";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", date.ToString("yyyyMMdd")));
                    cmdSelect.Parameters.Add(new SqlParameter("@TOP_COUNT", count));
                    cmdSelect.Parameters.Add(new SqlParameter("@APP", appid));
                    cmdSelect.Parameters.Add(new SqlParameter("@CHANNEL", channelid));
                    cmdSelect.Parameters.Add(new SqlParameter("@TYPE", type));
                    cmdSelect.Parameters.Add(new SqlParameter("@TRADE_TYPE", trade_type));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Simulate数据查询失败", ex);
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

        public int Create(SimulateTradeReq simulate, string appid, string channelid, string type)
        {
            logger.Info("SimulateTrade，" +
                    "DATE:" + simulate.DATE +
                    ",TIME:" + simulate.TIME +
                    ",APP:" + appid +
                    ",CHANNEL:" + channelid +
                    ",TYPE:" + type +
                    ",NONCE:" + simulate.NONCE +
                    ",CODE:" + simulate.CODE +
                    ",NAME:" + simulate.NAME +
                    ",SIDE:" + simulate.SIDE +
                    ",ACTION:" + simulate.ACTION +
                    ",TRADE_TYPE:" + simulate.TRADE_TYPE +
                    ",PRICE:" + simulate.PRICE +
                    ",NUMBER:" + simulate.NUMBER +
                    ",AMOUNT:" + simulate.AMOUNT +
                    ",FEE:" + simulate.FEE +
                    ",BALANCE:" + simulate.BALANCE
                );

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {

                String selectExsitString = "SELECT COUNT(1) FROM [TBL_SIMULATE_ORDER] "
                                    + "WHERE  1=1 "
                                    + "AND [DATE] = @DATE "
                                    + "AND [TIME] = @TIME "
                                    + "AND [CODE] = @CODE "
                                    + "AND [APP] = @APP "
                                    + "AND [CHANNEL] = @CHANNEL "
                                    + "AND [TYPE] = @TYPE "
                                    + "AND [TRADE_TYPE] = @TRADE_TYPE ";

                String insertString = "INSERT INTO [TBL_SIMULATE_ORDER]("
                                    + "[DATE],[TIME],[APP],[CHANNEL],[TYPE], [NONCE],[CODE],[NAME],[SIDE],[ACTION],[TRADE_TYPE],[PRICE],[NUMBER],[AMOUNT],[FEE],[BALANCE],[DATA]"
                                    + ") VALUES ("
                                    + "@DATE,@TIME,@APP,@CHANNEL,@TYPE,@NONCE,@CODE,@NAME,@SIDE,@ACTION,@TRADE_TYPE,@PRICE,@NUMBER,@AMOUNT,@FEE,@BALANCE,@DATA"
                                    + ")";
                try
                {
                    destinationConnection.Open();

                    SqlCommand cmdSelect = new SqlCommand(selectExsitString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", simulate.DATE));
                    cmdSelect.Parameters.Add(new SqlParameter("@TIME", simulate.TIME));
                    cmdSelect.Parameters.Add(new SqlParameter("@CODE", simulate.CODE));
                    cmdSelect.Parameters.Add(new SqlParameter("@APP", appid));
                    cmdSelect.Parameters.Add(new SqlParameter("@CHANNEL", channelid));
                    cmdSelect.Parameters.Add(new SqlParameter("@TYPE", type));
                    cmdSelect.Parameters.Add(new SqlParameter("@TRADE_TYPE", simulate.TRADE_TYPE));
                    int count = (int)cmdSelect.ExecuteScalar();

                    if (count == 0)
                    {

                        SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection);
                        cmdInsert.Parameters.Add(new SqlParameter("@DATE", simulate.DATE == null ? "" : simulate.DATE));
                        cmdInsert.Parameters.Add(new SqlParameter("@TIME", simulate.TIME == null ? "" : simulate.TIME));
                        cmdInsert.Parameters.Add(new SqlParameter("@APP", appid));
                        cmdInsert.Parameters.Add(new SqlParameter("@CHANNEL", channelid));
                        cmdInsert.Parameters.Add(new SqlParameter("@TYPE", type));
                        cmdInsert.Parameters.Add(new SqlParameter("@NONCE", simulate.NONCE));
                        cmdInsert.Parameters.Add(new SqlParameter("@CODE", simulate.CODE == null ? "" : simulate.CODE));
                        cmdInsert.Parameters.Add(new SqlParameter("@NAME", simulate.NAME == null ? "" : simulate.NAME));
                        cmdInsert.Parameters.Add(new SqlParameter("@SIDE", simulate.SIDE == null ? "" : simulate.SIDE));
                        cmdInsert.Parameters.Add(new SqlParameter("@ACTION", simulate.ACTION == null ? "" : simulate.ACTION));
                        cmdInsert.Parameters.Add(new SqlParameter("@TRADE_TYPE", simulate.TRADE_TYPE == null ? "" : simulate.TRADE_TYPE));
                        cmdInsert.Parameters.Add(new SqlParameter("@PRICE", simulate.PRICE));
                        cmdInsert.Parameters.Add(new SqlParameter("@NUMBER", simulate.NUMBER));
                        cmdInsert.Parameters.Add(new SqlParameter("@AMOUNT", simulate.AMOUNT));
                        cmdInsert.Parameters.Add(new SqlParameter("@FEE", simulate.FEE));
                        cmdInsert.Parameters.Add(new SqlParameter("@BALANCE", simulate.BALANCE));
                        cmdInsert.Parameters.Add(new SqlParameter("@DATA", ""));



                        // 执行操作
                        int row = cmdInsert.ExecuteNonQuery();
                        logger.Info("simulateTrade数据插入，UserInfo：" + simulate);
                        return row;
                    }
                    return 0;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("simulateTrade数据插入", ex);
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

    }
}