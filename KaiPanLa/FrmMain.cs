using KaiPanLaCommon;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace KaiPanLa
{
    public partial class FrmMain : Form
    {

        public bool isLogin = false;
        public bool isServiceOK = false;
        public bool isDatabaseOK = false;

        public Logger logger = Logger._;

        public FrmRankList frmRankList = null;
        public FrmAnalyse frmAnlayse = null;
        public FrmSignal frmSignal = null;
        public FrmSignalQuant frmSignalQuant = null;

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

        public FrmMain()
        {
            InitializeComponent();
        }

        private void initDelagates()
        {
            this.setDbCallBack = new setBoolCallBack(getDbAvailableCallback);
            this.setSiteCallBack = new setBoolCallBack(getSiteAvailableCallback);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
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

            if (this.frmRankList == null)
            {
                FrmRankList frmRankList = new FrmRankList();
                frmRankList.MdiParent = this;
                this.frmRankList = frmRankList;
                this.frmRankList.Show();
            }
            else
            {
                this.frmRankList.Show();
            }

            if (this.frmAnlayse == null)
            {
                FrmAnalyse frmAnlayse = new FrmAnalyse();
                this.frmAnlayse = frmAnlayse;
                frmAnlayse.MdiParent = this;
                this.frmAnlayse.Show();

            }
            else
            {
                this.frmAnlayse.Show();
            }

            if (this.frmSignal == null)
            {
                FrmSignal frmSignal = new FrmSignal();
                this.frmSignal = frmSignal;
                frmSignal.MdiParent = this;
                this.frmSignal.Show();

            }
            else
            {
                this.frmSignal.Show();
            }

            if (this.frmSignalQuant == null)
            {
                FrmSignalQuant frmSignalQuant = new FrmSignalQuant();
                this.frmSignalQuant = frmSignalQuant;
                frmSignalQuant.MdiParent = this;
                this.frmSignalQuant.Show();

            }
            else
            {
                this.frmSignalQuant.Show();
            }

        }


        private void bringAllWindowToNormal()
        {
            if (this.frmAnlayse != null)
            {
                this.frmAnlayse.WindowState = FormWindowState.Normal;
            }
            if (this.frmRankList != null)
            {
                this.frmRankList.WindowState = FormWindowState.Normal;
            }
            if (this.frmSignal != null)
            {
                this.frmSignal.WindowState = FormWindowState.Normal;
            }
            if (this.frmSignalQuant != null)
            {
                this.frmSignalQuant.WindowState = FormWindowState.Normal;
            }
        }

        private void resizeAllWindow()
        {
            if (this.frmAnlayse != null)
            {
                this.frmAnlayse.performSizeChanged(this.Size);
            }
            if (this.frmRankList != null)
            {
                this.frmRankList.performSizeChanged(this.Size);
            }
            if (this.frmSignal != null)
            {
                this.frmSignal.performSizeChanged(this.Size);
            }
            if (this.frmSignalQuant != null)
            {
                this.frmSignalQuant.performSizeChanged(this.Size);
            }
        }

        private void closeAllOpenedWindow()
        {
            if (this.frmRankList == null)
            {
            }
            else
            {
                this.frmRankList.Close();
                this.frmRankList = null;
            }

            if (this.frmAnlayse == null)
            {
            }
            else
            {
                this.frmAnlayse.Close();
                this.frmAnlayse = null;
            }

            if (this.frmSignal == null)
            {
            }
            else
            {
                this.frmSignal.Close();
                this.frmSignal = null;
            }

            if (this.frmSignalQuant == null)
            {
            }
            else
            {
                this.frmSignalQuant.Close();
                this.frmSignalQuant = null;
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

        private void toolStripStatusLabel5_Click(object sender, EventArgs e)
        {

        }

        private void 实时排行数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.frmRankList == null || this.frmRankList.IsDisposed)
            {
                FrmRankList frmRankList = new FrmRankList();
                this.frmRankList = frmRankList;

                frmRankList.MdiParent = this;
                frmRankList.Show();
            }
            frmRankList.BringToFront();
        }

        private void 数据分析窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmRankList == null || this.frmRankList.IsDisposed)
            {
                FrmRankList frmRankList = new FrmRankList();
                this.frmRankList = frmRankList;

                frmRankList.MdiParent = this;
                frmRankList.Show();
            }
            if (this.frmAnlayse == null || this.frmAnlayse.IsDisposed)
            {

                FrmAnalyse frmAnlayse = new FrmAnalyse();
                this.frmAnlayse = frmAnlayse;

                frmAnlayse.MdiParent = this;
                frmAnlayse.Show();
            }
            frmAnlayse.BringToFront();
        }

        private void 信号窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmRankList == null || this.frmRankList.IsDisposed)
            {
                FrmRankList frmRankList = new FrmRankList();
                this.frmRankList = frmRankList;

                frmRankList.MdiParent = this;
                frmRankList.Show();
            }
            if (this.frmAnlayse == null || this.frmAnlayse.IsDisposed)
            {

                FrmAnalyse frmAnlayse = new FrmAnalyse();
                this.frmAnlayse = frmAnlayse;

                frmAnlayse.MdiParent = this;
                frmAnlayse.Show();
            }
            if (this.frmSignal == null || this.frmSignal.IsDisposed)
            {

                FrmSignal frmSignal = new FrmSignal();
                this.frmSignal = frmSignal;

                frmSignal.MdiParent = this;
                frmSignal.Show();
            }

            frmSignal.BringToFront();
        }

        private void 量化信号窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmRankList == null || this.frmRankList.IsDisposed)
            {
                FrmRankList frmRankList = new FrmRankList();
                this.frmRankList = frmRankList;

                frmRankList.MdiParent = this;
                frmRankList.Show();
            }
            if (this.frmAnlayse == null || this.frmAnlayse.IsDisposed)
            {

                FrmAnalyse frmAnlayse = new FrmAnalyse();
                this.frmAnlayse = frmAnlayse;

                frmAnlayse.MdiParent = this;
                frmAnlayse.Show();
            }
            if (this.frmSignal == null || this.frmSignal.IsDisposed)
            {

                FrmSignal frmSignal = new FrmSignal();
                this.frmSignal = frmSignal;

                frmSignal.MdiParent = this;
                frmSignal.Show();
            }

            if (this.frmSignalQuant == null || this.frmSignalQuant.IsDisposed)
            {

                FrmSignalQuant frmSignalQuant = new FrmSignalQuant();
                this.frmSignalQuant = frmSignalQuant;

                frmSignalQuant.MdiParent = this;
                frmSignalQuant.Show();
            }

            this.frmSignalQuant.BringToFront();
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
                this.toolStripStatusLabel6.Text = "OK";
            }
            else
            {
                this.isDatabaseOK = false;
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

        private void toolStripStatusLabel5_Click_1(object sender, EventArgs e)
        {

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

            Thread thread2 = new Thread(new ThreadStart(runnableGetDatabaseAvailable));
            thread2.IsBackground = true;
            thread2.Start();
        }


        private void checkServerState()
        {
            Thread thread1 = new Thread(new ThreadStart(runnableGetSiteAvailable));
            thread1.IsBackground = true;
            thread1.Start();
        }

        private void toolStripStatusLabel7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel9_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel10_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DateTime dtnow = DateTime.Now;
            this.toolStripStatusLabel10.Text = dtnow.ToLongTimeString().ToString();//13:21:25

        }

        public void handleStockRankingData(DataTable dt, DateTime updateTime)
        {
            if (this.frmAnlayse == null || this.frmAnlayse.IsDisposed)
            {
            }
            else
            {
                frmAnlayse.setupStockRankingData(dt, updateTime);
            }
        }

        public void handleStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (this.frmSignal == null || this.frmSignal.IsDisposed)
            {
            }
            else
            {
                frmSignal.setupStockAnalyseData(dt, updateTime);
            }
        }

        public void handleFilterdStockAnalyseData(DataTable dt, DateTime updateTime)
        {
            if (this.frmSignal == null || this.frmSignal.IsDisposed)
            {
            }
            else
            {
                frmSignal.setupFilterdStockAnalyseData(dt, updateTime);
            }
        }

        public void handleJointSignalData(DataTable dt, DateTime updateTime)
        {
            if (this.frmSignalQuant == null || this.frmSignalQuant.IsDisposed)
            {
            }
            else
            {
                frmSignalQuant.setupJointQuantData(dt, updateTime);
            }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            this.resizeAllWindow();
        }

        private void 重新排列窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bringAllWindowToNormal();
            this.resizeAllWindow();
        }

    }
}
