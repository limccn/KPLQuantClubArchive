using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace KaiPanLaPlate
{
    public partial class FrmPopupStockInfo : Form
    {
        public string stockCode { get; set; }

        public void setupStockCode(string code, Point location)
        {
            this.stockCode = code;

            this.pictureBox1.Image = KaiPanLaPlate.Properties.Resources.loading;
            this.pictureBox1.Image = GetImageByCode(code, "min");

            this.pictureBox2.Image = KaiPanLaPlate.Properties.Resources.loading;
            this.pictureBox2.Image = GetImageByCode(code, "daily");
        }

        public void resetStockCode()
        {
            this.stockCode = "";

            this.pictureBox1.Image = KaiPanLaPlate.Properties.Resources.loading;
            this.pictureBox2.Image = KaiPanLaPlate.Properties.Resources.loading;
        }

        public FrmPopupStockInfo()
        {
            InitializeComponent();
        }

        public Image GetImageByCode(string code, string type)
        {
            string urlformat = "http://image.sinajs.cn/newchart/{2}/n/{0}{1}.gif";

            if (code.StartsWith("30") || code.StartsWith("00"))
            {
                return this.GetImage(String.Format(urlformat, "sz", code, type));

            }
            else if (code.StartsWith("60") || code.StartsWith("68"))
            {
                return this.GetImage(String.Format(urlformat, "sh", code, type));
            }
            else
            {
                return KaiPanLaPlate.Properties.Resources.not_found;
            }
        }

        public Image GetImage(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                return KaiPanLaPlate.Properties.Resources.not_found;
            }
            try
            {

                WebRequest webreq = WebRequest.Create(path);
                WebResponse webres = webreq.GetResponse();
                using (Stream stream = webres.GetResponseStream())
                {
                    Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(stream);
                    //返回
                    return bmp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                return KaiPanLaPlate.Properties.Resources.not_found;
            }
        }

        private void FrmPopupStockInfo_Load(object sender, EventArgs e)
        {
            this.Size = new Size(566, 640);
            this.pictureBox1.Size = new Size(550, 300);
            this.pictureBox1.Location = new Point(0, 0);

            this.pictureBox2.Size = new Size(550, 300);
            this.pictureBox2.Location = new Point(0, 301);

        }
    }
}
