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
    public partial class FrmPlateQuant : Form
    {

        public SignalFilterCondition signalFilter1 = new SignalFilterCondition() { enabled = true, QD = 100, BUY = 1200 };
        public SignalFilterCondition signalFilter2 = new SignalFilterCondition() { enabled = true, QD = 60, ZLJE = 2000 };
        public SignalFilterCondition signalFilter3 = new SignalFilterCondition() { enabled = true, TL = 1200, ZLJE = 2000 };
        public SignalFilterCondition signalFilter4 = new SignalFilterCondition();


        public Logger logger = Logger._;

        public static double WAN = 10000.0;
        public static double YI = 100000000.0;

        public bool isRealTimeData = true;

        public DataTable dtJointQuant = null;

        public DateTime updateTime = DateTime.Now;

        public FrmPlateQuant()
        {
            InitializeComponent();
        }

        public void setupJointQuantData(DataTable dt, DateTime updateTime)
        {
            if (dt == null)
            {
                return;
            }
            this.dtJointQuant = dt.Copy();
            this.updateTime = updateTime;

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

            //this.Location = new Point(size.Width - 420, 0);
            //this.Width = 400;
            //this.Height = size.Height / 3 - 80;

            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmPlateSignalFilterCond frmSignalFilter = new FrmPlateSignalFilterCond();
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
            if (this.isRealTimeData)
            {
                frmAnaDisSetting.dgvData = this.dataGridView1;
            }
            else
            {
                frmAnaDisSetting.dgvData = this.dataGridView2;
            }
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
            DataTableToEntity<PlateZXStockSignalListItemEntity> util = new DataTableToEntity<PlateZXStockSignalListItemEntity>();
            List<PlateZXStockSignalListItemEntity> listClear = new List<PlateZXStockSignalListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            if (this.dtJointQuant == null)
            {
                return;
            }

            DataTable dtJointQuant = this.dtJointQuant.Copy();


            if (dtJointQuant != null && dtJointQuant.Rows.Count > 0)
            {
                DataTable dtXHQRSJ = this.filterJointQuantXHQRSJ(dtJointQuant);
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

        private DataTable filterJointQuantXHQRSJ(DataTable source)
        {
            // 如果信号确认时间不为00:00则显示出来
            IEnumerable<DataRow> query = from row_f in source.AsEnumerable()
                                         where (
                                            !row_f.Field<string>("XHQRSJ").Equals("00:00")
                                         )
                                         select row_f;

            if (query.Any())
            {
                DataTable filterd = query.CopyToDataTable<DataRow>();
                return filterd;
            }
            return null;
        }

        private void reLoadFromDatabase()
        {
            //先清空
            DataTableToEntity<PlateStockSignalListItemEntity> util = new DataTableToEntity<PlateStockSignalListItemEntity>();
            List<PlateStockSignalListItemEntity> listClear = new List<PlateStockSignalListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView2.DataSource = dtClear;


            DataTable dtDatatable = this.QueryDatatableFromDatabase();

            IEnumerable<DataRow> query = from row in dtDatatable.AsEnumerable()
                                         select row;

            List<PlateStockSignalListItemEntity> list = new List<PlateStockSignalListItemEntity>();
            int index = 0;
            foreach (var row in query)
            {
                var newEntity = new PlateStockSignalListItemEntity
                {
                    _Number = ++index,

                    Date = row.Field<string>("DATE"),
                    _Date = DateTime.ParseExact(row.Field<string>("DATE"), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy/MM/dd"),
                    Time = row.Field<string>("TIME"),
                    _Time = DateTime.ParseExact(row.Field<string>("TIME"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    SBSJ = DateTime.ParseExact(row.Field<string>("SBSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    XHGXSJ = DateTime.ParseExact(row.Field<string>("XHGXSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),
                    XHQRSJ = DateTime.ParseExact(row.Field<string>("XHQRSJ"), "HHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("HH:mm"),

                    PlateID = row.Field<string>("PlateID"),
                    PlateName = row.Field<string>("PlateName"),
                    PlateQD = (double)row.Field<decimal>("PlateQD"),


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
                    _QJZF = (double)row.Field<decimal>("QJZF") / 100,

                    //Tag = row.Field<string>("TAG"),
                    JCF = (double)row.Field<decimal>("JCF"),
                    JEF = (double)row.Field<decimal>("JEF"),
                    TL = (double)row.Field<decimal>("TL"),
                    JEZH = (double)row.Field<decimal>("JEZH"),
                    JEZH2 = (double)row.Field<decimal>("JEZH2"),
                    QD = (double)row.Field<decimal>("QD"),
                };
                list.Add(newEntity);
            }

            //重新装载
            DataTable dtEntity = util.FillDataTable(list);


            if (dtEntity != null && dtEntity.Rows.Count > 0)
            {
                this.dataGridView2.AutoGenerateColumns = false;
                this.dataGridView2.DataSource = dtEntity;
                //清除选中
                this.dataGridView2.ClearSelection();
            }
            else
            {
            }
        }

        private DataTable QueryDatatableFromDatabase()
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            if (!frmMain.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消读取");
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String tableName = Common.GetAppSettingByKey("PlateStockSignalConfirmTableName");
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

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.isRealTimeData)
            {
                this.isRealTimeData = false;
                this.toolStripButton3.Text = "打开实时变动";
                this.dataGridView1.Visible = false;
                this.dataGridView2.Visible = true;

            }
            else
            {
                this.isRealTimeData = true;
                this.toolStripButton3.Text = "关闭实时变动";
                this.dataGridView1.Visible = true;
                this.dataGridView2.Visible = false;
            }

            this.reComputeTable();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView2.ClearSelection();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView1.ClearSelection();
        }
    }
}
