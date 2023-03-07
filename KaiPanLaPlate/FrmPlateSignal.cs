using KaiPanLaCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KaiPanLaPlate
{
    public partial class FrmPlateSignal : Form
    {

        //private int scrollIndex;

        public Logger logger = Logger._;

        public static double WAN = 10000.0;
        public static double YI = 100000000.0;

        public int signalTTL { get; set; } = 0;
        public bool markSignal { get; set; } = false;

        public string currentPlate { get; set; } = "";

        public DataTable dtAnalyse { get; set; } = null;
        public DataTable dtFilterdAnalyse { get; set; } = null;

        public DataTable dtLatest { get; set; } = null;

        public DateTime updateTime = DateTime.Now;

        public FrmPlateSignal()
        {
            InitializeComponent();
        }

        private void FrmPlateSignal_Load(object sender, EventArgs e)
        {
            this.performSizeChanged(this.MdiParent.Size);
            this.initControls();
        }

        private void initControls()
        {
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = SystemInformation.HighContrast ? SystemColors.Highlight : Color.MistyRose;
            this.dataGridView1.DefaultCellStyle.SelectionForeColor = SystemInformation.HighContrast ? SystemColors.HighlightText : SystemColors.ControlText;

        }

        public void setupPlateStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (dt == null)
            {
                return;
            }
            this.dtAnalyse = dt.Copy();
            this.updateTime = updateTime;

            //this.UpdateDataIfDataIsInDb();

            this.reComputeTable();
        }

        public void setupFilterdPlateStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (dt == null)
            {
                return;
            }
            this.dtFilterdAnalyse = dt.Copy();
            this.updateTime = updateTime;

            this.CatchSignalWhenDataUpdated();
            this.reComputeTable();
        }

        public void setupPlateListSelectChanged(string code, DateTime updateTime)
        {
            if (!code.Equals(this.currentPlate))
            {
                this.currentPlate = code;

                this.dataGridView1.DefaultCellStyle.SelectionBackColor = SystemInformation.HighContrast ? SystemColors.Highlight : Color.MistyRose;
                this.dataGridView1.DefaultCellStyle.SelectionForeColor = SystemInformation.HighContrast ? SystemColors.HighlightText : SystemColors.ControlText;

                this.recoverSelectedRows();
                this.dataGridView1.Refresh();

            }
        }

        public void reComputeTable()
        {
            this.reComputeWhenDataUpdated();
        }

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(0, 290);
            //this.Width = size.Width - 420;
            //int wp = (int)(0.45 * size.Width);

            this.Width = 885;
            this.Height = size.Height - 290 - 115;
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(this.toolStripTextBox1.Text, out value))
            {
                if (value >= 0 && value <= 3600)
                {
                    this.toolStripTextBox1.Text = value.ToString();
                    this.signalTTL = value;

                    this.reComputeTable();
                }
                else
                {
                    MessageBox.Show("请输入0~3600范围内的数字");
                }
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.reComputeTable();
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

        private void reComputeWhenDataUpdated()
        {

            //先清空
            DataTableToEntity<PlateStockSignalListItemEntity> util = new DataTableToEntity<PlateStockSignalListItemEntity>();
            List<PlateStockSignalListItemEntity> listClear = new List<PlateStockSignalListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;


            if (this.dtAnalyse == null)
            {
                return;
            }

            DataTable dtAnalyse = this.dtAnalyse.Copy();
            DataTable dtInDb = this.QueryDataTableFromDB();

            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;

            if (dtInDb != null && dtInDb.Rows.Count > 0)
            {
                if (dtAnalyse != null && dtAnalyse.Rows.Count > 0)
                {
                    List<PlateZXStockSignalListItemEntity> list = this.JoinTableFromDB(dtInDb, dtAnalyse);

                    DataTableToEntity<PlateZXStockSignalListItemEntity> util2 = new DataTableToEntity<PlateZXStockSignalListItemEntity>();
                    DataTable dtJoint = util2.FillDataTable(list);


                    this.dtLatest = dtJoint;

                    this.dataGridView1.AutoGenerateColumns = false;
                    this.dataGridView1.DataSource = dtJoint;



                    //if (dataGridView1.Rows.Count > 0)
                    //{
                    //this.dataGridView1.FirstDisplayedScrollingRowIndex = this.scrollIndex;
                    //}

                    //清除选中
                    //this.dataGridView1.ClearSelection();
                    if (dtJoint.Rows.Count > 0)
                    {
                        FrmMainPlate.handleJointSignalData(dtJoint, this.updateTime);
                    }

                    List<string> stockList = new List<string>();
                    foreach (PlateZXStockSignalListItemEntity item in list)
                    {
                        stockList.Add(item.Code);
                    }
                    if (stockList.Count > 0)
                    {
                        FrmMainPlate.handleJointSignalDataForStockList(stockList, this.updateTime);
                    }
                }
                else
                {
                }
            }
            else
            {
                // 数据库里面没有数据
                //this.dataGridView1.DataSource = null;
            }
        }

        private void UpdateDataIfDataIsInDb()
        {
            if (this.dtAnalyse == null)
            {
                return;
            }
            DataTable dtAnalyse = this.dtAnalyse.Copy();
            this.UpdateIfDataExsitToDB(dtAnalyse);
        }

        private void CatchSignalWhenDataUpdated()
        {
            if (this.dtFilterdAnalyse == null)
            {
                return;
            }
            DataTable dtFilterdAnalyse = this.dtFilterdAnalyse.Copy();
            this.InsertIfDataNotExsitToDB(dtFilterdAnalyse);
        }

        private List<PlateZXStockSignalListItemEntity> JoinTableFromDB(DataTable dtInDb, DataTable dtLatest)
        {
            //return dtInDb;

            int index = 0;

            var query1 = from dt1 in dtInDb.AsEnumerable()
                         join dt2 in dtLatest.AsEnumerable()
                         on new { DATE = dt1.Field<string>("DATE"), CODE = dt1.Field<string>("CODE") }
                         equals new { DATE = dt2.Field<string>("Date"), CODE = dt2.Field<string>("Code") }
                         into abjoin
                         from x in abjoin.DefaultIfEmpty() //orderby dt1.Field<string>("TIME") descending
                         select new PlateZXStockSignalListItemEntity
                         {
                             _Number = ++index,
                             Date = dt1.Field<string>("DATE"),
                             _Date = x == null ? "" : x.Field<string>("_Date"),
                             //_Date = DateTime.ParseExact(dt1.Field<string>("DATE"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd"),
                             Time = dt1.Field<string>("TIME"),
                             SBSJ = DateTime.ParseExact(dt1.Field<string>("SBSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                             XHGXSJ = DateTime.ParseExact(dt1.Field<string>("XHGXSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                             XHQRSJ = DateTime.ParseExact(dt1.Field<string>("XHQRSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                             //SBSJ = dt1.Field<DateTime>("CREATE_TIME").ToString("HH:mm:ss"),
                             _Time = x == null ? "" : x.Field<string>("_Time"),
                             //_Time = DateTime.ParseExact(dt1.Field<string>("TIME"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm:ss"),
                             PlateID = dt1.Field<string>("PLATEID"),
                             PlateName = dt1.Field<string>("PLATENAME"),
                             PlateQD = dt1.Field<object>("PLATEQD") == null ? 0 : (double)dt1.Field<decimal>("PLATEQD"),


                             Code = dt1.Field<string>("CODE"),
                             Name = dt1.Field<string>("NAME"),
                             SJLTP = (double)dt1.Field<decimal>("SJLTP"),
                             _SJLTP = (double)dt1.Field<decimal>("SJLTP") / YI,
                             Tude = dt1.Field<string>("TUDE"),

                             LBCS = dt1.Field<string>("LBCS"),
                             LT = dt1.Field<string>("LT"),
                             EJBK = dt1.Field<string>("EJBK"),

                             SPFD = (double)dt1.Field<decimal>("SPFD"),
                             _SPFD = (double)dt1.Field<decimal>("SPFD") / WAN,
                             ZDFD = (double)dt1.Field<decimal>("ZDFD"),
                             _ZDFD = (double)dt1.Field<decimal>("ZDFD") / WAN,
                             LZCS = (double)dt1.Field<decimal>("LZCS"),
                             DDJE300W = (double)dt1.Field<decimal>("DDJE300W"),
                             _DDJE300W = (double)dt1.Field<decimal>("DDJE300W") / WAN,

                             ZLJE = (double)dt1.Field<decimal>("ZLJE"),
                             _ZLJE = (double)dt1.Field<decimal>("ZLJE") / WAN,
                             _ZLJE_Format = ((double)dt1.Field<decimal>("ZLJE") >= YI || (double)dt1.Field<decimal>("ZLJE") <= -YI) ? ((double)dt1.Field<decimal>("ZLJE") / YI).ToString("0.00") + "亿" : ((double)dt1.Field<decimal>("ZLJE") / WAN).ToString("0") + "万",
                             Rate = (double)dt1.Field<decimal>("RATE"),
                             _Rate = (double)dt1.Field<decimal>("RATE") / 100,
                             Price = (double)dt1.Field<decimal>("PRICE"),
                             CJE = (double)dt1.Field<decimal>("CJE"),
                             _CJE = (double)dt1.Field<decimal>("CJE") / WAN,
                             Ratio = (double)dt1.Field<decimal>("RATIO"),
                             _Ratio = (double)dt1.Field<decimal>("RATIO") / 100,
                             Speed = (double)dt1.Field<decimal>("SPEED"),
                             Buy = (double)dt1.Field<decimal>("BUY"),
                             _Buy = (double)dt1.Field<decimal>("BUY") / WAN,
                             _Buy_Format = ((double)dt1.Field<decimal>("BUY") >= YI) ? ((double)dt1.Field<decimal>("BUY") / YI).ToString("0.00") + "亿" : ((double)dt1.Field<decimal>("BUY") / WAN).ToString("0") + "万",
                             Sell = (double)dt1.Field<decimal>("SELL"),
                             _Sell = (double)dt1.Field<decimal>("SELL") / WAN,
                             _Sell_Format = ((double)dt1.Field<decimal>("SELL") <= -YI) ? ((double)dt1.Field<decimal>("SELL") / YI).ToString("0.00") + "亿" : ((double)dt1.Field<decimal>("SELL") / WAN).ToString("0") + "万",
                             QJZF = (double)dt1.Field<decimal>("QJZF"),
                             _QJZF = (double)dt1.Field<decimal>("QJZF") / 100,

                             //Tag = dt1.Field<string>("TAG"),
                             JCF = (double)dt1.Field<decimal>("JCF"),
                             JEF = (double)dt1.Field<decimal>("JEF"),
                             TL = (double)dt1.Field<decimal>("TL"),
                             JEZH = (double)dt1.Field<decimal>("JEZH"),
                             JEZH2 = (double)dt1.Field<decimal>("JEZH2"),
                             QD = (double)dt1.Field<decimal>("QD"),

                             ZX_SJLTP = x == null ? 0 : x.Field<double>("_SJLTP"),
                             ZX_ZLJE = x == null ? 0 : x.Field<double>("_ZLJE"),
                             _ZX_ZLJE_Format = ((x == null ? 0 : x.Field<double>("_ZLJE")) >= WAN || (x == null ? 0 : x.Field<double>("_ZLJE")) <= -WAN) ? ((x == null ? 0 : x.Field<double>("_ZLJE")) / WAN).ToString("0.00") + "亿" : ((x == null ? 0 : x.Field<double>("_ZLJE"))).ToString("0") + "万",


                             ZX_Rate = x == null ? 0 : x.Field<double>("_Rate"),

                             ZX_Price = x == null ? 0 : x.Field<double>("Price"),

                             ZX_CJE = x == null ? 0 : x.Field<double>("_CJE"),
                             ZX_Ratio = x == null ? 0 : x.Field<double>("_Ratio"),

                             ZX_Speed = x == null ? 0 : x.Field<double>("Speed"),

                             ZX_Buy = x == null ? 0 : x.Field<double>("_Buy"),
                             _ZX_Buy_Format = ((x == null ? 0 : x.Field<double>("_Buy")) >= WAN) ? ((x == null ? 0 : x.Field<double>("_Buy")) / WAN).ToString("0.00") + "亿" : ((x == null ? 0 : x.Field<double>("_Buy"))).ToString("0") + "万",

                             ZX_Sell = x == null ? 0 : x.Field<double>("_Sell"),
                             _ZX_Sell_Format = ((x == null ? 0 : x.Field<double>("_Sell")) <= -WAN) ? ((x == null ? 0 : x.Field<double>("_Sell")) / WAN).ToString("0.00") + "亿" : ((x == null ? 0 : x.Field<double>("_Sell"))).ToString("0") + "万",


                             ZX_QJZF = x == null ? 0 : x.Field<double>("_QJZF"),

                             ZX_JCF = x == null ? 0 : x.Field<double>("JCF"),
                             ZX_JEF = x == null ? 0 : x.Field<double>("JEF"),
                             ZX_TL = x == null ? 0 : x.Field<double>("TL"),
                             ZX_JEZH = x == null ? 0 : x.Field<double>("JEZH"),
                             ZX_JEZH2 = x == null ? 0 : x.Field<double>("JEZH2"),
                             ZX_QD = x == null ? 0 : x.Field<double>("QD")
                         };

            List<PlateZXStockSignalListItemEntity> list = query1.ToList();
            return list;
        }
        private DataTable QueryDataTableFromDB()
        {
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;
            if (!FrmMainPlate.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消读取");
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String tableName = Common.GetAppSettingByKey("PlateStockSignalTableName");
                String selectString = "SELECT top 33 * FROM " + tableName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[XHGXSJ] > @XHGXSJ ";
                //String selectString = "SELECT * FROM " + tableName + " WHERE 1 = 1";
                try
                {
                    logger.Info("查询数据数据开始，表名" + tableName);
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    cmdSelect.Parameters.Add(new SqlParameter("@DATE", this.updateTime.ToString("yyyyMMdd")));
                    String time_to_ttl = "093000"; // 全部9:30
                    if (this.signalTTL > 0)
                    {
                        time_to_ttl = this.updateTime.AddSeconds(-this.signalTTL).ToString("HHmmss");
                    }
                    cmdSelect.Parameters.Add(new SqlParameter("@XHGXSJ", time_to_ttl));


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

        private void InsertIfDataNotExsitToDB(DataTable dtSource)
        {
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;
            if (!FrmMainPlate.isDatabaseOK)
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
                String tableName = Common.GetAppSettingByKey("PlateStockSignalTableName");
                String tableConfirmName = Common.GetAppSettingByKey("PlateStockSignalConfirmTableName");

                String findExistString = "SELECT COUNT(1) FROM " + tableName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";
                String findConfirmExistString = "SELECT COUNT(1) FROM " + tableConfirmName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";
                String insertString = "INSERT INTO " + tableName + "(" +
                    "[DATE], " +
                    "[TIME], " +
                    "[SBSJ], " +
                    "[XHGXSJ], " +
                    "[XHQRSJ], " +
                    "[PLATEID], " +
                    "[PLATENAME], " +
                    "[PLATEQD], " +
                    "[CODE], " +
                    "[NAME], " +
                    "[RATE], " +
                    "[PRICE], " +
                    "[CJE], " +
                    "[RATIO], " +
                    "[SPEED], " +
                    "[SJLTP], " +
                    "[TUDE], " +
                    "[LBCS], " +
                    "[LT], " +
                    "[SPFD], " +
                    "[ZDFD], " +
                    "[EJBK], " +
                    "[LZCS], " +
                    "[DDJE300W], " +
                    "[BUY], " +
                    "[SELL], " +
                    "[ZLJE], " +
                    "[QJZF], " +
                    //"[TAG], " +
                    "[JCF], " +
                    "[JEF], " +
                    "[JEZH], " +
                    "[JEZH2], " +
                    "[TL], " +
                    "[QD]" +
                    ") VALUES (" +
                    "@DATE, " +
                    "@TIME, " +
                    "@SBSJ, " +
                    "@XHGXSJ, " +
                    "@XHQRSJ, " +
                    "@PLATEID, " +
                    "@PLATENAME, " +
                    "@PLATEQD, " +
                    "@CODE, " +
                    "@NAME, " +
                    "@RATE, " +
                    "@PRICE, " +
                    "@CJE, " +
                    "@RATIO, " +
                    "@SPEED, " +
                    "@SJLTP, " +
                    "@TUDE, " +
                    "@LBCS, " +
                    "@LT, " +
                    "@SPFD, " +
                    "@ZDFD, " +
                    "@EJBK, " +
                    "@LZCS, " +
                    "@DDJE300W, " +
                    "@BUY, " +
                    "@SELL, " +
                    "@ZLJE, " +
                    "@QJZF, " +
                    //"@TAG, " +
                    "@JCF, " +
                    "@JEF, " +
                    "@JEZH, " +
                    "@JEZH2, " +
                    "@TL, " +
                    "@QD" +
                    //"null,"+ //Create Time
                    //"null" + // Update Time
                    ")";

                String insertConfirmString = "INSERT INTO " + tableConfirmName + "(" +
                    "[DATE], " +
                    "[TIME], " +
                    "[SBSJ], " +
                    "[XHGXSJ], " +
                    "[XHQRSJ], " +
                    "[PLATEID], " +
                    "[PLATENAME], " +
                    "[PLATEQD], " +
                    "[CODE], " +
                    "[NAME], " +
                    "[RATE], " +
                    "[PRICE], " +
                    "[CJE], " +
                    "[RATIO], " +
                    "[SPEED], " +
                    "[SJLTP], " +
                    "[TUDE], " +
                    "[LBCS], " +
                    "[LT], " +
                    "[SPFD], " +
                    "[ZDFD], " +
                    "[EJBK], " +
                    "[LZCS], " +
                    "[DDJE300W], " +
                    "[BUY], " +
                    "[SELL], " +
                    "[ZLJE], " +
                    "[QJZF], " +
                    //"[TAG], " +
                    "[JCF], " +
                    "[JEF], " +
                    "[JEZH], " +
                    "[JEZH2], " +
                    "[TL], " +
                    "[QD]" +
                    ") VALUES (" +
                    "@DATE, " +
                    "@TIME, " +
                    "@SBSJ, " +
                    "@XHGXSJ, " +
                    "@XHQRSJ, " +
                    "@PLATEID, " +
                    "@PLATENAME, " +
                    "@PLATEQD, " +
                    "@CODE, " +
                    "@NAME, " +
                    "@RATE, " +
                    "@PRICE, " +
                    "@CJE, " +
                    "@RATIO, " +
                    "@SPEED, " +
                    "@SJLTP, " +
                    "@TUDE, " +
                    "@LBCS, " +
                    "@LT, " +
                    "@SPFD, " +
                    "@ZDFD, " +
                    "@EJBK, " +
                    "@LZCS, " +
                    "@DDJE300W, " +
                    "@BUY, " +
                    "@SELL, " +
                    "@ZLJE, " +
                    "@QJZF, " +
                    //"@TAG, " +
                    "@JCF, " +
                    "@JEF, " +
                    "@JEZH, " +
                    "@JEZH2, " +
                    "@TL, " +
                    "@QD" +
                    //"null,"+ //Create Time
                    //"null" + // Update Time
                    ")";

                //第一次更新
                String updateString1 = "UPDATE " + tableName + " SET " +
                    "[TIME] = @TIME ," +
                    "[XHGXSJ] = @XHGXSJ ," +
                    "[UPDATE_TIME] = @UPDATE_TIME " +
                    "WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";

                //第二次更新
                String updateString2 = "UPDATE " + tableName + " SET " +
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
                            logger.Info("没有信号数据，插入数据开始，表名：" + tableName);

                            SqlCommand cmdInsert = new SqlCommand(insertString, destinationConnection, transaction);
                            cmdInsert.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SBSJ", dr["TIME"])); //上榜时间
                            cmdInsert.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"])); //信号更新时间
                            cmdInsert.Parameters.Add(new SqlParameter("@PLATEID", dr["PLATEID"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@PLATENAME", dr["PLATENAME"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@PLATEQD", dr["PLATEQD"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SJLTP", dr["SJLTP"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@TUDE", dr["TUDE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@LBCS", dr["LBCS"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@LT", dr["LT"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SPFD", dr["SPFD"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@ZDFD", dr["ZDFD"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@EJBK", dr["EJBK"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@LZCS", dr["LZCS"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@DDJE300W", dr["DDJE300W"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@TAG", dr["TAG"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@JCF", dr["JCF"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@JEF", dr["JEF"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@JEZH", dr["JEZH"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@JEZH2", dr["JEZH2"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@TL", dr["TL"]));
                            cmdInsert.Parameters.Add(new SqlParameter("@QD", dr["QD"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@CREATE_TIME", this.updateTime));
                            //cmdInsert.Parameters.Add(new SqlParameter("@UPDATE_TIME", this.updateTime));

                            isTradeSignal = this.checkTradeSignal(dr["QD"].ToString(),
                                    dr["TL"].ToString(),
                                    dr["BUY"].ToString(),
                                    dr["ZLJE"].ToString(),
                                    (Int64)WAN);

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
                            logger.Info("新增信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["PLATEID"] + "," + dr["PLATENAME"] + "," + dr["CODE"] + "," + dr["NAME"]);
                            cmdInsert.ExecuteNonQuery();
                        }
                        else if (count == 1)
                        {
                            logger.Info("存在信号数据，更新数据开始，表名：" + tableName);
                            SqlCommand cmdUpdate = null;

                            cmdUpdate = new SqlCommand(updateString1, destinationConnection, transaction);
                            cmdUpdate.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@UPDATE_TIME", DateTime.Now));

                            logger.Info("更新信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["PLATEID"] + "," + dr["PLATENAME"] + "," + dr["CODE"] + "," + dr["NAME"]);
                            cmdUpdate.ExecuteNonQuery();

                            isTradeSignal = this.checkTradeSignal(dr["QD"].ToString(),
                                    dr["TL"].ToString(),
                                    dr["BUY"].ToString(),
                                    dr["ZLJE"].ToString(),
                                    (Int64)WAN);

                            // 如果满足交易信号，更新信号确认时间
                            if (isTradeSignal)
                            {
                                cmdUpdate = new SqlCommand(updateString2, destinationConnection, transaction);
                                cmdUpdate.Parameters.Add(new SqlParameter("@XHQRSJ", dr["TIME"])); //信号更新时间
                                cmdUpdate.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                                cmdUpdate.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));

                                logger.Info("更新确认交易信号数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["PLATEID"] + "," + dr["PLATENAME"] + "," + dr["CODE"] + "," + dr["NAME"]);
                                cmdUpdate.ExecuteNonQuery();
                            }

                        }

                        // 如果是交易信号，则插入相应的信号表
                        if (isTradeSignal && Common.getShouldWatchSignalRealtimeUpdateTable())
                        {

                            SqlCommand cmdSelectConfirm = new SqlCommand(findConfirmExistString, destinationConnection, transaction);
                            cmdSelectConfirm.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdSelectConfirm.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));

                            int countConfirm = (int)cmdSelectConfirm.ExecuteScalar();

                            if (countConfirm == 0)
                            {
                                logger.Info("确认有效信号，插入数据开始，表名：" + tableConfirmName);

                                SqlCommand cmdInsert = new SqlCommand(insertConfirmString, destinationConnection, transaction);
                                cmdInsert.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SBSJ", dr["TIME"])); //上榜时间
                                cmdInsert.Parameters.Add(new SqlParameter("@XHGXSJ", dr["TIME"])); //信号更新时间
                                cmdInsert.Parameters.Add(new SqlParameter("@PLATEID", dr["PLATEID"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@PLATENAME", dr["PLATENAME"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@PLATEQD", dr["PLATEQD"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SJLTP", dr["SJLTP"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@TUDE", dr["TUDE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@LBCS", dr["LBCS"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@LT", dr["LT"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SPFD", dr["SPFD"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@ZDFD", dr["ZDFD"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@EJBK", dr["EJBK"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@LZCS", dr["LZCS"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@DDJE300W", dr["DDJE300W"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));
                                //cmdInsert.Parameters.Add(new SqlParameter("@TAG", dr["TAG"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@JCF", dr["JCF"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@JEF", dr["JEF"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@JEZH", dr["JEZH"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@JEZH2", dr["JEZH2"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@TL", dr["TL"]));
                                cmdInsert.Parameters.Add(new SqlParameter("@QD", dr["QD"]));
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
                    logger.Warn("插入或更新数据库数据失败，表名：" + tableName, ex);
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

        private void UpdateIfDataExsitToDB(DataTable dtSource)
        {
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;
            if (!FrmMainPlate.isDatabaseOK)
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
                String tableName = Common.GetAppSettingByKey("PlateStockSignalTableName");
                String findExistString = "SELECT COUNT(1) FROM " + tableName + " WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";
                String updateString = "UPDATE " + tableName + " SET " +
                    "[TIME] = @TIME ," +
                    "[RATE] = @RATE ," +
                    "[PRICE] = @PRICE ," +
                    "[CJE] = @CJE ," +
                    "[RATIO] = @RATIO ," +
                    "[SPEED] = @SPEED ," +
                    "[SJLTP] = @SJLTP ," +
                    //"[TUDE] = @TUDE ," +
                    "[LBCS] = @LBCS ," +
                    "[LT] = @LT , " +
                    "[SPFD] = @SPFD, " +
                    "[ZDFD] = @ZDFD, " +
                    "[EJBK] = @EJBK, " +
                    "[LZCS] = @LZCS, " +
                    "[DDJE300W] = @DDJE300W, " +
                    "[BUY] = @BUY ," +
                    "[SELL] = @SELL ," +
                    "[ZLJE] = @ZLJE ," +
                    "[QJZF] = @QJZF ," +
                    //"[TAG] = @TAG ," +
                    "[JCF] = @JCF ," +
                    "[JEF] = @JEF ," +
                    "[JEZH] = @JEZH ," +
                    "[JEZH2] = @JEZH2 ," +
                    "[TL] = @TL ," +
                    "[QD] = @QD ," +
                    "[UPDATE_TIME] = @UPDATE_TIME " +
                    "WHERE 1 = 1 AND " +
                    "[DATE] = @DATE AND " +
                    "[CODE] = @CODE";

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

                        if (count == 1)
                        {
                            logger.Info("存在信号数据，更新数据开始，表名：" + tableName);
                            SqlCommand cmdUpdate = new SqlCommand(updateString, destinationConnection, transaction);
                            cmdUpdate.Parameters.Add(new SqlParameter("@DATE", dr["DATE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@TIME", dr["TIME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@CODE", dr["CODE"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@NAME", dr["NAME"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@RATE", dr["RATE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@PRICE", dr["PRICE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@CJE", dr["CJE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@RATIO", dr["RATIO"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SPEED", dr["SPEED"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SJLTP", dr["SJLTP"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@TUDE", dr["TUDE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@LBCS", dr["LBCS"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@LT", dr["LT"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SPFD", dr["SPFD"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@ZDFD", dr["ZDFD"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@EJBK", dr["EJBK"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@LZCS", dr["LZCS"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@DDJE300W", dr["DDJE300W"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@BUY", dr["BUY"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@SELL", dr["SELL"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@ZLJE", dr["ZLJE"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@QJZF", dr["QJZF"]));
                            //cmdInsert.Parameters.Add(new SqlParameter("@TAG", dr["TAG"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@JCF", dr["JCF"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@JEF", dr["JEF"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@JEZH", dr["JEZH"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@JEZH2", dr["JEZH2"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@TL", dr["TL"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@QD", dr["QD"]));
                            cmdUpdate.Parameters.Add(new SqlParameter("@UPDATE_TIME", DateTime.Now));

                            logger.Info("更新已有数据：" + dr["DATE"] + " " + dr["TIME"] + " " + dr["CODE"] + "," + dr["NAME"]);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    logger.Warn("更新数据库数据失败，表名：" + tableName, ex);
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

        private bool checkTradeSignal(string strQD, string strTL, string strZLJE, string strBUY, Int64 factor = 1)
        {

            Int64 QD = (Int64)Double.Parse(strQD);
            Int64 TL = (Int64)Double.Parse(strTL);
            Int64 ZLJE = (Int64)Double.Parse(strZLJE) / factor;
            Int64 BUY = (Int64)Double.Parse(strBUY) / factor;

            FrmMainPlate frmMainPlate = (FrmMainPlate)this.MdiParent;
            SignalFilterCondition f1 = frmMainPlate.frmPlateQuant.signalFilter1;
            SignalFilterCondition f2 = frmMainPlate.frmPlateQuant.signalFilter2;
            SignalFilterCondition f3 = frmMainPlate.frmPlateQuant.signalFilter3;
            SignalFilterCondition f4 = frmMainPlate.frmPlateQuant.signalFilter4;


            if (f1.enabled && (QD >= f1.QD) && (TL >= f1.TL) && (ZLJE >= f1.ZLJE) && (BUY >= f1.BUY))
            {
                return true;
            }
            if (f2.enabled && (QD >= f2.QD) && (TL >= f2.TL) && (ZLJE >= f2.ZLJE) && (BUY >= f2.BUY))
            {
                return true;
            }
            if (f3.enabled && (QD >= f3.QD) && (TL >= f3.TL) && (ZLJE >= f3.ZLJE) && (BUY >= f3.BUY))
            {
                return true;
            }
            if (f4.enabled && (QD >= f4.QD) && (TL >= f4.TL) && (ZLJE >= f4.ZLJE) && (BUY >= f4.BUY))
            {
                return true;
            }

            return false;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.markSignal = !this.markSignal;
            if (this.markSignal)
            {
                this.toolStripButton3.Text = "标记信号:已开启";
            }
            else
            {
                this.toolStripButton3.Text = "标记信号:已关闭";
            }

            //重新计算
            this.reComputeTable();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            //关闭选中，清除已选择的行
            //this.dataGridView1.ClearSelection();
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {

            //if (this.dataGridView1.FirstDisplayedScrollingRowIndex >= 0)
            //{
            //    this.scrollIndex = this.dataGridView1.FirstDisplayedScrollingRowIndex;
            //}
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {


        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (!Common.getShouldFriendlyRowDataColor())
                {
                    return;
                }

                if (e.RowIndex != -1)
                {

                    if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colName"))
                    {
                        DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];
                        //如果不标记信号，返回
                        if (this.markSignal)
                        {
                            ////符合信号个股变色
                            bool isTradeSignal = this.checkTradeSignal(
                               dgrSingle.Cells["colQD"].Value.ToString(),
                                dgrSingle.Cells["colTL"].Value.ToString(),
                                dgrSingle.Cells["colZLJE"].Value.ToString(),
                                dgrSingle.Cells["colBuy"].Value.ToString(),
                                1);

                            if (isTradeSignal)
                            {
                                e.CellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                e.CellStyle.ForeColor = SystemColors.ControlText;
                            }
                        }
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZLJEFormat"))
                    {
                        double ZLJE = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colZLJE"].Value.ToString());
                        e.CellStyle.ForeColor = ZLJE < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : ZLJE == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZX_ZLJEFormat"))
                    {
                        double ZX_ZLJE = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colZX_ZLJE"].Value.ToString());
                        e.CellStyle.ForeColor = ZX_ZLJE < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : ZX_ZLJE == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else
                    {
                        //
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("数据转换失败", ex);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            recoverSelectedRows();
        }

        private void recoverSelectedRows()
        {
            //如果需要标记选中的行
            if (Common.getShouldMarkUpSelectedRows())
            {
                this.dataGridView1.ClearSelection();
                foreach (DataGridViewRow dgrSingle in this.dataGridView1.Rows)
                {
                    ////板块条件变色
                    string plateCode = dgrSingle.Cells["colPlateID"].Value.ToString();

                    if (this.currentPlate != null && plateCode.Equals(this.currentPlate))
                    {
                        dgrSingle.Selected = true;
                    }
                }
            }
        }


    }
}

