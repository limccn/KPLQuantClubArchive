using KaiPanLaCommon;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace KaiPanLaPortfolio
{
    public partial class FrmMainPort : Form
    {
        public bool isLogin = false;
        public bool isServiceOK = false;
        public bool isDatabaseOK = false;

        public Logger logger = Logger._;

        public FrmPortfolioList frmPortfolioList = null;
        public FrmPortfolioSignal frmPortfolioSignal = null;

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

        public FrmMainPort()
        {
            InitializeComponent();
        }

        private void initDelagates()
        {
            this.setDbCallBack = new setBoolCallBack(getDbAvailableCallback);
            this.setSiteCallBack = new setBoolCallBack(getSiteAvailableCallback);
        }


        private void FrmMainPort_Load(object sender, EventArgs e)
        {
            this.Text = "超级自选 " + Common.GetApplicationVersion();

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


        private void openAllWindow()
        {

            if (this.frmPortfolioList == null)
            {
                FrmPortfolioList frmPortfolioList = new FrmPortfolioList();
                frmPortfolioList.MdiParent = this;
                this.frmPortfolioList = frmPortfolioList;
                this.frmPortfolioList.Show();
            }
            else
            {
                this.frmPortfolioList.Show();
            }

            if (this.frmPortfolioSignal == null)
            {
                FrmPortfolioSignal frmPortfolioSignal = new FrmPortfolioSignal();
                frmPortfolioSignal.MdiParent = this;
                this.frmPortfolioSignal = frmPortfolioSignal;
                this.frmPortfolioSignal.Show();
            }
            else
            {
                this.frmPortfolioSignal.Show();
            }
        }

        private void closeAllOpenedWindow()
        {
            if (this.frmPortfolioList == null)
            {
            }
            else
            {
                this.frmPortfolioList.Close();
                this.frmPortfolioList = null;
            }

            if (this.frmPortfolioSignal == null)
            {
            }
            else
            {
                this.frmPortfolioSignal.Close();
                this.frmPortfolioSignal = null;
            }


        }

        private void bringAllWindowToNormal()
        {
            if (this.frmPortfolioList != null)
            {
                this.frmPortfolioList.WindowState = FormWindowState.Normal;
            }

            if (this.frmPortfolioSignal != null)
            {
                this.frmPortfolioSignal.WindowState = FormWindowState.Normal;
            }
        }

        private void resizeAllWindow()
        {
            if (this.frmPortfolioList != null)
            {
                this.frmPortfolioList.performSizeChanged(this.Size);
            }

            if (this.frmPortfolioSignal != null)
            {
                this.frmPortfolioSignal.performSizeChanged(this.Size);
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Common.IsMonitorTime(DateTime.Now))
            {
                this.checkServerState();
            }
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Common.IsMonitorTime(DateTime.Now))
            {
                this.checkDbState();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DateTime dtnow = DateTime.Now;
            this.toolStripStatusLabel10.Text = dtnow.ToLongTimeString().ToString();//13:21:25

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


        private void 重新排列窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bringAllWindowToNormal();
            this.resizeAllWindow();
        }

        private void FrmMainPort_SizeChanged(object sender, EventArgs e)
        {
            this.resizeAllWindow();
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

            this.toolStripStatusLabel8.Text = String.Format("{0}-{1}", Common.GetAppSettingByKey("MonitorStartTime"), Common.GetAppSettingByKey("MonitorEndTime"));

        }

        private void 自选股窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmPortfolioList == null || this.frmPortfolioList.IsDisposed)
            {
                FrmPortfolioList frmPortfolioList = new FrmPortfolioList();
                this.frmPortfolioList = frmPortfolioList;

                frmPortfolioList.MdiParent = this;
                frmPortfolioList.Show();
            }
            this.frmPortfolioList.BringToFront();
        }

        private void 信号窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frmPortfolioSignal == null || this.frmPortfolioSignal.IsDisposed)
            {
                FrmPortfolioSignal frmPortfolioSignal = new FrmPortfolioSignal();
                this.frmPortfolioSignal = frmPortfolioSignal;

                frmPortfolioSignal.MdiParent = this;
                frmPortfolioSignal.Show();
            }
            this.frmPortfolioSignal.BringToFront();
        }


        public void handlePortfolioData(DataTable dt, DateTime updateTime)
        {
            if (this.frmPortfolioSignal == null || this.frmPortfolioSignal.IsDisposed)
            {
            }
            else
            {
                frmPortfolioSignal.setupPortfolioData(dt, updateTime);
            }
        }
    }
}
