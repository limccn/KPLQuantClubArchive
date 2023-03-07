using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace KaiPanLaPlate
{
    public partial class FrmPopupStockInfoWeb : Form
    {
        public string symbol { get; set; } = "";

        private ChromiumWebBrowser browser;

        public void performSizeChanged(Size size)
        {
            this.Location = new Point(0, 290);

            this.Width = 885;
            this.Height = size.Height - 290 - 115;
        }

        public void setupSymbol(string code = "", string url = null)
        {
            this.symbol = code;
            this.WindowState = FormWindowState.Normal;
            this.loadUrl(url);
        }

        public void resetSymbol()
        {
            this.symbol = "";
        }

        private void loadUrl(string url)
        {
            if (this.browser == null)
            {
                return;
            }
            if (!this.browser.IsBrowserInitialized)
            {
                return;
            }

            if (this.browser.GetBrowser().IsLoading)
            {
                this.browser.GetBrowser().StopLoad();
            }
            try
            {
                if (url != null)
                {
                    this.browser.LoadUrl(url);
                }
                else
                {
                    this.browser.LoadUrl(this.getUrl());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FrmPopupStockInfoWeb()
        {
            InitializeComponent();

            ChromiumWebBrowser browser = new ChromiumWebBrowser("about:blank");
            browser.Dock = DockStyle.Fill;
            browser.BrowserSettings.LocalStorage = CefState.Enabled;

            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            this.Controls.Add(browser);

            this.browser = browser;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            ChromiumWebBrowser b = (ChromiumWebBrowser)sender;
            if (b.IsBrowserInitialized)
            {
                b.LoadUrl(this.getUrl());
            }

        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            ChromiumWebBrowser b = (ChromiumWebBrowser)sender;
            if (!b.IsLoading)
            {
                b.GetBrowser().SetZoomLevel(-1);
                b.GetBrowser().ExecuteScriptAsync("window.scrollTo(document.body.scrollWidth,10);");
            }
        }

        private void FrmPopupStockInfo_Load(object sender, EventArgs e)
        {

        }

        private string getUrl()
        {
            if (this.symbol.StartsWith("6") || this.symbol.StartsWith("3") || this.symbol.StartsWith("1") || this.symbol.StartsWith("0"))
            {
                return "https://www.kaipanla.com/index.php/stock/index?id=" + this.symbol;
            }
            else if (this.symbol.StartsWith("8"))
            {
                return "https://www.kaipanla.com/index.php/quotes/plate?sid=" + this.symbol;
            }
            else
            {
                return "https://www.kaipanla.com/";
            }
        }

        private void FrmPopupStockInfoWeb_Load(object sender, EventArgs e)
        {
            this.performSizeChanged(this.MdiParent.Size);
        }
    }
}
