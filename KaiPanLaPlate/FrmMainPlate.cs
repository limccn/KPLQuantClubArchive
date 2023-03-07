using CefSharp;
using CefSharp.WinForms;
using KaiPanLaCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace KaiPanLaPlate
{
    public partial class FrmMainPlate : Form
    {
        public bool isLogin = false;
        public bool isServiceOK = false;
        public bool isDatabaseOK = false;

        public Logger logger = Logger._;

        public FrmPlate frmPlate = null;
        public FrmPlateStockList frmPlateStockList = null;
        public FrmPlateAnalyse frmPlateAnalyse = null;
        public FrmPlateQuant frmPlateQuant = null;
        public FrmPlateSignal frmPlateSignal = null;
        public FrmPopupStockInfo frmPopupStockInfo = null;
        public FrmPopupStockInfoWeb frmPopupStockInfoWeb = null;




        public string UserId
        {
            get;
            set;
        }

        public string UserToken
        {
            get;
            set;
        }


        public FrmMainPlate()
        {
            InitializeComponent();

            string basePath = this.getAppBaseDir();
            string appDataPath = basePath + @"\AppData";
            // 判断路径
            if (false == System.IO.Directory.Exists(appDataPath))
            {
                //不存在则创建文件夹
                Directory.CreateDirectory(appDataPath);
            }

            CefSettings settings = new CefSettings();
            settings.PersistSessionCookies = true; //持久化session
            settings.CachePath = appDataPath + @"\Cache";
            settings.UserDataPath = appDataPath + @"\UserData";
            settings.LocalesDirPath = appDataPath + @"\Locales";
            settings.LogFile = appDataPath + @"\LogData";
            settings.Locale = "zh-CN";
            settings.AcceptLanguageList = "zh-CN,zh;q=0.9";
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";

            //初始化
            Cef.Initialize(settings);
        }

        public string getAppBaseDir()
        {
            string base_path = "";
            if (System.Environment.CurrentDirectory == AppDomain.CurrentDomain.BaseDirectory)//Windows应用程序则相等  
            {
                base_path = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                base_path = AppDomain.CurrentDomain.BaseDirectory + @"CEFBrowser";
            }
            return base_path;
        }

        private void initDelagates()
        {
            this.setDbCallBack = new setBoolCallBack(getDbAvailableCallback);
            this.setSiteCallBack = new setBoolCallBack(getSiteAvailableCallback);
        }

        private void FrmMainPlate_Load(object sender, EventArgs e)
        {
            this.Text = "狙击龙头 " + Common.getApplicationVersion();

            this.initDelagates();

            if (!this.isLogin)
            {
                this.showLoginDialog();
            }
            else
            {
                this.checkServerState();
                // 开始
                this.timer1.Start();

                this.checkDbState();
                this.timer2.Start();

            }

            this.setFormState();
        }

        private void showLoginDialog()
        {
            FrmComLogin frmLogin = new FrmComLogin();
            frmLogin.ShowDialog();

            if (frmLogin.DialogResult == DialogResult.OK)
            {
                this.UserId = frmLogin.UserId;
                this.UserToken = frmLogin.UserToken;
                this.isLogin = true;
                this.setFormState();

                this.checkServerState();
                // 开始
                this.timer1.Start();

                this.checkDbState();

                this.timer2.Start();

                // 打开所有窗口
                this.openAllWindow();
                // 重新排列所有窗口
                this.resizeAllWindow();

            }
            else if (frmLogin.DialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("请先登录");
                this.showLoginDialog();
                this.isLogin = false;
            }
            frmLogin.Close();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出吗？");

            if (dr == DialogResult.OK)
            {
                this.closeAllOpenedWindow();
                this.Close();
            }
            else if (dr == DialogResult.Cancel)
            {

            }
        }

        private void openAllWindow()
        {

            if (this.frmPopupStockInfoWeb == null)
            {
                FrmPopupStockInfoWeb frmPopupStockInfoWeb = new FrmPopupStockInfoWeb();
                this.frmPopupStockInfoWeb = frmPopupStockInfoWeb;
                frmPopupStockInfoWeb.MdiParent = this;

                this.frmPopupStockInfoWeb.Show();
                if (this.开盘啦个股ToolStripMenuItem.Checked)
                {
                    this.frmPopupStockInfoWeb.WindowState = FormWindowState.Normal;
                    //this.frmPopupStockInfo.WindowState = FormWindowState.Minimized;
                }
            }

            if (this.frmPopupStockInfo == null)
            {
                FrmPopupStockInfo frmPopupStockInfo = new FrmPopupStockInfo();
                this.frmPopupStockInfo = frmPopupStockInfo;
                frmPopupStockInfo.MdiParent = this;

                this.frmPopupStockInfoWeb.Show();

                if (this.新浪个股ToolStripMenuItem.Checked)
                {
                    //this.frmPopupStockInfoWeb.WindowState = FormWindowState.Minimized;
                    this.frmPopupStockInfo.WindowState = FormWindowState.Normal;
                }

            }

            if (this.frmPlate == null)
            {
                FrmPlate frmPlate = new FrmPlate();
                frmPlate.MdiParent = this;
                this.frmPlate = frmPlate;
                this.frmPlate.Show();
            }
            else
            {
                this.frmPlate.Show();
            }

            if (this.frmPlateStockList == null)
            {
                FrmPlateStockList frmPlateStockList = new FrmPlateStockList();
                this.frmPlateStockList = frmPlateStockList;
                frmPlateStockList.MdiParent = this;
                this.frmPlateStockList.Show();

            }
            else
            {
                this.frmPlateStockList.Show();
            }



            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateQuant == null)
                {
                    FrmPlateQuant frmPlateQuant = new FrmPlateQuant();
                    this.frmPlateQuant = frmPlateQuant;
                    frmPlateQuant.MdiParent = this;
                    this.frmPlateQuant.Show();

                }
                else
                {
                    this.frmPlateQuant.Show();
                }

                if (this.frmPlateSignal == null)
                {
                    FrmPlateSignal frmPlateSignal = new FrmPlateSignal();
                    this.frmPlateSignal = frmPlateSignal;
                    frmPlateSignal.MdiParent = this;
                    this.frmPlateSignal.Show();

                }
                else
                {
                    this.frmPlateSignal.Show();
                }

                if (this.frmPlateAnalyse == null)
                {
                    FrmPlateAnalyse frmPlateAnalyse = new FrmPlateAnalyse();
                    this.frmPlateAnalyse = frmPlateAnalyse;
                    frmPlateAnalyse.MdiParent = this;
                    this.frmPlateAnalyse.Show();

                }
                else
                {
                    this.frmPlateAnalyse.Show();
                }

            }


        }


        private void bringAllWindowToNormal()
        {
            if (this.frmPlateStockList != null)
            {
                this.frmPlateStockList.WindowState = FormWindowState.Normal;
            }
            if (this.frmPlate != null)
            {
                this.frmPlate.WindowState = FormWindowState.Normal;
            }

            if (Common.getIsLightWeightMode())
            {

                //轻量模式，不需要处理
            }
            {
                if (this.frmPlateQuant != null)
                {
                    this.frmPlateQuant.WindowState = FormWindowState.Normal;
                }
                if (this.frmPlateAnalyse != null)
                {
                    this.frmPlateAnalyse.WindowState = FormWindowState.Normal;
                }
                if (this.frmPlateSignal != null)
                {
                    this.frmPlateSignal.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void resizeAllWindow()
        {

            if (this.frmPlate != null)
            {
                this.frmPlate.performSizeChanged(this.Size);
            }

            if (this.frmPlateStockList != null)
            {
                this.frmPlateStockList.performSizeChanged(this.Size);
            }

            if (this.frmPopupStockInfoWeb != null)
            {
                this.frmPopupStockInfoWeb.performSizeChanged(this.Size);
            }


            if (Common.getIsLightWeightMode())
            {

                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateQuant != null)
                {
                    this.frmPlateQuant.performSizeChanged(this.Size);
                }

                if (this.frmPlateSignal != null)
                {
                    this.frmPlateSignal.performSizeChanged(this.Size);
                }

                if (this.frmPlateAnalyse != null)
                {
                    this.frmPlateAnalyse.performSizeChanged(this.Size);
                }
            }
        }

        private void closeAllOpenedWindow()
        {
            if (this.frmPlate == null)
            {
            }
            else
            {
                this.frmPlate.Close();
                this.frmPlate = null;
            }

            if (this.frmPlateStockList == null)
            {
            }
            else
            {
                this.frmPlateStockList.Close();
                this.frmPlateStockList = null;
            }

            if (this.frmPopupStockInfo == null)
            {
            }
            else
            {
                this.frmPopupStockInfo.Close();
                this.frmPopupStockInfo = null;
            }

            if (this.frmPopupStockInfoWeb == null)
            {
            }
            else
            {
                this.frmPopupStockInfoWeb.Close();
                this.frmPopupStockInfoWeb = null;
            }

            if (Common.getIsLightWeightMode())
            {

                //轻量模式，不需要处理
            }
            else
            {

                if (this.frmPlateQuant == null)
                {
                }
                else
                {
                    this.frmPlateQuant.Close();
                    this.frmPlateQuant = null;
                }

                if (this.frmPlateAnalyse == null)
                {
                }
                else
                {
                    this.frmPlateAnalyse.Close();
                    this.frmPlateAnalyse = null;
                }

                if (this.frmPlateSignal == null)
                {
                }
                else
                {
                    this.frmPlateSignal.Close();
                    this.frmPlateSignal = null;
                }
            }
        }

        private void 重新登录LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定重新登录吗？");

            if (dr == DialogResult.OK)
            {
                this.closeAllOpenedWindow();
                this.showLoginDialog();
            }
            else if (dr == DialogResult.Cancel)
            {

            }
        }

        private void 重新排列窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bringAllWindowToNormal();
            this.resizeAllWindow();
        }


        private void 数据分析窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmPlate == null || this.frmPlate.IsDisposed)
            {
                FrmPlate frmPlate = new FrmPlate();
                this.frmPlate = frmPlate;

                frmPlate.MdiParent = this;
                frmPlate.Show();
            }
            frmPlate.BringToFront();
        }


        private void 信号窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmPlate == null || this.frmPlate.IsDisposed)
            {
                FrmPlate frmPlate = new FrmPlate();
                this.frmPlate = frmPlate;

                frmPlate.MdiParent = this;
                frmPlate.Show();
            }
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {

                FrmPlateStockList frmPlateStockList = new FrmPlateStockList();
                this.frmPlateStockList = frmPlateStockList;

                frmPlateStockList.MdiParent = this;
                frmPlateStockList.Show();
            }
            frmPlateStockList.BringToFront();
        }

        private void 量化信号窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (this.frmPlate == null || this.frmPlate.IsDisposed)
            {
                FrmPlate frmPlate = new FrmPlate();
                this.frmPlate = frmPlate;

                frmPlate.MdiParent = this;
                frmPlate.Show();
            }
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {

                FrmPlateStockList frmPlateStockList = new FrmPlateStockList();
                this.frmPlateStockList = frmPlateStockList;

                frmPlateStockList.MdiParent = this;
                frmPlateStockList.Show();
            }
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateAnalyse == null || this.frmPlateAnalyse.IsDisposed)
                {

                    FrmPlateAnalyse frmPlateAnalyse = new FrmPlateAnalyse();
                    this.frmPlateAnalyse = frmPlateAnalyse;

                    frmPlateAnalyse.MdiParent = this;
                    frmPlateAnalyse.Show();
                }

                if (this.frmPlateSignal == null || this.frmPlateSignal.IsDisposed)
                {

                    FrmPlateSignal frmPlateSignal = new FrmPlateSignal();
                    this.frmPlateSignal = frmPlateSignal;

                    frmPlateSignal.MdiParent = this;
                    frmPlateSignal.Show();
                }

                frmPlateSignal.BringToFront();
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            if (this.frmPlate == null || this.frmPlate.IsDisposed)
            {
                FrmPlate frmPlate = new FrmPlate();
                this.frmPlate = frmPlate;

                frmPlate.MdiParent = this;
                frmPlate.Show();
            }
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {

                FrmPlateStockList frmPlateStockList = new FrmPlateStockList();
                this.frmPlateStockList = frmPlateStockList;

                frmPlateStockList.MdiParent = this;
                frmPlateStockList.Show();
            }
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {

                if (this.frmPlateAnalyse == null || this.frmPlateAnalyse.IsDisposed)
                {

                    FrmPlateAnalyse frmPlateAnalyse = new FrmPlateAnalyse();
                    this.frmPlateAnalyse = frmPlateAnalyse;

                    frmPlateAnalyse.MdiParent = this;
                    frmPlateAnalyse.Show();
                }
                if (this.frmPlateSignal == null || this.frmPlateSignal.IsDisposed)
                {

                    FrmPlateSignal frmPlateSignal = new FrmPlateSignal();
                    this.frmPlateSignal = frmPlateSignal;

                    frmPlateSignal.MdiParent = this;
                    frmPlateSignal.Show();
                }
                if (this.frmPlateQuant == null || this.frmPlateQuant.IsDisposed)
                {

                    FrmPlateQuant frmStockQuant = new FrmPlateQuant();
                    this.frmPlateQuant = frmStockQuant;

                    frmStockQuant.MdiParent = this;
                    frmStockQuant.Show();
                }

                frmPlateQuant.BringToFront();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {



            if (this.frmPlate == null || this.frmPlate.IsDisposed)
            {
                FrmPlate frmPlate = new FrmPlate();
                this.frmPlate = frmPlate;

                frmPlate.MdiParent = this;
                frmPlate.Show();
            }
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {

                FrmPlateStockList frmPlateStockList = new FrmPlateStockList();
                this.frmPlateStockList = frmPlateStockList;

                frmPlateStockList.MdiParent = this;
                frmPlateStockList.Show();
            }

            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateAnalyse == null || this.frmPlateAnalyse.IsDisposed)
                {

                    FrmPlateAnalyse frmPlateAnalyse = new FrmPlateAnalyse();
                    this.frmPlateAnalyse = frmPlateAnalyse;

                    frmPlateAnalyse.MdiParent = this;
                    frmPlateAnalyse.Show();
                }

                frmPlateAnalyse.BringToFront();
            }


        }

        private bool remoteSiteAvailable(string url)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                logger.Info("请求远程站点状态获取开始");
                re = (HttpWebRequest)WebRequest.Create(url);
                re.Method = "GET";
                using (res = (HttpWebResponse)re.GetResponse())
                {
                    logger.Info("请求远程站点状态获取完成，状态OK");
                    if (res.ContentLength != 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Warn("请求远程站点状态获取失败", ex);
                return false;
            }
            return false;
        }


        private bool databaseAvailable(string connstr)
        {
            using (SqlConnection sqlConn = new SqlConnection(connstr))
            {
                try
                {
                    logger.Info("Sqlserver服务器状态获取开始");
                    sqlConn.Open();

                    using (SqlCommand cmd = sqlConn.CreateCommand())
                    {
                        cmd.CommandText = "select GETDATE()";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            logger.Info("Sqlserver服务器状态获取完成");
                            if (reader.Read())
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("Sqlserver服务器状态获取失败", ex);
                    return false;
                }
                finally
                {
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        sqlConn.Close();
                    }
                }
                return false;
            }

        }


        //定义回调
        private delegate void setBoolCallBack(bool value);
        //声明回调
        private setBoolCallBack setSiteCallBack;
        private setBoolCallBack setDbCallBack;


        private void runnableGetSiteAvailable()
        {
            bool serAvailable = this.remoteSiteAvailable(Common.GetAppSettingByKey("SiteAvailableUrl"));
            Invoke(setSiteCallBack, serAvailable);
        }

        private void runnableGetDatabaseAvailable()
        {
            bool dbAvailable = databaseAvailable(Common.GetDatabaseConnectString());
            Invoke(setDbCallBack, dbAvailable);
        }

        private void getSiteAvailableCallback(bool value)
        {
            if (value)
            {
                this.isServiceOK = true;
                this.toolStripStatusLabel2.Text = "OK";
            }
            else
            {
                this.isServiceOK = false;
                this.toolStripStatusLabel2.Text = "不可用";
            }
        }


        private void getDbAvailableCallback(bool value)
        {
            if (value)
            {
                this.isDatabaseOK = true;
                Common.DATABASE_CHECK_STATE = true;
                this.toolStripStatusLabel6.Text = "OK";
            }
            else
            {
                this.isDatabaseOK = false;
                Common.DATABASE_CHECK_STATE = false;
                this.toolStripStatusLabel6.Text = "不可用";
            }
        }

        private void setFormState()
        {
            if (this.isLogin)
            {
                this.toolStripStatusLabel4.Text = this.UserId;
            }
            else
            {
                this.toolStripStatusLabel4.Text = "未登录";
            }

            this.toolStripStatusLabel8.Text = String.Format("{0}-{1}", Common.GetAppSettingByKey("BusinessStartTime"), Common.GetAppSettingByKey("BusinessEndTime"));

        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Common.isBusinessTime(DateTime.Now))
            {
                this.checkDbState();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Common.isBusinessTime(DateTime.Now))
            {
                this.checkServerState();
            }
        }

        private void checkDbState()
        {
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要检查数据库
                this.toolStripStatusLabel6.Text = "轻量模式";
            }
            else
            {
                Thread thread2 = new Thread(new ThreadStart(runnableGetDatabaseAvailable));
                thread2.IsBackground = true;
                thread2.Start();
            }
        }


        private void checkServerState()
        {
            Thread thread1 = new Thread(new ThreadStart(runnableGetSiteAvailable));
            thread1.IsBackground = true;
            thread1.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DateTime dtnow = DateTime.Now;
            this.toolStripStatusLabel10.Text = dtnow.ToLongTimeString().ToString();//13:21:25

        }

        public void handlePlateListData(Dictionary<string, PlateParam> plates, DateTime updateTime)
        {
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {
            }
            else
            {
                frmPlateStockList.setupPlateListData(plates, updateTime);
            }
        }

        public void handlePlateListSelectChanged(string code, DateTime updateTime)
        {
            if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
            {
            }
            else
            {
                frmPlateStockList.setupPlateListSelectChanged(code, updateTime);
            }

            //轻量模式，不需要处理
            if (this.frmPopupStockInfoWeb == null || this.frmPopupStockInfoWeb.IsDisposed)
            {
                FrmPopupStockInfoWeb frmPopupStockInfoWeb = new FrmPopupStockInfoWeb();
                this.frmPopupStockInfoWeb = frmPopupStockInfoWeb;
                frmPopupStockInfoWeb.MdiParent = this;
            }
            if (this.frmPopupStockInfoWeb.WindowState == FormWindowState.Normal)
            {
                this.frmPopupStockInfoWeb.Size = new Size(this.frmPlate.Size.Width, this.Size.Height - 290 - 115);
                this.frmPopupStockInfoWeb.Show();
                this.frmPopupStockInfoWeb.Location = new Point(0, 290);
                this.frmPopupStockInfoWeb.BringToFront();
                this.frmPopupStockInfoWeb.setupSymbol(code, null);
            }

            if (Common.getIsLightWeightMode())
            {

            }
            else
            {
                if (this.frmPlateSignal == null || this.frmPlateSignal.IsDisposed)
                {
                }
                else
                {
                    frmPlateSignal.setupPlateListSelectChanged(code, updateTime);
                }
            }

        }

        public void handlePlateStockListData(DataTable dt, Double QDFilter, DateTime updateTime)
        {

            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateAnalyse == null || this.frmPlateAnalyse.IsDisposed)
                {
                }
                else
                {
                    frmPlateAnalyse.setupPlateStockList(dt, QDFilter, updateTime);
                }
            }
        }

        public void handlePlateStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateSignal == null || this.frmPlateSignal.IsDisposed)
                {
                }
                else
                {
                    frmPlateSignal.setupPlateStockAnalyseData(dt, updateTime);
                }
            }
        }

        public void handleFilterdPlateStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateSignal == null || this.frmPlateSignal.IsDisposed)
                {
                }
                else
                {
                    frmPlateSignal.setupFilterdPlateStockAnalyseData(dt, updateTime);
                }
            }

        }

        public void handleJointSignalData(DataTable dt, DateTime updateTime)
        {
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateQuant == null || this.frmPlateQuant.IsDisposed)
                {
                }
                else
                {
                    frmPlateQuant.setupJointQuantData(dt, updateTime);
                }
            }

        }


        public void handleJointSignalDataForStockList(List<string> list, DateTime updateTime)
        {
            if (Common.getIsLightWeightMode())
            {
                //轻量模式，不需要处理
            }
            else
            {
                if (this.frmPlateStockList == null || this.frmPlateStockList.IsDisposed)
                {
                }
                else
                {
                    frmPlateStockList.setupJointStockListData(list, updateTime);
                }
            }
        }

        public void handleStockListDataGridViewCellDoubleClick(string code = "", string url = null)
        {
            if (this.新浪个股ToolStripMenuItem.Checked)
            {
                if (this.frmPopupStockInfo == null || this.frmPopupStockInfo.IsDisposed)
                {
                    FrmPopupStockInfo frmPopupStockInfo = new FrmPopupStockInfo();
                    this.frmPopupStockInfo = frmPopupStockInfo;
                    frmPopupStockInfo.MdiParent = this;
                }

                this.frmPopupStockInfo.Size = new Size(566, 640);

                this.frmPopupStockInfo.Show();
                int x = this.frmPlateStockList.Location.X - this.frmPopupStockInfo.Size.Width;
                int y = 290;
                this.frmPopupStockInfo.Location = new Point(x, y);


                this.frmPopupStockInfo.BringToFront();

                this.frmPopupStockInfo.setupStockCode(code, new Point());
            }
            if (this.开盘啦个股ToolStripMenuItem.Checked)
            {
                if (this.frmPopupStockInfoWeb == null || this.frmPopupStockInfoWeb.IsDisposed)
                {
                    FrmPopupStockInfoWeb frmPopupStockInfoWeb = new FrmPopupStockInfoWeb();
                    this.frmPopupStockInfoWeb = frmPopupStockInfoWeb;
                    frmPopupStockInfoWeb.MdiParent = this;
                }

                this.frmPopupStockInfoWeb.Size = new Size(this.frmPlate.Size.Width, this.Size.Height - 290 - 115);

                this.frmPopupStockInfoWeb.Show();
                this.frmPopupStockInfoWeb.Location = new Point(0, 290);


                this.frmPopupStockInfoWeb.BringToFront();

                this.frmPopupStockInfoWeb.setupSymbol(code, url);
            }

        }

        public void handleStockListDataGridViewCellLeave(string code, PointF point)
        {
            if (this.新浪个股ToolStripMenuItem.Checked)
            {
                if (this.frmPopupStockInfo == null || this.frmPopupStockInfo.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfo.resetStockCode();
                    this.frmPopupStockInfo.Hide();
                }
            }
            if (this.开盘啦个股ToolStripMenuItem.Checked)
            {
                if (this.frmPopupStockInfoWeb == null || this.frmPopupStockInfoWeb.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfoWeb.resetSymbol();
                    this.frmPopupStockInfoWeb.Hide();
                }
            }
        }

        private void FrmMainPlate_SizeChanged(object sender, EventArgs e)
        {
            this.resizeAllWindow();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void 开盘啦个股ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!item.Checked)
            {
                item.Checked = true;
                this.新浪个股ToolStripMenuItem.Checked = false;

                if (this.frmPopupStockInfoWeb == null || this.frmPopupStockInfoWeb.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfoWeb.WindowState = FormWindowState.Normal;
                }

                if (this.frmPopupStockInfo == null || this.frmPopupStockInfo.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfo.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void 新浪个股ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!item.Checked)
            {
                item.Checked = true;
                this.开盘啦个股ToolStripMenuItem.Checked = false;

                if (this.frmPopupStockInfoWeb == null || this.frmPopupStockInfoWeb.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfoWeb.WindowState = FormWindowState.Minimized;
                }

                if (this.frmPopupStockInfo == null || this.frmPopupStockInfo.IsDisposed)
                {
                }
                else
                {
                    this.frmPopupStockInfo.WindowState = FormWindowState.Normal;
                }
            }

        }

        private void 窗口WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Common.getIsLightWeightMode())
            {
                this.toolStripMenuItem1.Enabled = false;
                this.toolStripMenuItem2.Enabled = false;
                this.量化信号窗口ToolStripMenuItem.Enabled = false;
            }
        }
    }
}
