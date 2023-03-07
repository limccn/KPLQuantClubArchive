using KaiPanLaCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace KaiPanLa
{
    public partial class FrmRankList : Form
    {
        private bool isAutoRefresh = true;

        private int pageSize = 100;
        private int marketCap = 5;
        private string marketCapDate = "";
        private string businessDate = "";
        private int sortNameIndex = 1;
        private int sortTypeIndex = 1;

        private string UserId = "";
        private string UserToken = "";


        public Logger logger = Logger._;

        private bool isInit = true;
        private bool isFirstLoad = true;


        public DataTable dtLatest { get; set; }

        public FrmRankList()
        {
            InitializeComponent();
        }

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(0, 0);
            this.Width = size.Width / 2 - 10;
            this.Height = size.Height / 2 - 100;
        }

        private void initControls()
        {
            this.toolStripComboBox1.Text = this.pageSize.ToString();
            this.toolStripComboBox2.Text = "5";
            this.toolStripComboBox3.Text = "1000";
            this.toolStripComboBox4.Text = "净额";
            this.toolStripComboBox5.Text = "降序";
            this.toolStripComboBox6.Text = "";
        }

        private void initDelagates()
        {
            this.setCallBack = new setStockRankingCallBack(getStockRankingCallbackMethod);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            this.toolStripLabel4.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");//2020-01-01 13:21:25

            // 交易日，交易时间段刷新
            if (Common.isBusinessTime(dt))
            {
                this.toolStripComboBox6.Text = this.businessDate;

                this.getStockRankingData();
            }
        }

        private void runnableGetStockRankingList()
        {

            string url = Common.getStockRankingApiRequestUrl();

            StockRankingRequest param = new StockRankingRequest();
            param.UserID = this.UserId;
            param.Token = this.UserToken;
            param.st = this.pageSize;
            param.Date = this.businessDate;
            param.Ratio = this.marketCap;
            param.Type = this.sortNameIndex;
            param.Order = this.sortTypeIndex;

            StockRankingsEntity entity = this.postStockRankingRequest(url, param);
            if (entity != null)
            {
                Invoke(setCallBack, entity);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.isAutoRefresh)
            {
                this.isAutoRefresh = false;
                this.toolStripButton2.Text = "自动更新：关闭";
                this.toolStripButton2.Enabled = true;
                this.timer1.Stop();
            }
            else
            {
                this.isAutoRefresh = true;
                this.toolStripButton2.Text = "自动更新：开启";
                this.toolStripButton2.Enabled = true;
                this.timer1.Start();
            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strPageSize = toolStripComboBox1.Text.Trim();
            if (String.IsNullOrEmpty(strPageSize))
            {
                return;
            }
            int pageSize;
            if (Int32.TryParse(strPageSize, out pageSize))
            {

                if (pageSize == 10 ||
                    pageSize == 25 ||
                    pageSize == 50 ||
                    pageSize == 100 ||
                    pageSize == 200)
                {
                    this.pageSize = pageSize;
                    //刷新数据
                    if (this.isInit)
                    {

                    }
                    else
                    {
                        this.getStockRankingData();
                    }
                }
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strInterval = toolStripComboBox2.Text.Trim();
            if (String.IsNullOrEmpty(strInterval))
            {
                return;
            }
            int timeInterval;
            if (Int32.TryParse(strInterval, out timeInterval))
            {

                if (timeInterval == 5 ||
                    timeInterval == 10 ||
                    timeInterval == 20 ||
                    timeInterval == 30 ||
                    timeInterval == 60)
                {
                    this.timer1.Stop();
                    this.timer1.Interval = timeInterval * 1000;
                    this.timer1.Start();
                    return;
                }
            }
        }

        private void toolStripComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSort = toolStripComboBox4.Text.Trim();
            if (String.IsNullOrEmpty(strSort))
            {
                return;
            }
            switch (strSort)
            {

                case "净额":
                    this.sortNameIndex = 1;
                    break;
                case "流通市值":
                    this.sortNameIndex = 2;
                    break;
                case "主力买入":
                    this.sortNameIndex = 3;
                    break;
                case "主力卖出":
                    this.sortNameIndex = 4;
                    break;
                case "区间涨幅":
                    this.sortNameIndex = 5;
                    break;
                case "最新涨幅":
                    this.sortNameIndex = 6;
                    break;
                default:
                    this.sortNameIndex = 1;
                    break;
            }
            //刷新数据
            if (this.isInit)
            {

            }
            else
            {
                this.getStockRankingData();
            }
        }

        private void toolStripComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSort = toolStripComboBox5.Text.Trim();
            if (String.IsNullOrEmpty(strSort))
            {
                return;
            }
            switch (strSort)
            {

                case "升序":
                    this.sortTypeIndex = 0;
                    break;
                case "降序":
                    this.sortTypeIndex = 1;
                    break;
                default:
                    break;
            }
            //刷新数据
            if (this.isInit)
            {

            }
            else
            {
                this.getStockRankingData();
            }
        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMktCap = toolStripComboBox3.Text.Trim();
            if (String.IsNullOrEmpty(strMktCap))
            {
                return;
            }
            int intMktCap;
            if (Int32.TryParse(strMktCap, out intMktCap))
            {

                switch (intMktCap)
                {
                    case 1000:
                        this.marketCap = 5;
                        break;
                    case 500:
                        this.marketCap = 4;
                        break;
                    case 300:
                        this.marketCap = 3;
                        break;
                    case 200:
                        this.marketCap = 2;
                        break;
                    case 100:
                        this.marketCap = 1;
                        break;
                    case 50:
                        this.marketCap = 0;
                        break;
                    default:
                        return;
                }
            }
            //刷新数据
            if (this.isInit)
            {

            }
            else
            {
                this.getStockRankingData();
            }
        }

        private void toolStripComboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDate = toolStripComboBox6.Text.Trim();
            if (String.IsNullOrEmpty(strDate))
            {
                return;
            }
            DateTime dt;
            if (DateTime.TryParse(strDate, out dt))
            {
                this.marketCapDate = dt.ToString("yyyy-MM-dd");
                this.businessDate = dt.ToString("yyyy-MM-dd");
            }
            //不刷新
        }

        //定义回调
        private delegate void setStockRankingCallBack(StockRankingsEntity value);
        //声明回调
        private setStockRankingCallBack setCallBack;

        private void getStockRankingCallbackMethod(StockRankingsEntity value)
        {
            logger.Info("新线程读取远程服务器数据结束，主线程刷新UI");
            if (value == null)
            {
                logger.Info("服务端返回的数据为空");
                return;
            }

            //数据不为空清除首次加载状态
            this.isFirstLoad = false;

            Int64 updateTime = value.Time;
            DateTime dtUpdate = Common.convertTimeStamp(updateTime);
            this.toolStripLabel4.Text = dtUpdate.ToString("yyyy-MM-dd HH:mm:ss");//2020-01-01 13:21:25

            //清空界面显示
            DataTableToEntity<StockRankingListItemEntity> util = new DataTableToEntity<StockRankingListItemEntity>();
            List<StockRankingListItemEntity> listClear = new List<StockRankingListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            if (value.list != null && value.list.Count > 0)
            {

                //DataTableToEntity<StockRankingListItemEntity> util = new DataTableToEntity<StockRankingListItemEntity>();
                DataTable dt = util.FillDataTable(value.list);

                //按涨速排序
                dt.DefaultView.Sort = "Speed DESC";
                dt = dt.DefaultView.ToTable();

                this.dataGridView1.DataSource = dt;
                this.dataGridView1.ClearSelection();

                //this.dataGridView1.Visible = true;
                this.dataGridView1.AutoGenerateColumns = false;
                this.toolStripLabel1.Visible = false;
                this.toolStripLabel1.Text = "";

                if (Common.getShouldWriteDataToDB())
                {
                    this.dataTableToSQLServer(dt);
                }

                //传值给Mdi窗体
                FrmMain frmMain = (FrmMain)this.MdiParent;
                // 非交易时间段，不主动刷新数据
                if (Common.isBusinessTime(DateTime.Now))
                {
                    frmMain.handleStockRankingData(dt, dtUpdate);
                }
            }
            else
            {
                //this.dataGridView1.Visible = false;
                this.toolStripLabel1.Visible = true;
                this.toolStripLabel1.Text = "没有符合条件的数据";
            }


            List<string> day = value.Day;
            if (day != null && day.Count > 0)
            {
                this.toolStripComboBox6.Visible = true;
                this.toolStripComboBox6.Items.Clear();
                this.toolStripComboBox6.Items.AddRange(day.ToArray<string>());
                // 选定第一个
                this.toolStripComboBox6.SelectedIndex = 0;
            }
            else
            {
                this.toolStripComboBox6.Visible = false;
                this.toolStripComboBox6.Text = "";
            }
        }

        private StockRankingsEntity postStockRankingRequest(string url, StockRankingRequest param)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;

            StockRankingsEntity entity = null;
            try
            {
                logger.Info("请求StockRankings服务端数据开始");
                re = (HttpWebRequest)WebRequest.Create(url);
                re.Method = "POST";
                re.ContentType = "application/x-www-form-urlencoded";

                StringBuilder sb = new StringBuilder();

                sb.Append(String.Format("{0}={1}&", "c", HttpUtility.UrlEncode(param.c)));
                sb.Append(String.Format("{0}={1}&", "a", HttpUtility.UrlEncode(param.a)));
                sb.Append(String.Format("{0}={1}&", "Date", HttpUtility.UrlEncode(param.Date)));
                sb.Append(String.Format("{0}={1}&", "RStart", param.RStart));
                sb.Append(String.Format("{0}={1}&", "REnd", param.REnd));
                sb.Append(String.Format("{0}={1}&", "Ratio", param.Ratio));
                sb.Append(String.Format("{0}={1}&", "Type", param.Type));
                sb.Append(String.Format("{0}={1}&", "Order", param.Order));
                sb.Append(String.Format("{0}={1}&", "index", param.index));
                sb.Append(String.Format("{0}={1}&", "st", param.st));
                sb.Append(String.Format("{0}={1}&", "UserID", HttpUtility.UrlEncode(param.UserID)));
                sb.Append(String.Format("{0}={1}", "Token", HttpUtility.UrlEncode(param.Token)));

                logger.Info(String.Format("请求URL：{0}", url));
                logger.Info(String.Format("请求参数：{0}", sb.ToString()));

                byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                re.ContentLength = data.Length;
                using (Stream reqStream = re.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }

                using (res = (HttpWebResponse)re.GetResponse())
                {
                    logger.Info("请求StockRankings服务端数据处理完成");
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        using (System.IO.Stream stream = res.GetResponseStream())
                        {
                            entity = this.parseResponse(stream);
                        }
                    }
                }
                logger.Info("请求StockRankings服务端数据处理完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("请求StockRankings服务端数据失败", ex);
                return null;
            }
            return entity;
        }

        private StockRankingsEntity requestRemoteData(string url)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            StockRankingsEntity entity = null;
            try
            {
                logger.Info("请求StockRankings服务端数据开始");
                re = (HttpWebRequest)WebRequest.Create(url);
                using (res = (HttpWebResponse)re.GetResponse())
                {
                    logger.Info("请求StockRankings服务端数据完成");
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        using (System.IO.Stream stream = res.GetResponseStream())
                        {
                            entity = this.parseResponse(stream);
                        }
                    }
                }
                logger.Info("请求StockRank服务端数据处理完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("请求服务端数据失败", ex);
                return null;
            }
            return entity;
        }

        private StockRankingsEntity parseResponse(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            try
            {
                logger.Info("解析服务端返回JSON数据开始");
                string temp = sr.ReadToEnd();

                JObject jo = (JObject)JsonConvert.DeserializeObject(temp);


                StockRankingsEntity entity = JsonConvert.DeserializeObject<StockRankingsEntity>(jo.ToString());

                //if (entity.list == null)
                //{
                //    string entities = jo["list"].ToString();
                //    List<StockRankingListItemEntity> list = JsonConvert.DeserializeObject<List<StockRankingListItemEntity>>(entities);

                DateTime dtUpdate = Common.convertTimeStamp(entity.Time);

                int index = 0;
                foreach (StockRankingListItemEntity e in entity.list)
                {
                    e._Number = ++index;
                    e.Date = dtUpdate.ToString("yyyyMMdd");
                    e._Date = dtUpdate.ToString("yyyy/MM/dd");
                    e.Time = dtUpdate.ToString("HHmmss");
                    e._Time = dtUpdate.ToString("HH:mm:ss");
                    e._SJLTP = e.SJLTP / 100000000.0;
                    e._CJE = e.CJE / 100000000.0;
                    e._ZLJE = e.ZLJE / 10000.0;
                    e._Buy = e.Buy / 10000.0;
                    e._Sell = e.Sell / 10000.0;
                    e._Rate = e.Rate / 100;
                    e._Ratio = e.Ratio / 100;
                    e._QJZF = e.QJZF / 100;
                }

                //}

                //if (entity.Day == null)
                //{
                //    string days = jo["Day"].ToString();
                //    List<string> list_days = JsonConvert.DeserializeObject<List<string>>(days);
                //    entity.Day = list_days;
                //}

                logger.Info("解析服务端返回JSON数据完成");
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("解析服务端返回JSON数据失败", ex);
                return null;
            }
            finally
            {
                sr.Dispose();
                sr.Close();
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            this.getStockRankingData();
        }

        private void getStockRankingData()
        {
            FrmMain frmMain = (FrmMain)this.MdiParent;
            //等待服务器状态检查
            if (frmMain.isServiceOK)
            {
                logger.Info("启动新线程读取远程服务器数据");
                Thread thread = new Thread(new ThreadStart(runnableGetStockRankingList));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                logger.Info("远程服务器异常，取消读取操作");
            }
        }

        private void dataTableToSQLServer(DataTable dt)
        {
            FrmMain frmMain = (FrmMain)this.MdiParent;
            if (!frmMain.isDatabaseOK)
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

            string tablename = Common.GetAppSettingByKey("StockRankingTableName");//要插入的表的表名

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    try
                    {
                        logger.Info("DataTable写入DB开始,表名：" + tablename);

                        bulkCopy.DestinationTableName = tablename;
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
                        logger.Info("DataTable写入DB完成,表名：" + tablename);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        logger.Warn("DataTable写入DB失败,表名：" + tablename, ex);
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

        private void FrmRankList_Load(object sender, EventArgs e)
        {
            this.performSizeChanged(this.MdiParent.Size);
            this.initControls();
            this.initDelagates();

            FrmMain frmMain = (FrmMain)this.MdiParent;
            this.UserId = frmMain.UserId;
            this.UserToken = frmMain.UserToken;

            //每秒检查状态
            this.timer2.Start();
            this.displayButtons(DateTime.Now);
            this.checkAutoRefresh(DateTime.Now);

            this.isFirstLoad = true;
            //延时启动开始
            this.timer3.Start();

            //  获取数据
            //this.getStockRankingData();

            this.isInit = false;
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            // 更新businessDate
            if (Common.isBusinessDate(dt))
            {
                this.businessDate = dt.ToString("yyyy-MM-dd");
            }

            this.displayButtons(dt);
            this.checkAutoRefresh(dt);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //延时启动专用tick
            //非交易时间段，首次加载时更新
            if (this.isFirstLoad)
            {
                logger.Info("首次打开窗口，等待开始读取远程数据");
                this.timer3.Start();
                // 交易时间内，使用自动刷新的数据，无需刷新
                if (Common.isBusinessTime(DateTime.Now))
                {
                    logger.Info("交易时间段，使用自动更新获取服务器数据");
                    return;
                }
                else
                {
                    logger.Info("非交易时间段，主动获取服务器数据");
                    this.getStockRankingData();
                }
            }
            else
            {
                //延时加载结束
                this.timer3.Stop();
            }
        }

        private void displayButtons(DateTime dt)
        {
            if (Common.isBusinessTime(dt))
            {
                //this.toolStripButton2.Visible = true;
                this.toolStripLabel2.Visible = true;
                this.toolStripComboBox2.Visible = true;
                this.toolStripLabel5.Visible = true;
            }
            else
            {
                //this.toolStripButton2.Visible = false;
                this.toolStripLabel2.Visible = false;
                this.toolStripComboBox2.Visible = false;
                this.toolStripLabel5.Visible = false;
            }
        }

        private void checkAutoRefresh(DateTime dt)
        {
            if (Common.isBusinessTime(dt))
            {
                if (this.isAutoRefresh)
                {
                    this.toolStripButton2.Text = "自动更新：开启";
                    this.toolStripButton2.Enabled = true;
                    this.timer1.Start();
                }
                else
                {
                    this.toolStripButton2.Text = "自动更新：关闭";
                    this.toolStripButton2.Enabled = true;
                    this.timer1.Stop();
                }
            }
            else
            {
                this.toolStripButton2.Text = "自动更新：等待";
                this.toolStripButton2.Enabled = false;
                this.timer1.Stop();
            }
        }

        private void toolStripLabel8_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //关闭选中，清除已选择的行
            this.dataGridView1.ClearSelection();
        }

    }
}
