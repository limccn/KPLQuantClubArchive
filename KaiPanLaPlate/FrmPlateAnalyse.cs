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
    public partial class FrmPlateAnalyse : Form
    {


        public Logger logger = Logger._;

        public static double WAN = 10000.0;
        //public static double SHIWAN    = 100000.0;
        //public static double BAIWAN    = 1000000.0;
        //public static double QIANWAN   = 10000000.0;
        public static double YI = 100000000.0;
        //public static double SHIYI     = 1000000000.0;

        public static Int64 MAX_THREADHOLD = 99999999;



        private double dZLMDY { get; set; } = 100;
        private double dJEYZ { get; set; } = 2000;
        private double dLTSZ { get; set; } = 10;
        private double dJEZHDY { get; set; } = 406;
        private double dJEZHYZ { get; set; } = 400;
        private double dJEJSL { get; set; } = 50000;
        private double dLTSZYZ { get; set; } = 30;



        private double dZLJE = 800;
        private double dSpeed = 0.1;
        private double dRateFrom = 0.0;
        private double dRateTo = 6.0;
        private double dSJLTPFrom = 35;
        private double dSJLTPTo = 2000;
        private double dTL = 1;
        private double dQD = 10;
        private double dHSL = 0.0;
        private double dMRCJZB = 1.0;

        private double dPlateQDFrom = 1500;


        public DataTable dtOrigin = null;
        public DateTime updateTime = DateTime.Now;

        public FrmPlateAnalyse()
        {
            InitializeComponent();
        }

        public void performSizeChanged(Size size)
        {
            //this.Location = new Point(size.Width - 420, size.Height / 3 - 80);
            //this.Width = 400;
            //this.Height = size.Height / 3 - 110;

            this.WindowState = FormWindowState.Minimized;
        }

        public void setupPlateStockList(DataTable dt, Double QDFilter, DateTime updateTime)
        {
            if (dt == null)
            {
                return;
            }
            this.dtOrigin = dt.Copy();
            this.updateTime = updateTime;
            this.dPlateQDFrom = QDFilter;

            this.queryDatatable();
        }

        public void queryDatatable()
        {
            if (this.dtOrigin == null)
            {
                return;
            }
            DataTable dt = this.dtOrigin.Copy();


            //if (query.Count() > 0)
            //{
            //    DataTable result = query.CopyToDataTable<DataRow>();
            //    this.dataGridView1.DataSource = result;
            //}

            //先清空
            DataTableToEntity<PlateStockAnalyseListItemEntity> util = new DataTableToEntity<PlateStockAnalyseListItemEntity>();
            List<PlateStockAnalyseListItemEntity> listClear = new List<PlateStockAnalyseListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            IEnumerable<DataRow> query = from row in dt.AsEnumerable()
                                         select row;

            List<PlateStockAnalyseListItemEntity> list = new List<PlateStockAnalyseListItemEntity>();
            int index = 0;
            foreach (var row in query)
            {
                var newEntity = new PlateStockAnalyseListItemEntity
                {
                    _Number = ++index,

                    Date = this.updateTime.ToString("yyyyMMdd"),
                    _Date = this.updateTime.ToString("yyyy/MM/dd"),

                    Time = this.updateTime.ToString("HHmmss"),
                    _Time = this.updateTime.ToString("HH:mm:ss"),

                    PlateID = row.Field<string>("PlateID"),
                    PlateName = row.Field<string>("PlateName"),
                    PlateQD = row.Field<double>("PlateQD"),


                    Code = row.Field<string>("Code"),
                    Name = row.Field<string>("Name"),

                    SJLTP = row.Field<double>("SJLTP"),
                    _SJLTP = row.Field<double>("SJLTP") / YI,

                    Tude = row.Field<string>("Tude"),

                    LBCS = row.Field<string>("LBCS"),
                    LT = row.Field<string>("LT"),
                    EJBK = row.Field<string>("EJBK"),

                    SPFD = row.Field<double>("SPFD"),
                    _SPFD = row.Field<double>("SPFD") / WAN,

                    ZDFD = row.Field<double>("ZDFD"),
                    _ZDFD = row.Field<double>("ZDFD") / WAN,

                    LZCS = row.Field<double>("LZCS"),

                    DDJE300W = row.Field<double>("DDJE300W"),
                    _DDJE300W = row.Field<double>("DDJE300W") / WAN,

                    ZLJE = row.Field<double>("ZLJE"),
                    _ZLJE = row.Field<double>("ZLJE") / WAN,

                    Rate = row.Field<double>("Rate"),
                    _Rate = row.Field<double>("Rate") / 100,

                    Price = row.Field<double>("Price"),
                    CJE = row.Field<double>("CJE"),
                    _CJE = row.Field<double>("CJE") / WAN,

                    Ratio = row.Field<double>("Ratio"),
                    _Ratio = row.Field<double>("Ratio") / 100,

                    Speed = row.Field<double>("Speed"),
                    Buy = row.Field<double>("Buy"),
                    _Buy = row.Field<double>("Buy") / WAN,

                    Sell = row.Field<double>("Sell"),
                    _Sell = row.Field<double>("Sell") / WAN,

                    QJZF = row.Field<double>("QJZF"),
                    _QJZF = row.Field<double>("QJZF") / 100,

                    //Tag = row.Field<string>("Tag"),

                    //JCF = (Int64)(100 * row.Field<double>("Buy") / Math.Abs((row.Field<double>("Sell")) < (this.dZLMDY * WAN) ? Math.Abs(row.Field<double>("Sell")) : (this.dZLMDY * WAN))),
                    JCF = this.computeJCF(row),
                    //JEF = (Int64)(100 * row.Field<double>("ZLJE") / (this.dJEYZ * WAN)),
                    JEF = this.computeJEF(row),
                    /*TL = (Int64)(100 * row.Field<double>("Buy")
                        / Math.Abs((row.Field<double>("Sell")) < (this.dZLMDY * WAN) ? Math.Abs(row.Field<double>("Sell")) : (this.dZLMDY * WAN))
                        + (row.Field<double>("ZLJE") / (this.dJEYZ * WAN) * 100)),*/
                    TL = this.computeTL(row),
                    //JEZH = (Int64)(row.Field<double>("ZLJE") * ((this.dLTSZ * YI) / row.Field<double>("SJLTP"))),
                    JEZH = this.computeJEZH(row),
                    //JEZH2 = (Int64)((row.Field<double>("ZLJE") * (this.dLTSZ * YI) / row.Field<double>("SJLTP") - (this.dJEZHYZ * WAN)) / this.dJEJSL),
                    JEZH2 = this.computeJEZH2(row),
                    //QD = (Int64)((row.Field<double>("ZLJE") * ((this.dLTSZ * YI) / row.Field<double>("SJLTP"))) < (this.dJEZHDY * WAN) ? 1 : (row.Field<double>("ZLJE") * (this.dLTSZ * YI) / row.Field<double>("SJLTP") - (this.dJEZHYZ * WAN)) / this.dJEJSL),
                    QD = this.computeQD(row),
                };
                list.Add(newEntity);
            }

            //重新装载
            DataTable dtEntity = util.FillDataTable(list);
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;

            // 刷新最新数据
            if (dtEntity != null)
            {
                FrmMainPlate.handlePlateStockAnalyseData(dtEntity, this.updateTime);
            }

            //this.dataGridView1.DataSource = dtEntity;

            IEnumerable<DataRow> query2 = from row_f in dtEntity.AsEnumerable()
                                          where (
                                          row_f.Field<double>("PlateQD") >= this.dPlateQDFrom && //板块强度大于1500
                                          row_f.Field<double>("ZLJE") >= this.dZLJE * WAN &&
                                          row_f.Field<double>("Speed") >= this.dSpeed &&
                                          row_f.Field<double>("Rate") >= this.dRateFrom &&
                                          row_f.Field<double>("Rate") <= this.dRateTo &&
                                          row_f.Field<double>("SJLTP") >= this.dSJLTPFrom * YI &&
                                          row_f.Field<double>("SJLTP") <= this.dSJLTPTo * YI &&
                                          row_f.Field<double>("TL") >= this.dTL &&
                                          row_f.Field<double>("QD") >= this.dQD &&
                                          row_f.Field<double>("Ratio") >= this.dHSL &&
                                          //买成比
                                          (row_f.Field<double>("Buy") / (row_f.Field<double>("CJE") + 1.0)) <= this.dMRCJZB
                                          )
                                          select row_f;



            if (query2.Any()) // 不用count 用any
            {
                DataTable filterd = query2.CopyToDataTable<DataRow>();

                // if (filterd.Rows[0] == null || String.IsNullOrEmpty(filterd.Rows[0].Field<string>("Code")))
                //     return;


                //传值给Mdi窗体
                FrmMainPlate.handleFilterdPlateStockAnalyseData(filterd, this.updateTime);

                this.dataGridView1.ClearSelection();
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = filterd;
                this.dataGridView1.ClearSelection();
                //this.dataGridView1.Visible = true;

                if (Common.getShouldWriteDataToDB()
                    && Common.isSessionTime(this.updateTime)
                    && Common.isFilterdTime(this.updateTime))
                {
                    this.filterdAnalyseTableToDB(filterd);
                }
            }

            // 最后数据入库
            if (Common.getShouldWriteLatestAnalyseToMemoryTable()
                && Common.isSessionTime(this.updateTime)
                && Common.isFilterdTime(this.updateTime))
            {
                this.latestAnalyseTableToDB(dtEntity);
            }

        }

        private Int64 computeJCF(DataRow row)
        {
            if (row.Field<double>("Sell") > 0)
            {
                return 0;
            }
            double sell = Math.Abs(row.Field<double>("Sell")) < (this.dZLMDY * WAN) ? (this.dZLMDY * WAN) : Math.Abs(row.Field<double>("Sell"));
            Int64 value = (Int64)(100 * row.Field<double>("Buy") / sell);

            if (value < 0)
            {
                return 0;
            }
            else if (value > MAX_THREADHOLD)
            {
                return MAX_THREADHOLD;
            }
            else
            {
                return value;
            }
        }

        private Int64 computeJEF(DataRow row)
        {
            Int64 value = (Int64)(100 * row.Field<double>("ZLJE") / (this.dJEYZ * WAN));

            if (value < 0)
            {
                return 0;
            }
            else if (value > MAX_THREADHOLD)
            {
                return MAX_THREADHOLD;
            }
            else
            {
                return value;
            }
        }

        private Int64 computeTL(DataRow row)
        {
            Int64 value = this.computeJCF(row) + this.computeJEF(row);
            if (value < 0)
            {
                return 0;
            }
            else if (value > MAX_THREADHOLD)
            {
                return MAX_THREADHOLD;
            }
            else
            {
                return value;
            }
        }

        private Int64 computeJEZH(DataRow row)
        {
            if (row.Field<double>("SJLTP") <= 0)
            {
                return 0;
            }

            Int64 value = (Int64)(row.Field<double>("Buy") * ((this.dLTSZ * YI) / row.Field<double>("SJLTP")));

            if (value < 0)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        private Int64 computeJEZH2(DataRow row)
        {
            // 流动市值 系数
            Double coff = row.Field<double>("SJLTP") / (this.dLTSZYZ * YI);
            Int64 value = (Int64)((this.computeJEZH(row) - (this.dJEZHYZ * WAN)) / this.dJEJSL);
            // 流动市值系数>1
            if (coff > 1)
            {
                value = (Int64)(value * coff);
            }
            //
            if (value < 0)
            {
                return 0;
            }
            else if (value > MAX_THREADHOLD)
            {
                return MAX_THREADHOLD;
            }
            else
            {
                return value;
            }
        }

        private Int64 computeQD(DataRow row)
        {
            Int64 jezh = this.computeJEZH(row);
            Int64 jezh2 = this.computeJEZH2(row);

            if (jezh < (this.dJEZHDY * WAN))
            {
                return 1;
            }
            else
            {
                return jezh2;
            }
        }




        private void latestAnalyseTableToDB(DataTable dt)
        {
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;
            if (!FrmMainPlate.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消写入");
                return;
            }

            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                string stockLatestAnalyseTableName = Common.GetAppSettingByKey("PlateStockLatestAnalyseTableName");//要插入的表的表名

                //先清空表
                string SQL = "DELETE FROM [" + stockLatestAnalyseTableName + "] WHERE 1 = 1";
                if (Common.getShouldTruncateMemoryTable())
                {
                    SQL = "TRUNCATE TABLE [" + stockLatestAnalyseTableName + "]";
                }
                SqlCommand cmd = new SqlCommand(SQL, destinationConnection);
                int eff = cmd.ExecuteNonQuery();

                logger.Info("清空表数据完成,表名:" + stockLatestAnalyseTableName);

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    try
                    {
                        logger.Info("DataTable写入DB开始,表名:" + stockLatestAnalyseTableName);

                        bulkCopy.DestinationTableName = stockLatestAnalyseTableName;
                        bulkCopy.BatchSize = dt.Rows.Count;

                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.ColumnName.StartsWith("_"))
                            {
                                continue;
                            }
                            else
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.ToUpper());
                            }
                        }

                        bulkCopy.WriteToServer(dt);
                        logger.Info("DataTable写入DB完成,表名:" + stockLatestAnalyseTableName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.Warn("DataTable写入DB失败,表名:" + stockLatestAnalyseTableName, ex);
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

        }

        private void filterdAnalyseTableToDB(DataTable dt)
        {
            FrmMainPlate FrmMainPlate = (FrmMainPlate)this.MdiParent;
            if (!FrmMainPlate.isDatabaseOK)
            {
                logger.Info("数据库服务状态异常，取消写入");
                return;
            }

            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                string PlateStockAnalyseTableName = Common.GetAppSettingByKey("PlateStockAnalyseTableName");//要插入的表的表名
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    try
                    {
                        logger.Info("DataTable写入DB开始");

                        bulkCopy.DestinationTableName = PlateStockAnalyseTableName;//要插入的表的表名
                        bulkCopy.BatchSize = dt.Rows.Count;

                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.ColumnName.StartsWith("_"))
                            {
                                continue;
                            }
                            else
                            {
                                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName.ToUpper());
                            }
                        }

                        bulkCopy.WriteToServer(dt);
                        logger.Info("DataTable写入DB完成,表名：" + PlateStockAnalyseTableName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.Warn("DataTable写入DB失败,表名：" + PlateStockAnalyseTableName, ex);
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

        }

        public void reComputeTable()
        {
            this.queryDatatable();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.reComputeTable();
        }

        private void toolStripTextBox2_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox2.Text, out value))
            {
                this.toolStripTextBox2.Text = value.ToString();
                this.dSpeed = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");


            }
        }


        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox1.Text, out value))
            {
                this.toolStripTextBox1.Text = value.ToString();
                this.dZLJE = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");

            }

        }

        private void toolStripTextBox3_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox3.Text, out value))
            {
                this.toolStripTextBox3.Text = value.ToString();
                if (value <= this.dRateTo)
                {
                    this.dRateFrom = value;
                    this.reComputeTable();
                }
                else
                {
                    MessageBox.Show("请输入正确的范围");

                }
            }
        }

        private void toolStripTextBox5_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox5.Text, out value))
            {
                this.toolStripTextBox5.Text = value.ToString();
                if (value >= this.dRateFrom)
                {
                    this.dRateTo = value;
                    this.reComputeTable();
                }
                else
                {
                    MessageBox.Show("请输入正确的范围");
                }
            }
        }

        private void toolStripTextBox4_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox4.Text, out value))
            {
                this.toolStripTextBox4.Text = value.ToString();
                this.dSJLTPTo = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripTextBox6_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox6.Text, out value))
            {
                this.toolStripTextBox6.Text = value.ToString();
                this.dTL = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripTextBox7_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox7.Text, out value))
            {
                this.toolStripTextBox7.Text = value.ToString();
                this.dQD = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;


        }

        private void toolStripTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;


        }

        private void toolStripTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;

        }

        private void toolStripTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void FrmAnalyse_Load(object sender, EventArgs e)
        {
            this.performSizeChanged(this.MdiParent.Size);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmPlateStockAnalyseFilterCond frmFilterCondition = new FrmPlateStockAnalyseFilterCond();


            frmFilterCondition.dZLMDY = this.dZLMDY;
            frmFilterCondition.dJEYZ = this.dJEYZ;
            frmFilterCondition.dLTSZ = this.dLTSZ;
            frmFilterCondition.dJEZHDY = this.dJEZHDY;
            frmFilterCondition.dJEZHYZ = this.dJEZHYZ;
            frmFilterCondition.dJEJSL = this.dJEJSL;
            frmFilterCondition.dLTSZYZ = this.dLTSZYZ;


            frmFilterCondition.ShowDialog();

            if (frmFilterCondition.DialogResult == DialogResult.OK)
            {

                this.dZLMDY = frmFilterCondition.dZLMDY;
                this.dJEYZ = frmFilterCondition.dJEYZ;
                this.dLTSZ = frmFilterCondition.dLTSZ;
                this.dJEZHDY = frmFilterCondition.dJEZHDY;
                this.dJEZHYZ = frmFilterCondition.dJEZHYZ;
                this.dJEJSL = frmFilterCondition.dJEJSL;
                this.dLTSZYZ = frmFilterCondition.dLTSZYZ;


                this.reComputeTable();
            }
            else if (frmFilterCondition.DialogResult == DialogResult.Cancel)
            {
            }
            frmFilterCondition.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //关闭选中，清除已选择的行
            this.dataGridView1.ClearSelection();
        }

        private void toolStripTextBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox8_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox8.Text, out value))
            {
                this.toolStripTextBox8.Text = value.ToString();
                this.dHSL = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripTextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox9_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox9.Text, out value))
            {
                this.toolStripTextBox9.Text = value.ToString();
                this.dMRCJZB = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripTextBox10_Leave(object sender, EventArgs e)
        {
            double value;
            if (Double.TryParse(this.toolStripTextBox10.Text, out value))
            {
                this.toolStripTextBox10.Text = value.ToString();
                this.dSJLTPFrom = value;
                this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
