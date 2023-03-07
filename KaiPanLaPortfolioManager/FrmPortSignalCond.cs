using System;
using System.Windows.Forms;

namespace KaiPanLaPortfolioManager
{
    public partial class FrmPortSignalCond : Form
    {

        public string strCode { get; set; } = "";
        public string strName { get; set; } = "";
        public string strPrice { get; set; } = "";
        public string strRate { get; set; } = "";
        public string strTag { get; set; } = "";

        public FrmPortSignalCond()
        {
            InitializeComponent();
        }

        private void initControls()
        {
            this.label1.Text = this.strCode;
            this.label2.Text = this.strName;
            this.label3.Text = this.strPrice;
            this.label4.Text = String.Format("{0}%", this.strRate);

            if (this.strTag.Equals("开盘核按钮") || this.strTag.Equals(""))
            {
                this.radioButton1.Checked = true;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;

            }
            else if (this.strTag.Equals("保留观望"))
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = true;
                this.radioButton3.Checked = false;

            }
            else if (this.strTag.Equals("涨停监测"))
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = true;

            }
            else
            {
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
                this.radioButton3.Checked = false;
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 关闭对话框
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.initControls();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.strTag = "开盘核按钮";
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.strTag = "保留观望";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.strTag = "涨停监测";
            }
        }

        private void FrmPortSignalCond_Load(object sender, EventArgs e)
        {
            this.initControls();
        }


    }
}
