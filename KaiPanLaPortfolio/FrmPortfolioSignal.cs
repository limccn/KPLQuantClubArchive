using KaiPanLaCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KaiPanLaPortfolio
{
    public partial class FrmPortfolioSignal : Form
    {

        public PortSignalFilterCondition signalFilter1 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 0,
            SJLTP_TO = 15,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 520,
            BUY = 1200
        };
        public PortSignalFilterCondition signalFilter2 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 15,
            SJLTP_TO = 100,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = true,
            ZLJE = 400,
            BUY = 900
        };
        public PortSignalFilterCondition signalFilter3 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 100,
            SJLTP_TO = 150,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 3100,
            BUY = 4500
        };
        public PortSignalFilterCondition signalFilter4 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 150,
            SJLTP_TO = 99999,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 3300,
            BUY = 4900
        };

        public Logger logger = Logger._;

        public static double WAN = 10000.0;
        public static double YI = 100000000.0;

        public bool isRealTimeData = true;

        public DataTable dtPortfolio = null;

        public DateTime updateTime = DateTime.Now;

        public FrmPortfolioSignal()
        {
            InitializeComponent();
        }

        public void setupPortfolioData(DataTable dt, DateTime updateTime)
        {
            if (dt == null)
            {
                return;
            }
            this.dtPortfolio = dt.Copy();
            this.updateTime = updateTime;

            this.InsertOrUpdateDataInDB(dt);

            this.reComputeTable();
        }

        public void reComputeTable()
        {
            if (this.isRealTimeData)
            {
                this.reComputeFromRealtime();
            }
            else
            {
                this.reLoadFromDatabase();
            }
        }

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(0, size.Height / 2 - 100);
            this.Width = size.Width - 20;
            this.Height = size.Height / 2 - 10;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmPortfolioSignalCond frmSignalFilter = new FrmPortfolioSignalCond();
            frmSignalFilter.signalFilter1 = this.signalFilter1;
            frmSignalFilter.signalFilter2 = this.signalFilter2;
            frmSignalFilter.signalFilter3 = this.signalFilter3;
            frmSignalFilter.signalFilter4 = this.signalFilter4;

            frmSignalFilter.ShowDialog();
            if (frmSignalFilter.DialogResult == DialogResult.OK)
            {
                this.signalFilter1 = frmSignalFilter.signalFilter1;
                this.signalFilter2 = frmSignalFilter.signalFilter2;
                this.signalFilter3 = frmSignalFilter.signalFilter3;
                this.signalFilter4 = frmSignalFilter.signalFilter4;
            }
            else if (frmSignalFilter.DialogResult == DialogResult.Cancel)
            {
            }
            frmSignalFilter.Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmComDgvDisSetting frmAnaDisSetting = new FrmComDgvDisSetting();
            frmAnaDisSetting.dgvData = this.dataGridView1;
            frmAnaDisSetting.ShowDialog();
            if (frmAnaDisSetting.DialogResult == DialogResult.OK)
            {

            }
            else if (frmAnaDisSetting.DialogResult == DialogResult.Cancel)
            {
            }
            frmAnaDisSetting.Close();
        }

        private void reComputeFromRealtime()
        {

            //先清空
            DataTableToEntity<PortfolioListItemEntity> util = new DataTableToEntity<PortfolioListItemEntity>();
            List<PortfolioListItemEntity> listClear = new List<PortfolioListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            if (this.dtPortfolio == null)
            {
                return;
            }

            DataTable dtPortfolio = this.dtPortfolio.Copy();


            if (dtPortfolio != null && dtPortfolio.Rows.Count > 0)
            {
                DataTable dtXHQRSJ = this.filterPortfolioWithXHQRSJ(dtPortfolio);
                if (dtXHQRSJ != null && dtXHQRSJ.Rows.Count > 0)
                {
                    this.dataGridView1.AutoGenerateColumns = false;
                    this.dataGridView1.DataSource = dtXHQRSJ;
                    //清除选中
                    this.dataGridView1.ClearSelection();
                }
            }
            else
            {
            }
        }

        private DataTable filterPortfolioWithXHQRSJ(DataTable source)
        {
            DataTableToEntity<PortfolioListItemEntity> util = new DataTableToEntity<PortfolioListItemEntity>();
            List<PortfolioListItemEntity> portList = util.FillModel(source);


            List<PortfolioSignalListItemEntity> sinalList = new List<PortfolioSignalListItemEntity>();
            foreach (PortfolioListItemEntity row in portList)
            {

                var newEntity = new PortfolioSignalListItemEntity();

                //复制属性
                newEntity._Number = row._Number;
                newEntity.Date = row.Date;
                newEntity.Time = row.Time;
                newEntity.Code = row.Code;
                newEntity.Name = row.Name;
                newEntity.Rate = row.Rate;
                newEntity.Price = row.Price;
                newEntity.CJE = row.CJE;
                newEntity.Ratio = row.Ratio;
                newEntity.Speed = row.Speed;
                newEntity.SJLTP = row.SJLTP;
                newEntity.Tude = row.Tude;
                newEntity.Buy = row.Buy;
                newEntity.Sell = row.Sell;
                newEntity.ZLJE = row.ZLJE;
                newEntity.QJZF = row.QJZF;
                newEntity._Date = row._Date;
                newEntity._Time = row._Time;
                newEntity._Rate = row._Rate;
                newEntity._CJE = row._CJE;
                newEntity._Ratio = row._Ratio;
                newEntity._SJLTP = row._SJLTP;
                newEntity._Buy = row._Buy;
                newEntity._Sell = row._Sell;
                newEntity._ZLJE = row._ZLJE;
                newEntity._QJZF = row._QJZF;

                newEntity.XHGXSJ = row._Time;

                if (this.checkTradeSignal(row._SJLTP, row._Buy, row._ZLJE, row.Rate))
                {
                    newEntity.XHQRSJ = row._Time;
                }
                else
                {
                    newEntity.XHQRSJ = "-";
                }

                sinalList.Add(newEntity);
            }

            DataTableToEntity<PortfolioSignalListItemEntity> signalutil = new DataTableToEntity<PortfolioSignalListItemEntity>();
            //重新装载
            DataTable dtEntity = signalutil.FillDataTable(sinalList);


            if (dtEntity != null && dtEntity.Rows.Count > 0)
            {
                return dtEntity;
            }
            return null;
        }

        private void reLoadFromDatabase()
        {
            //先清空
            DataTableToEntity<PortfolioSignalListItemEntity> util = new DataTableToEntity<PortfolioSignalListItemEntity>();

            List<PortfolioSignalListItemEntity> listClear = new List<PortfolioSignalListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;


            DataTable dtDatatable = this.QueryDatatableFromDatabase();

            IEnumerable<DataRow> query = from row in dtDatatable.AsEnumerable()
                                         select row;

            List<PortfolioSignalListItemEntity> list = new List<PortfolioSignalListItemEntity>();
            int index = 0;
            foreach (var row in query)
            {
                var newEntity = new PortfolioSignalListItemEntity
                {
                    _Number = ++index,

                    Date = row.Field<string>("DATE"),
                    _Date = DateTime.ParseExact(row.Field<string>("DATE"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd"),
                    Time = row.Field<string>("TIME"),
                    _Time = DateTime.ParseExact(row.Field<string>("TIME"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    XHGXSJ = DateTime.ParseExact(row.Field<string>("XHGXSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    XHQRSJ = DateTime.ParseExact(row.Field<string>("XHQRSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    Code = row.Field<string>("CODE"),
                    Name = row.Field<string>("NAME"),
                    Tude = row.Field<string>("TUDE"),
                    SJLTP = (double)row.Field<decimal>("SJLTP"),
                    _SJLTP = (double)row.Field<decimal>("SJLTP") / YI,
                    ZLJE = (double)row.Field<decimal>("ZLJE"),
                    _ZLJE = (double)row.Field<decimal>("ZLJE") / WAN,
                    Rate = (double)row.Field<decimal>("RATE"),
                    _Rate = (double)row.Field<decimal>("RATE") / 100,
                    Price = (double)row.Field<decimal>("PRICE"),
                    CJE = (double)row.Field<decimal>("CJE"),
                    _CJE = (double)row.Field<decimal>("CJE") / WAN,
                    Ratio = (double)row.Field<decimal>("RATIO"),
                    _Ratio = (double)row.Field<decimal>("RATIO") / 100,
                    Speed = (double)row.Field<decimal>("SPEED"),
                    Buy = (double)row.Field<decimal>("BUY"),
                    _Buy = (double)row.Field<decimal>("BUY") / WAN,
                    Sell = (double)row.Field<decimal>("SELL"),
                    _Sell = (double)row.Field<decimal>("SELL") / WAN,
                    QJZF = (double)row.Field<decimal>("QJZF"),
                    _QJZF = (double)row.Field<decimal>("QJZF") / 100
                };
                list.Add(newEntity);
            }

            //重新装载
            DataTable dtEntity = util.FillDataTable(list);


            if (dtEntity != null && dtEntity.Rows.Count > 0)
            {
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = dtEntity;
                //清除选中
                this.dataGridView1.ClearSelection();
            }
            else
            {
            }
        }

        private DataTable QueryDatatableFromDatabase()
        {
            FrmMainPort frmMainPort = (FrmMainPort)this.MdiParent;
            if (!frmMainPort.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消读取");
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String tableName = Common.GetPortfolioSignalTableName();
                String selectString = "SELECT * FROM " + tableName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE";
                try
                {
                    logger.Info("查询数据数据开始，表名" + tableName);
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", this.updateTime.ToString("yyyyMMdd")));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    logger.Info("查询数据数据完成，表名" + tableName);
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("查询数据失败", ex);
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

        private void InsertOrUpdateDataInDB(DataTable dtSource)
        {
            FrmMainPort frmMain = (FrmMainPort)this.MdiParent;
            if (!frmMain.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消写入");
                return;
            }

            if (dtSource == null)
            {
                return;
            }

            if (dtSource.Rows.Count == 0)
            {
                return;
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String tableLatestName = Common.GetPortfolioLatestTableName();
                String tableSignalName = Common.GetPortfolioSignalTableName();

                String findExistString = "SELECT COUNT(1) FROM " + tableLatestName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";
                String findConfirmExistString = "SELECT COUNT(1) FROM " + tableSignalName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";

                String insertString = "INSERT INTO " + tableLatestName + "(" +
                    "[DATE], " +
                    "[TIME], " +
                    "[XHGXSJ], " +
                    "[XHQRSJ], " +
                    "[CODE], " +
                    "[NAME], " +
                    "[RATE], " +
                    "[PRICE], " +
                    "[CJE], " +
                    "[RATIO], " +
                    "[SPEED], " +
                    "[SJLTP], " +
                    "[TUDE], " +
                    "[BUY], " +
                    "[SELL], " +
                    "[ZLJE], " +
                    "[QJZF] " +
                    ") VALUES (" +
                    "@DATE, " +
                    "@TIME, " +
                    "@XHGXSJ, " +
                    "@XHQRSJ, " +
                    "@CODE, " +
                    "@NAME, " +
                    "@RATE, " +
                    "@PRICE, " +
                    "@CJE, " +
                    "@RATIO, " +
                    "@SPEED, " +
                    "@SJLTP, " +
                    "@TUDE, " +
                    "@BUY, " +
                    "@SELL, " +
                    "@ZLJE, " +
                    "@QJZF " +
                    ")";

                String insertConfirmString = "INSERT INTO " + tableSignalName + "(" +
                    "[DATE], " +
                    "[TIME], " +
                    "[XHGXSJ], " +
                    "[XHQRSJ], " +
                    "[CODE], " +
                    "[NAME], " +
                    "[RATE], " +
                    "[PRICE], " +
                    "[CJE], " +
                    "[RATIO], " +
                    "[SPEED], " +
                    "[SJLTP], " +
                    "[TUDE], " +
                    "[BUY], " +
                    "[SELL], " +
                    "[ZLJE], " +
                    "[QJZF] " +
                    ") VALUES (" +
                    "@DATE, " +
                    "@TIME, " +
                    "@XHGXSJ, " +
                    "@XHQRSJ, " +
                    "@CODE, " +
                    "@NAME, " +
                    "@RATE, " +
                    "@PRICE, " +
                    "@CJE, " +
                    "@RATIO, " +
                    "@SPEED, " +
                    "@SJLTP, " +
                    "@TUDE, " +
                    "@BUY, " +
                    "@SELL, " +
                    "@ZLJE, " +
                    "@QJZF " +
                    ")";

                //第一次更新
                String updateString1 = "UPDATE " + tableLatestName + " SET " +
                    "[TIME] = @TIME ," +
                    "[XHGXSJ] = @XHGXSJ ," +
                    "[RATE] = @RATE, " +
                    "[PRICE] = @PRICE, " +
                    "[CJE] = @CJE, " +
                    "[RATIO] = @RATIO, " +
                    "[SPEED] = @SPEED, " +
                    "[BUY] = @BUY, " +
                    "[SELL] = @SELL, " +
                    "[ZLJE] = @ZLJE, " +
                    "[QJZF] = @QJZF, " +
                    "[UPDATE_TIME] = @UPDATE_TIME " +
                    "WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";

                //第二次更新
                String updateString2 = "UPDATE " + tableLatestName + " SET " +
                    "[XHQRSJ] = @XHQRSJ " +
                    "WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE AND " +
                    "[XHQRSJ] = '000000'";

                destinationConnection.Open();
                SqlTransaction transaction = destinationConnection.BeginTransaction();
                try
                {

                    foreach (DataRow dr in dtSource.Rows)
                    {
                        SqlCommand cmdSelect = new SqlCommand(findExistString, destinationConnection, transaction);
                        cmdSelect.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                        cmdSelect.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));

                        int count = (int)cmdSelect.ExecuteScalar();

                        bool isTradeSignal = false;

                        if (count == 0)
                        {
                            logger.Info("没有信号数据，插入数据开始，表名：" + tableLatestName);

                            SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection, transaction);
                            cmdInsert.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"])); //信号更新时间
                            cmdInsert.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SJLTP", dr["SJLTP"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@TUDE", dr["TUDE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));

                            isTradeSignal = this.checkTradeSignal(
                                    Double.Parse(dr["SJLTP"].ToString()) / YI,
                                    Double.Parse(dr["BUY"].ToString()) / WAN,
                                    Double.Parse(dr["ZLJE"].ToString()) / WAN,
                                    Double.Parse(dr["RATE"].ToString())
                                    );

                            // 如果是交易信号，直接插入时设置
                            if (isTradeSignal)
                            {
                                cmdInsert.Parameters.Add(new SqlParameter("@XHQRSJ", dr["TIME"])); //信号更新时间
                            }
                            else
                            {
                                cmdInsert.Parameters.Add(new SqlParameter("@XHQRSJ", "000000")); //信号更新时间
                            }

                            //插入
                            logger.Info("新增信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["CODE"] + "," + dr["NAME"]);
                            cmdInsert.ExecuteNonQuery();
                        }
                        else if (count == 1)
                        {
                            logger.Info("存在信号数据，更新数据开始，表名：" + tableLatestName);
                            SqlCommand cmdUpdate = null;

                            cmdUpdate = new SqlCommand(updateString1, destinationConnection, transaction);
                            cmdUpdate.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@UPDATE_TIME", DateTime.Now));

                            logger.Info("更新信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["CODE"] + "," + dr["NAME"]);
                            cmdUpdate.ExecuteNonQuery();

                            isTradeSignal = this.checkTradeSignal(
                                    Double.Parse(dr["SJLTP"].ToString()) / YI,
                                    Double.Parse(dr["BUY"].ToString()) / WAN,
                                    Double.Parse(dr["ZLJE"].ToString()) / WAN,
                                    Double.Parse(dr["RATE"].ToString())
                                    );

                            // 如果满足交易信号，更新信号确认时间
                            if (isTradeSignal)
                            {
                                cmdUpdate = new SqlCommand(updateString2, destinationConnection, transaction);
                                cmdUpdate.Parameters.Add(new SqlParameter("@XHQRSJ", dr["TIME"])); //信号更新时间
                                cmdUpdate.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                                cmdUpdate.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));

                                logger.Info("更新确认交易信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["CODE"] + "," + dr["NAME"]);
                                cmdUpdate.ExecuteNonQuery();
                            }

                        }

                        // 如果是交易信号，则插入相应的信号表
                        if (isTradeSignal)
                        {

                            SqlCommand cmdSelectConfirm = new SqlCommand(findConfirmExistString, destinationConnection, transaction);
                            cmdSelectConfirm.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdSelectConfirm.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));

                            int countConfirm = (int)cmdSelectConfirm.ExecuteScalar();

                            if (countConfirm == 0)
                            {
                                logger.Info("确认有效信号，插入数据开始，表名：" + tableSignalName);

                                SqlCommand cmdInsert = new SqlCommand(insertConfirmString, destinationConnection, transaction);
                                cmdInsert.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"])); //信号更新时间
                                cmdInsert.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SJLTP", dr["SJLTP"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@TUDE", dr["TUDE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));

                                //cmdInsert.Parameters.Add(new SqlParameter("@CREATE_TIME", this.updateTime));
                                //cmdInsert.Parameters.Add(new SqlParameter("@UPDATE_TIME", this.updateTime));

                                cmdInsert.Parameters.Add(new SqlParameter("@XHQRSJ", dr["TIME"])); //信号更新时间
                                //插入
                                logger.Info("新增有效信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["CODE"] + "," + dr["NAME"]);
                                cmdInsert.ExecuteNonQuery();
                            }
                            else
                            {
                                logger.Info("已经存在的交易信号，无需更新");
                            }

                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    logger.Warn("插入或更新数据库数据失败，表名：" + tableLatestName, ex);
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.isRealTimeData)
            {
                this.isRealTimeData = false;
                this.toolStripButton3.Text = "打开实时变动";
                this.colXHQRSJ.Visible = true;

            }
            else
            {
                this.isRealTimeData = true;
                this.toolStripButton3.Text = "关闭实时变动";
                this.colXHQRSJ.Visible = false;
            }

            this.reComputeTable();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView1.ClearSelection();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            if (e.RowIndex < this.dataGridView1.Rows.Count)
            {
                DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];
                try
                {
                    // 信号确认时间不为空
                    if (!dgrSingle.Cells["colXHQRSJ"].Value.ToString().Equals("-"))
                    {
                        dgrSingle.Cells["colNAME"].Style.ForeColor = Color.Red;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("数据转换失败", ex);
                }
            }
        }

        private bool checkTradeSignal(double SJLTP, double BUY, double ZLJE, double RATE)
        {
            //实际流通盘系数，实际流通盘/10亿
            double sjltp_ceff = SJLTP / 10.0;

            if (this.signalFilter1.enabled &&
                SJLTP >= this.signalFilter1.SJLTP_FROM && SJLTP <= this.signalFilter1.SJLTP_TO &&
                RATE >= this.signalFilter1.RATE_FROM && RATE <= this.signalFilter1.RATE_TO)
            {
                if (this.signalFilter1.SJLTP_CEFF)
                {
                    //WK-净额分值 = 400 - （实际流通盘（亿） - 10亿）/1亿 * 1
                    double JEFZ = sjltp_ceff * (this.signalFilter1.ZLJE - (SJLTP - 10));
                    //WK - 主买分值 = 900 - （实际流通盘（亿） -10亿）/ 1亿 * 5
                    double ZMFZ = sjltp_ceff * (this.signalFilter1.BUY - 5 * (SJLTP - 10));
                    if (ZLJE >= JEFZ &&
                        BUY >= ZMFZ)
                    {
                        return true;
                    }
                }
                else
                {
                    if (ZLJE >= this.signalFilter1.ZLJE &&
                        BUY >= this.signalFilter1.BUY)
                    {
                        return true;
                    }
                }

            }
            if (this.signalFilter2.enabled &&
                SJLTP >= this.signalFilter2.SJLTP_FROM && SJLTP <= this.signalFilter2.SJLTP_TO &&
                RATE >= this.signalFilter2.RATE_FROM && RATE <= this.signalFilter2.RATE_TO)
            {
                if (this.signalFilter2.SJLTP_CEFF)
                {
                    ////WK-净额分值 = 400 - （实际流通盘（亿） - 10亿）/1亿 * 1
                    double JEFZ = sjltp_ceff * (this.signalFilter2.ZLJE - (SJLTP - 10));
                    //WK - 主买分值 = 900 - （实际流通盘（亿） -10亿）/ 1亿 * 5
                    double ZMFZ = sjltp_ceff * (this.signalFilter2.BUY - 5 * (SJLTP - 10));
                    if (ZLJE >= JEFZ &&
                        BUY >= ZMFZ)
                    {
                        return true;
                    }
                }
                else
                {
                    if (ZLJE >= this.signalFilter2.ZLJE &&
                        BUY >= this.signalFilter2.BUY)
                    {
                        return true;
                    }
                }
            }

            if (this.signalFilter3.enabled &&
                SJLTP >= this.signalFilter3.SJLTP_FROM && SJLTP <= this.signalFilter3.SJLTP_TO &&
                RATE >= this.signalFilter3.RATE_FROM && RATE <= this.signalFilter3.RATE_TO)
            {
                if (this.signalFilter3.SJLTP_CEFF)
                {
                    //WK-净额分值 = 400 - （实际流通盘（亿） - 10亿）/1亿 * 1
                    double JEFZ = sjltp_ceff * (this.signalFilter3.ZLJE - (SJLTP - 10));
                    //WK - 主买分值 = 900 - （实际流通盘（亿） -10亿）/ 1亿 * 5
                    double ZMFZ = sjltp_ceff * (this.signalFilter3.BUY - 5 * (SJLTP - 10));
                    if (ZLJE >= JEFZ &&
                        BUY >= ZMFZ)
                    {
                        return true;
                    }
                }
                else
                {
                    if (ZLJE >= this.signalFilter3.ZLJE &&
                        BUY >= this.signalFilter3.BUY)
                    {
                        return true;
                    }
                }
            }
            if (this.signalFilter4.enabled &&
                SJLTP >= this.signalFilter4.SJLTP_FROM && SJLTP <= this.signalFilter4.SJLTP_TO &&
                RATE >= this.signalFilter4.RATE_FROM && RATE <= this.signalFilter4.RATE_TO)
            {
                if (this.signalFilter4.SJLTP_CEFF)
                {
                    //WK-净额分值 = 400 - （实际流通盘（亿） - 10亿）/1亿 * 1
                    double JEFZ = sjltp_ceff * (this.signalFilter4.ZLJE - (SJLTP - 10));
                    //WK - 主买分值 = 900 - （实际流通盘（亿） -10亿）/ 1亿 * 5
                    double ZMFZ = sjltp_ceff * (this.signalFilter4.BUY - 5 * (SJLTP - 10));
                    if (ZLJE >= JEFZ &&
                        BUY >= ZMFZ)
                    {
                        return true;
                    }
                }
                else
                {
                    if (ZLJE >= this.signalFilter4.ZLJE &&
                        BUY >= this.signalFilter4.BUY)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
