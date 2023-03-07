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
    public partial class FrmPlateStockList : Form
    {


        private int pageSize = 30;
        private string businessDate = "";
        private int sortNameIndex = 1;
        private int sortTypeIndex = 1;

        //private int scrollIndex;

        private int QDFilter = 1500;

        private string UserId = "";
        private string UserToken = "";

        private List<string> signalStockCodeList;

        private string currentPlate;
        private Dictionary<string, PlateParam> requestPlates;

        public Logger logger = Logger._;

        private bool isInit = true;
        // private bool isFirstLoad = true;

        //public DataTable dtOrigin = null;
        public DateTime updateTime = DateTime.Now;

        public Dictionary<string, PlateStockListWrapperEntity> dictLatest { get; set; }

        public Dictionary<string, DataTable> dictLatestCache { get; set; }

        public DataTable dtLatest { get; set; }

        public FrmPlateStockList()
        {
            InitializeComponent();
        }

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(885, 0);
            this.Width = size.Width - 885 - 20;
            this.Height = size.Height - 115;
        }


        public void setupPlateListData(Dictionary<string, PlateParam> plates, DateTime updateTime)
        {
            //if (dt == null)
            //{
            //    return;
            //}
            // this.dtOrigin = dt.Copy();
            this.updateTime = updateTime;
            this.businessDate = updateTime.ToString("yyyy-MM-dd");
            // 
            if (plates != null && plates.Count > 0)
            {
                this.requestPlates = plates;
                //空白的key
                if (String.IsNullOrEmpty(this.currentPlate))
                {
                    this.currentPlate = plates[plates.Keys.First()].Code;
                }
                //不存在的key
                if (!plates.Keys.Contains(this.currentPlate))
                {
                    this.currentPlate = plates.Keys.First();
                }

                this.getPlateStockListData();
            }
        }

        public void setupPlateListSelectChanged(string code, DateTime updateTime)
        {
            if (!code.Equals(this.currentPlate))
            {
                if (this.dictLatest == null)
                {
                    return;
                }

                if (this.dictLatestCache == null)
                {
                    return;
                }

                //没有本地缓存则跳过
                if (!this.dictLatest.Keys.Contains(code))
                {
                    return;
                }

                this.currentPlate = code;

                if (this.dictLatestCache.ContainsKey(code))
                {
                    DataTable dt = this.dictLatestCache[code];

                    this.dataGridView1.AutoGenerateColumns = false;
                    this.dataGridView1.DataSource = dt;
                    this.dataGridView1.Refresh();
                }
            }
        }
        public void setupJointStockListData(List<string> list, DateTime updateTime)
        {
            this.businessDate = updateTime.ToString("yyyy-MM-dd");
            if (this.signalStockCodeList == null || list.Count > this.signalStockCodeList.Count)
            {
                this.signalStockCodeList = list;

                this.dataGridView1.DefaultCellStyle.SelectionBackColor = SystemInformation.HighContrast ? SystemColors.Highlight : Color.MistyRose;
                this.dataGridView1.DefaultCellStyle.SelectionForeColor = SystemInformation.HighContrast ? SystemColors.HighlightText : SystemColors.ControlText;

                this.recoverSelectedRows();

                this.dataGridView1.Refresh();
            }
        }
        private void initControls()
        {
            this.toolStripComboBox1.Text = this.pageSize.ToString();
            this.toolStripComboBox4.Text = "涨跌幅";
            this.toolStripComboBox5.Text = "降序";

            this.dataGridView1.DefaultCellStyle.SelectionBackColor = SystemInformation.HighContrast ? SystemColors.Highlight : Color.MistyRose;
            this.dataGridView1.DefaultCellStyle.SelectionForeColor = SystemInformation.HighContrast ? SystemColors.HighlightText : SystemColors.ControlText;

        }

        private void initDelagates()
        {
            this.setCallBack = new setPlateStockListCallBack(getPlateStockListCallbackMethod);
        }

        private void runnableGetPlateStockList()
        {

            string url = Common.getPlatePostRequestUrl();

            Dictionary<string, PlateStockListWrapperEntity> dict = new Dictionary<string, PlateStockListWrapperEntity>();
            //循环发送请求，获得板块个股信息
            foreach (string code in this.requestPlates.Keys)
            {
                PlateStockListRequest param = new PlateStockListRequest();
                param.UserID = this.UserId;
                param.Token = this.UserToken;
                param.Order = this.sortTypeIndex;
                param.Type = this.sortNameIndex;
                param.st = this.pageSize;

                param.PlateID = code;

                PlateStockListWrapperEntity entity = this.postPlateStockListRequest(url, param);

                if (entity != null)
                {
                    dict.Add(code, entity);
                }
            }
            Invoke(setCallBack, dict);

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
                    pageSize == 30
                    )
                {
                    this.pageSize = pageSize;
                    //刷新数据
                    if (this.isInit)
                    {

                    }
                    else
                    {
                        this.getPlateStockListData();
                    }
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
                case "价格":
                    this.sortNameIndex = 10;
                    break;
                case "涨跌幅":
                    this.sortNameIndex = 6;
                    break;
                case "涨速":
                    this.sortNameIndex = 7;
                    break;
                case "成交额":
                    this.sortNameIndex = 8;
                    break;
                case "换手率":
                    this.sortNameIndex = 9;
                    break;
                case "实际流通":
                    this.sortNameIndex = 2;
                    break;
                case "主力买入":
                    this.sortNameIndex = 3;
                    break;
                case "主力卖出":
                    this.sortNameIndex = 4;
                    break;
                case "主力净额":
                    this.sortNameIndex = 1;
                    break;
                case "领涨":
                    this.sortNameIndex = 27;
                    break;

                default:
                    this.sortNameIndex = 6;
                    break;
            }
            //刷新数据
            if (this.isInit)
            {

            }
            else
            {
                this.getPlateStockListData();
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
                this.getPlateStockListData();
            }
        }

        //定义回调
        private delegate void setPlateStockListCallBack(Dictionary<string, PlateStockListWrapperEntity> dict);
        //声明回调
        private setPlateStockListCallBack setCallBack;

        private void getPlateStockListCallbackMethod(Dictionary<string, PlateStockListWrapperEntity> dict)
        {
            logger.Info("新线程读取远程服务器数据结束，主线程刷新UI");
            if (dict == null || dict.Count == 0)
            {
                logger.Info("服务端返回的数据为空或数据不存在");

                this.toolStripLabel1.Visible = true;
                this.toolStripLabel1.Text = "没有符合条件的数据";

                return;
            }

            this.dictLatest = dict;
            this.dictLatestCache = new Dictionary<string, DataTable>();

            DataTableToEntity<PlateStockListItemEntity> util = new DataTableToEntity<PlateStockListItemEntity>();
            List<PlateStockListItemEntity> listClear = new List<PlateStockListItemEntity>();
            DataTable dtClear = util.FillDataTable(listClear);
            this.dataGridView1.DataSource = dtClear;

            //Boolean findCurrent;

            //去重用dict和list
            Dictionary<string, PlateStockListItemEntity> dictItemNoRepeat = new Dictionary<string, PlateStockListItemEntity>();
            List<PlateStockListItemEntity> listItemNoRepeat = new List<PlateStockListItemEntity>();

            DateTime datetimeUpdate = new DateTime();

            //循环处理
            foreach (string key in dict.Keys)
            {
                PlateStockListWrapperEntity entity = dict[key];
                PlateStockListEntity listEntity = entity.stocks;

                // 判断是否为空
                if (listEntity.parsedList == null || listEntity.parsedList.Count == 0)
                {
                    continue;
                }

                //将单次请求的结果转换为dt
                DataTable dt = util.FillDataTable(listEntity.parsedList);
                this.dictLatestCache.Add(key, dt);
                //处理需要显示的部分
                if (this.currentPlate.Equals(key))
                {
                    //findCurrent = true;
                    Int64 updateTime = listEntity.Time;
                    datetimeUpdate = Common.convertTimeStamp(updateTime);

                    this.dtLatest = dt;

                    this.dataGridView1.AutoGenerateColumns = false;
                    this.dataGridView1.DataSource = dt;
                    //this.dataGridView1.ClearSelection();
                    //if (dataGridView1.Rows.Count > this.scrollIndex)
                    //{
                    //this.dataGridView1.FirstDisplayedScrollingRowIndex = this.scrollIndex;
                    //}

                    //this.dataGridView1.Visible = true;

                    this.toolStripLabel1.Visible = false;
                    this.toolStripLabel1.Text = "";
                }


                //PlateParam param = this.requestPlates[key];
                // 筛选需要传参的数据,当前板块强度大于1500

                //使用dict去重复
                foreach (PlateStockListItemEntity stock in listEntity.parsedList)
                {
                    //
                    string keyCode = stock.Code;

                    //不包含，使用字典去重
                    if (!dictItemNoRepeat.ContainsKey(keyCode))
                    {

                        listItemNoRepeat.Add(stock);
                        dictItemNoRepeat.Add(keyCode, stock);
                    }
                    else
                    {
                        dictItemNoRepeat[keyCode] = stock;
                    }
                }

            }

            if (listItemNoRepeat.Count > 0)
            {
                DataTable dtAllItems = util.FillDataTable(listItemNoRepeat);

                //合并不需要显示的部分

                //写入数据库
                if (Common.getShouldWriteDataToDB()
                    && Common.isSessionTime(datetimeUpdate)
                    && Common.isFilterdTime(datetimeUpdate))
                {
                    this.dataTableToSQLServer(dtAllItems);
                }

                //传值给Mdi窗体
                FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
                //非交易时间段，不主动刷新数据
                if (Common.isBusinessTime(DateTime.Now))
                {
                    frmMain.handlePlateStockListData(dtAllItems, this.QDFilter, datetimeUpdate);
                }
            }
            else
            {
                logger.Info("服务端返回的数据为空或数据不存在");
            }

        }

        private PlateStockListWrapperEntity postPlateStockListRequest(string url, PlateStockListRequest param)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;

            PlateStockListWrapperEntity entity = null;
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
                sb.Append(String.Format("{0}={1}&", "Type", param.Type));
                sb.Append(String.Format("{0}={1}&", "Order", param.Order));
                sb.Append(String.Format("{0}={1}&", "Start", param.Start));
                sb.Append(String.Format("{0}={1}&", "End", param.End));
                sb.Append(String.Format("{0}={1}&", "Index", param.Index));
                sb.Append(String.Format("{0}={1}&", "st", param.st));
                sb.Append(String.Format("{0}={1}&", "PlateID", param.PlateID));
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
                            entity = this.parseResponse(stream, param.PlateID);
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

        private PlateStockListWrapperEntity parseResponse(Stream stream, string plateCode)
        {
            StreamReader sr = new StreamReader(stream);
            try
            {
                logger.Info("解析服务端返回JSON数据开始");
                string temp = sr.ReadToEnd();

                JObject jo = (JObject)JsonConvert.DeserializeObject(temp);


                PlateStockListEntity stocks = JsonConvert.DeserializeObject<PlateStockListEntity>(jo.ToString());

                List<PlateStockListItemEntity> parsedList = new List<PlateStockListItemEntity>();

                //if (entity.list == null)
                //{
                //    string entities = jo["list"].ToString();
                //    List<StockRankingListItemEntity> list = JsonConvert.DeserializeObject<List<StockRankingListItemEntity>>(entities);

                DateTime dtUpdate = Common.convertTimeStamp(stocks.Time);

                int index = 0;
                foreach (Object[] oa in stocks.list)
                {
                    PlateStockListItemEntity e = new PlateStockListItemEntity();

                    e.Date = dtUpdate.ToString("yyyyMMdd");
                    e._Date = dtUpdate.ToString("yyyy/MM/dd");
                    e.Time = dtUpdate.ToString("HHmmss");
                    e._Time = dtUpdate.ToString("HH:mm:ss");
                    e._Number = ++index;
                    e.PlateID = plateCode;
                    e.PlateName = this.requestPlates[plateCode].Name;
                    e.PlateQD = this.requestPlates[plateCode].QD;
                    e.Code = oa[0].ToString();
                    e.Name = oa[1].ToString();
                    e.Price = Double.Parse(oa[5].ToString());

                    e.Rate = Double.Parse(oa[6].ToString());
                    e.CJE = Double.Parse(oa[7].ToString());
                    e.Ratio = Double.Parse(oa[8].ToString());
                    e.Speed = Double.Parse(oa[9].ToString());
                    e.SJLTP = Double.Parse(oa[10].ToString());
                    e.Buy = Double.Parse(oa[11].ToString());
                    e.Sell = Double.Parse(oa[12].ToString());
                    e.ZLJE = Double.Parse(oa[13].ToString());
                    e.QJZF = 0; // 没有区间涨幅
                    e.Tude = oa[4].ToString().Replace("'", "");

                    e.LBCS = oa[23].ToString();
                    e.LT = oa[24].ToString();

                    e.SPFD = Double.Parse(oa[28].ToString());
                    e.ZDFD = Double.Parse(oa[29].ToString());
                    e.EJBK = oa[39].ToString();
                    e.LZCS = Double.Parse(oa[40].ToString());
                    e.DDJE300W = Double.Parse(oa[50].ToString());


                    e._Speed = e.Speed / 100.0;
                    e._Rate = e.Rate / 100.0;
                    e._Ratio = e.Ratio / 100.0;
                    e._QJZF = e.QJZF / 100.0;
                    e._SJLTP = e.SJLTP / 100000000.0;
                    e._CJE = e.CJE / 100000000.0;

                    e._ZLJE = e.ZLJE / 10000.0;
                    e._ZLJE_Format = (e._ZLJE >= 10000 || e._ZLJE <= -10000) ? (e._ZLJE / 10000).ToString("0.00") + "亿" : e._ZLJE.ToString("0") + "万";
                    e._Buy = e.Buy / 10000.0;
                    e._Buy_Format = (e._Buy >= 10000) ? (e._Buy / 10000).ToString("0.00") + "亿" : e._Buy.ToString("0") + "万";
                    e._Sell = e.Sell / 10000.0;
                    e._Sell_Format = (e._Sell <= -10000) ? (e._Sell / 10000).ToString("0.00") + "亿" : e._Sell.ToString("0") + "万";


                    e._SPFD = e.SPFD / 10000.0;
                    e._SPFD_Format = e._SPFD == 0 ? "" : (e._SPFD >= 10000) ? (e._SPFD / 10000).ToString("0.00") + "亿" : e._SPFD.ToString("0") + "万";
                    e._ZDFD = e.ZDFD / 10000.0;
                    e._ZDFD_Format = e._ZDFD == 0 ? "" : (e._ZDFD >= 10000) ? (e._ZDFD / 10000).ToString("0.00") + "亿" : e._ZDFD.ToString("0") + "万";
                    e._DDJE300W = e.DDJE300W / 10000.0;
                    e._DDJE300W_Format = (e._DDJE300W >= 10000 || e._DDJE300W <= -10000) ? (e._DDJE300W / 10000).ToString("0.00") + "亿" : e._DDJE300W.ToString("0") + "万";

                    parsedList.Add(e);
                }

                stocks.parsedList = parsedList;

                //}

                //if (entity.Day == null)
                //{
                //    string days = jo["Day"].ToString();
                //    List<string> list_days = JsonConvert.DeserializeObject<List<string>>(days);
                //    entity.Day = list_days;
                //}

                logger.Info("解析服务端返回JSON数据完成");
                PlateStockListWrapperEntity entity = new PlateStockListWrapperEntity();
                entity.stocks = stocks;
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

        private void getPlateStockListData()
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            //等待服务器状态检查
            if (frmMain.isServiceOK)
            {
                logger.Info("启动新线程读取远程服务器数据");
                Thread thread = new Thread(new ThreadStart(runnableGetPlateStockList));
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

            string tablename = Common.GetAppSettingByKey("PlateStockListTableName");//要插入的表的表名

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

            //this.isFirstLoad = true;
            //延时启动开始

            //  获取数据
            //this.getPlateStockListData();

            this.isInit = false;
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
            //this.dataGridView1.ClearSelection();
            //回复选中行
            //this.recoverSelectedRows();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.getPlateStockListData();
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

                    if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZLJEFormat"))
                    {
                        double ZLJE = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colZLJE"].Value.ToString());
                        e.CellStyle.ForeColor = ZLJE < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : ZLJE == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colDDJE300WFormat"))
                    {
                        double DDJE = Double.Parse(this.dataGridView1.Rows[e.RowIndex].Cells["colDDJE300W"].Value.ToString());
                        e.CellStyle.ForeColor = DDJE < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : DDJE == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSpeed"))
                    {
                        double speed = Double.Parse(e.Value.ToString());
                        e.CellStyle.ForeColor = speed < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : speed == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colRate"))
                    {
                        double rate = Double.Parse(e.Value.ToString());
                        e.CellStyle.ForeColor = rate < 0 ? SystemInformation.HighContrast ? Color.Lime : Color.Green
                            : rate == 0 ? SystemColors.ControlText : Color.Red;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colLZCS"))
                    {
                        double LZCS = Double.Parse(e.Value.ToString());
                        e.CellStyle.ForeColor = LZCS > 0 ? Color.Red : SystemColors.ControlText;
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colName"))
                    {
                        DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];
                        double rate = Double.Parse(dgrSingle.Cells["colRate"].Value.ToString());
                        double ZLJE = Double.Parse(dgrSingle.Cells["colZLJE"].Value.ToString());
                        double speed = Double.Parse(dgrSingle.Cells["colSpeed"].Value.ToString());

                        if (rate >= -0.02 && rate <= 0.06
                            && ZLJE >= 1000)
                        {
                            e.CellStyle.ForeColor = Color.Red;

                        }
                        else
                        {
                            e.CellStyle.ForeColor = SystemColors.ControlText;
                        }
                    }
                    else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSJLTP"))
                    {
                        double SJLTP = Double.Parse(e.Value.ToString());
                        if (SJLTP >= 100)
                        {
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else
                        {
                            e.CellStyle.ForeColor = SystemColors.ControlText;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("数据转换失败", ex);
            }
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            //if (this.dataGridView1.FirstDisplayedScrollingRowIndex >= 0)
            //{
            //    this.scrollIndex = this.dataGridView1.FirstDisplayedScrollingRowIndex;
            //}
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != '.' && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            int value;
            if (Int32.TryParse(this.toolStripTextBox1.Text, out value))
            {
                this.toolStripTextBox1.Text = value.ToString();
                this.QDFilter = value;
                //this.reComputeTable();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            this.recoverSelectedRows();
        }

        private void recoverSelectedRows()
        {

            if (Common.getShouldMarkUpSelectedRows())
            {
                this.dataGridView1.ClearSelection();
                if (this.signalStockCodeList != null)
                {
                    //查找并筛选选中行
                    foreach (DataGridViewRow dgrSingle in this.dataGridView1.Rows)
                    {
                        string stockCode = dgrSingle.Cells["colCode"].Value.ToString();
                        if (this.signalStockCodeList.Contains(stockCode))
                        {
                            dgrSingle.Selected = true;
                        }

                    }
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sortText = "涨跌幅";
            string sortOrder = "降序";

            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colCJE"))
            {
                sortText = "成交额";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSpeed"))
            {
                sortText = "涨速";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSJLTP"))
            {
                sortText = "实际流通";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colZLJEFormat"))
            {
                sortText = "主力净额";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colRate"))
            {
                sortText = "涨跌幅";
            }
            else if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colLZCS"))
            {
                sortText = "领涨";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.recoverSelectedRows();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                string stockCode = this.dataGridView1.Rows[e.RowIndex].Cells["colCode"].Value.ToString();

                FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;

                frmMain.handleStockListDataGridViewCellDoubleClick(stockCode, null);
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801225", null);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801902", null);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801900", null);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801904", null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801903", null);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("801909", null);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            frmMain.handleStockListDataGridViewCellDoubleClick("159602", null);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            string url = "https://www.kaipanla.com/";
            frmMain.handleStockListDataGridViewCellDoubleClick("", url);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            string url = "https://www.kaipanla.com/index.php/live/index";
            frmMain.handleStockListDataGridViewCellDoubleClick("", url);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            FrmMainPlate frmMain = (FrmMainPlate)this.MdiParent;
            string url = "https://cn.investing.com/indices/indices-futures";
            frmMain.handleStockListDataGridViewCellDoubleClick("", url);
        }
    }
}
