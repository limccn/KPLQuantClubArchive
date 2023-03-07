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

namespace KaiPanLaPlate
{
    public partial class FrmPlate : Form
    {
        private bool isAutoRefresh = true;

        private int pageSize = 10;
        private string businessDate = "";
        private int sortNameIndex = 1;
        private int sortTypeIndex = 1;

        private int scrollIndex;

        private string UserId = "";
        private string UserToken = "";

        public Logger logger = Logger._;

        private bool isInit = true;
        private bool isFirstLoad = true;

        private string selectPlateID = "";

        private string[] exceptPlateCode = { "801314", "885699", "885858" };


        public DataTable dtLatest { get; set; }

        public FrmPlate()
        {
            InitializeComponent();
        }

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(0, 0);
            this.Width = 885;
            this.Height = 290;
        }

        private void initControls()
        {
            this.toolStripComboBox1.Text = this.pageSize.ToString();
            this.toolStripComboBox2.Text = "5"; //10秒刷新
            this.timer1.Stop();
            this.timer1.Interval = 5 * 1000; //10秒刷新
            this.timer1.Start();
            this.toolStripComboBox4.Text = "强度";
            this.toolStripComboBox5.Text = "降序";


        }

        private void initDelagates()
        {
            this.setCallBack = new setPlateListCallBack(getPlateListCallbackMethod);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            this.toolStripLabel4.Text = dt.ToString("yyyy-MM-dd HH:mm:ss");//2020-01-01 13:21:25

            // 交易日，交易时间段刷新
            if (Common.isBusinessTime(dt)
                && Common.isSessionTime(dt))
            {
                this.getPlateListData();
            }
        }

        private void runnableGetPlateList()
        {

            string url = Common.getPlatePostRequestUrl();

            PlateListRequest param = new PlateListRequest();
            param.UserID = this.UserId;
            param.Token = this.UserToken;
            param.Order = this.sortTypeIndex;
            param.Type = this.sortNameIndex;
            param.st = this.pageSize;  // 与API保持一致

            PlateListWrapperEntity entity = this.postPlateListRequest(url, param);
            if (entity != null)
            {
                Invoke(setCallBack, entity);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.getPlateListData();
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

                if (pageSize == 5 ||
                    pageSize == 10)
                {
                    this.pageSize = pageSize;
                    //刷新数据
                    if (this.isInit)
                    {

                    }
                    else
                    {
                        this.getPlateListData();
                    }
                }
            }

            //this.scrollIndex = -1;
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

            //this.scrollIndex = -1;
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
                case "强度":
                    this.sortNameIndex = 1;
                    break;
                case "涨跌幅":
                    this.sortNameIndex = 2;
                    break;
                case "涨速":
                    this.sortNameIndex = 3;
                    break;
                case "成交额":
                    this.sortNameIndex = 4;
                    break;
                case "主力净额":
                    this.sortNameIndex = 5;
                    break;
                case "主力买入":
                    this.sortNameIndex = 6;
                    break;
                case "主力卖出":
                    this.sortNameIndex = 7;
                    break;
                case "量比":
                    this.sortNameIndex = 8;
                    break;
                case "流通市值":
                    this.sortNameIndex = 9;
                    break;
                default:
                    this.sortNameIndex = 1;
                    break;
            }

            //this.scrollIndex = -1;
            //刷新数据
            if (this.isInit)
            {

            }
            else
            {
                this.getPlateListData();
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
                this.getPlateListData();
            }
        }

        //定义回调
        private delegate void setPlateListCallBack(PlateListWrapperEntity value);
        //声明回调
        private setPlateListCallBack setCallBack;

        private void getPlateListCallbackMethod(PlateListWrapperEntity value)
        {
            logger.Info("新线程读取远程服务器数据结束，主线程刷新UI");
            if (value == null)
            {
                logger.Info("服务端返回的数据为空");
                return;
            }

            //数据不为空清除首次加载状态
            this.isFirstLoad = false;

            PlateListEntity listEntity = value.plates;

            Int64 updateTime = listEntity.Time;
            DateTime datetimeUpdate = Common.convertTimeStamp(updateTime);
            this.toolStripLabel4.Text = datetimeUpdate.ToString("yyyy-MM-dd HH:mm:ss");//2020-01-01 13:21:25

            //清空界面显示
            DataTableToEntity<PlateListItemEntity> util = new DataTableToEntity<PlateListItemEntity>();
            List<PlateListItemEntity> listClear = new List<PlateListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            if (listEntity.parsedList != null && listEntity.parsedList.Count > 0)
            {
                //DataTableToEntity<StockRankingListItemEntity> util = new DataTableToEntity<StockRankingListItemEntity>();
                DataTable dt = util.FillDataTable(listEntity.parsedList);

                //按涨速排序
                //dt.DefaultView.Sort = "_Number DESC";
                dt = dt.DefaultView.ToTable();

                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = dt;
                //this.dataGridView1.ClearSelection();

                //if (this.dataGridView1.Rows.Count > 0)
                //{
                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.scrollIndex;
                //}

                //this.dataGridView1.Visible = true;

                this.toolStripLabel1.Visible = false;
                this.toolStripLabel1.Text = "";

                if (Common.getShouldWriteDataToDB()
                    && Common.isSessionTime(datetimeUpdate)
                    && Common.isFilterdTime(datetimeUpdate))
                {
                    this.dataTableToSQLServer(dt);
                }

                Dictionary<string, PlateParam> parsedPlateDict = new Dictionary<string, PlateParam>();
                foreach (PlateListItemEntity e in listEntity.parsedList)
                {
                    PlateParam plateParam = new PlateParam();
                    plateParam.Code = e.Code;
                    plateParam.Name = e.Name;
                    plateParam.QD = e.QD;
                    parsedPlateDict.Add(e.Code, plateParam);
                }

                //传值给Mdi窗体
                FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
                // 非交易时间段，不主动刷新数据
                if (Common.isBusinessTime(DateTime.Now))
                {
                    frmMain.handlePlateListData(parsedPlateDict, datetimeUpdate);
                }
            }
            else
            {
                //this.dataGridView1.Visible = false;
                this.toolStripLabel1.Visible = true;
                this.toolStripLabel1.Text = "没有符合条件的数据";
            }

        }

        private PlateListWrapperEntity postPlateListRequest(string url, PlateListRequest param)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;

            PlateListWrapperEntity entity = null;
            try
            {
                logger.Info("请求Plate服务端数据开始");
                re = (HttpWebRequest)WebRequest.Create(url);
                re.Method = "POST";
                re.ContentType = "application/x-www-form-urlencoded";

                StringBuilder sb = new StringBuilder();

                sb.Append(String.Format("{0}={1}&", "c", HttpUtility.UrlEncode(param.c)));
                sb.Append(String.Format("{0}={1}&", "a", HttpUtility.UrlEncode(param.a)));
                sb.Append(String.Format("{0}={1}&", "SelType", param.SelType));
                sb.Append(String.Format("{0}={1}&", "ZSType", param.ZSType));
                sb.Append(String.Format("{0}={1}&", "Type", param.Type));
                sb.Append(String.Format("{0}={1}&", "Order", param.Order));
                sb.Append(String.Format("{0}={1}&", "Start", param.Start));
                sb.Append(String.Format("{0}={1}&", "End", param.End));
                sb.Append(String.Format("{0}={1}&", "Index", param.Index));
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
                    logger.Info("请求Plate服务端数据处理完成");
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        using (System.IO.Stream stream = res.GetResponseStream())
                        {
                            entity = this.parseResponse(stream);
                        }
                    }
                }
                logger.Info("请求Plate服务端数据处理完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("请求Plate服务端数据失败", ex);
                return null;
            }
            return entity;
        }

        //private StockRankingsEntity requestRemoteData(string url)
        //{
        //    HttpWebRequest re = null;
        //    HttpWebResponse res = null;
        //    StockRankingsEntity entity = null;
        //    try
        //    {
        //        logger.Info("请求Plate服务端数据开始");
        //        re = (HttpWebRequest)WebRequest.Create(url);
        //        using (res = (HttpWebResponse)re.GetResponse())
        //        {
        //            logger.Info("请求Plate服务端数据完成");
        //            if (res.StatusCode == HttpStatusCode.OK)
        //            {
        //                using (System.IO.Stream stream = res.GetResponseStream())
        //                {
        //                    entity = this.parseResponse(stream);
        //                }
        //            }
        //        }
        //        logger.Info("请求StockRank服务端数据处理完成");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        logger.Warn("请求服务端数据失败", ex);
        //        return null;
        //    }
        //    return entity;
        //}

        private PlateListWrapperEntity parseResponse(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            try
            {
                logger.Info("解析服务端返回JSON数据开始");
                string temp = sr.ReadToEnd();

                JObject jo = (JObject)JsonConvert.DeserializeObject(temp);


                PlateListEntity plates = JsonConvert.DeserializeObject<PlateListEntity>(jo.ToString());
                List<PlateListItemEntity> parsedList = new List<PlateListItemEntity>();

                //if (entity.list == null)
                //{
                //    string entities = jo["list"].ToString();
                //    List<StockRankingListItemEntity> list = JsonConvert.DeserializeObject<List<StockRankingListItemEntity>>(entities);

                DateTime dtUpdate = Common.convertTimeStamp(plates.Time);

                int index = 0;
                foreach (Object[] oa in plates.list)
                {
                    //只处理前page条数
                    if (index >= this.pageSize)
                    {
                        break;
                    }

                    //排除ST和科创板
                    if (this.exceptPlateCode.Contains(oa[0].ToString()))
                    {
                        continue;
                    }

                    PlateListItemEntity e = new PlateListItemEntity();

                    e.Date = dtUpdate.ToString("yyyyMMdd");
                    e._Date = dtUpdate.ToString("yyyy/MM/dd");
                    e.Time = dtUpdate.ToString("HHmmss");
                    e._Time = dtUpdate.ToString("HH:mm:ss");
                    e._Number = ++index;
                    e.Code = oa[0].ToString();
                    e.Name = oa[1].ToString();
                    e.QD = Double.Parse(oa[2].ToString());
                    e.Rate = Double.Parse(oa[3].ToString());
                    e.Speed = Double.Parse(oa[4].ToString());
                    e.CJE = Double.Parse(oa[5].ToString());
                    e.ZLJE = Double.Parse(oa[6].ToString());
                    e.Buy = Double.Parse(oa[7].ToString());
                    e.Sell = Double.Parse(oa[8].ToString());
                    e.LB = Double.Parse(oa[9].ToString());
                    e.LTSZ = Double.Parse(oa[10].ToString());
                    e.QJZF = Double.Parse(oa[11].ToString());
                    e.LastPrice = "";

                    e._Speed = e.Speed / 100.0;
                    e._Rate = e.Rate / 100.0;
                    e._LTSZ = e.LTSZ / 100000000.0;
                    e._CJE = e.CJE / 100000000.0;
                    e._ZLJE = e.ZLJE / 10000.0;
                    e._Buy = e.Buy / 10000.0;
                    e._Sell = e.Sell / 10000.0;
                    e._ZLJE = e.ZLJE / 10000.0;
                    e._ZLJE_Format = (e._ZLJE >= 10000 || e._ZLJE <= -10000) ? (e._ZLJE / 10000).ToString("0.00") + "亿" : e._ZLJE.ToString("0") + "万";
                    e._Buy = e.Buy / 10000.0;
                    e._Buy_Format = (e._Buy >= 10000) ? (e._Buy / 10000).ToString("0.00") + "亿" : e._Buy.ToString("0") + "万";
                    e._Sell = e.Sell / 10000.0;
                    e._Sell_Format = (e._Sell <= -10000) ? (e._Sell / 10000).ToString("0.00") + "亿" : e._Sell.ToString("0") + "万";


                    parsedList.Add(e);

                }

                plates.parsedList = parsedList;

                //}

                //if (entity.Day == null)
                //{
                //    string days = jo["Day"].ToString();
                //    List<string> list_days = JsonConvert.DeserializeObject<List<string>>(days);
                //    entity.Day = list_days;
                //}

                logger.Info("解析服务端返回JSON数据完成");
                PlateListWrapperEntity entity = new PlateListWrapperEntity();
                entity.plates = plates;
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



        private void getPlateListData()
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            //等待服务器状态检查
            if (frmMain.isServiceOK)
            {
                logger.Info("启动新线程读取远程服务器数据");
                Thread thread = new Thread(new ThreadStart(runnableGetPlateList));
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
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            if (Common.getIsLightWeightMode())
            {
                logger.Info("轻量模式，取消写入");
                return;
            }
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

            string tablename = Common.GetAppSettingByKey("PlateListTableName");//要插入的表的表名

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

            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
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
            //this.getPlateListData();

            this.isInit = false;
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
                if (Common.isBusinessTime(DateTime.Now)
                    && Common.isSessionTime(DateTime.Now))
                {
                    logger.Info("交易时间段，使用自动更新获取服务器数据");
                    return;
                }
                else
                {
                    logger.Info("非交易时间段，主动获取服务器数据");
                    this.getPlateListData();
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
            // this.dataGridView1.ClearSelection();
            //只有一行默认不管
            if (this.dataGridView1.Rows.Count > 1 && this.dataGridView1.SelectedRows.Count > 0)
            {

                DataGridViewRow dgrSingle = this.dataGridView1.SelectedRows[0];
                string selectPlateID = dgrSingle.Cells["colCode"].Value.ToString();
                if (!this.selectPlateID.Equals(selectPlateID))
                {
                    this.selectPlateID = selectPlateID;

                    //传值给Mdi窗体
                    FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
                    // 非交易时间段，不主动刷新数据
                    if (Common.isBusinessTime(DateTime.Now))
                    {
                        frmMain.handlePlateListSelectChanged(selectPlateID, DateTime.Now);
                    }
                }
            }
        }


        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {

            if (this.dataGridView1.FirstDisplayedScrollingRowIndex >= 0)
            {
                this.scrollIndex = this.dataGridView1.FirstDisplayedScrollingRowIndex;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.selectPlateID))
            {
                if (this.dataGridView1.Rows.Count > 0)
                {

                    foreach (DataGridViewRow dr in this.dataGridView1.Rows)
                    {
                        string code = dr.Cells["colCode"].Value.ToString();
                        if (this.selectPlateID.Equals(code))
                        {
                            dr.Selected = true;
                            //只选一行
                            break;
                        }
                        else
                        {
                            dr.Selected = false;
                        }
                    }

                }
            }
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
                    if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSellFormat"))
                    {
                        e.CellStyle.ForeColor = SystemInformation.HighContrast ? Color.Lime : Color.Green;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZLJEFormat"))
                    {
                        double ZLJE = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colZLJE"].Value.ToString());
                        e.CellStyle.ForeColor = ZLJE < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : ZLJE == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSpeed"))
                    {
                        double speed = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colSpeed2"].Value.ToString());
                        e.CellStyle.ForeColor = speed < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : speed == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colRate"))
                    {
                        double rate = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colRate3"].Value.ToString());
                        e.CellStyle.ForeColor = rate < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : rate == 0 ? SystemColors.ControlText : Color.Red;
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sortText = "强度";
            string sortOrder = "降序";

            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colQD"))
            {
                sortText = "强度";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSpeed"))
            {
                sortText = "涨速";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZLJEFormat"))
            {
                sortText = "主力净额";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colRate"))
            {
                sortText = "涨跌幅";
            }
            else
            {
                //不在范围内，返回
                return;
            }

            //处理列头文本
            foreach (DataGridViewColumn column in this.dataGridView1.Columns)
            {
                if (!column.Visible)
                {
                    continue;
                }
                string text = column.HeaderText;
                //找到当前列
                if (e.ColumnIndex == column.Index)
                {

                    if (text.Contains("▼"))
                    {
                        column.HeaderText = text.Replace("▼", "▲");
                        sortOrder = "升序";
                    }
                    else if (text.Contains("▲"))
                    {
                        column.HeaderText = text.Replace("▲", "▼");
                        sortOrder = "降序";
                    }
                    else
                    {
                        column.HeaderText = column.HeaderText + "▼";
                        sortOrder = "降序";
                    }
                }
                else
                {
                    column.HeaderText = text.Replace("▼", "").Replace("▲", "");
                }
            }

            this.toolStripComboBox4.SelectedItem = sortText;
            this.toolStripComboBox5.SelectedItem = sortOrder;

        }
    }
}
