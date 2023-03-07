using KaiPanLaCommon;
using System;
using System.Windows.Forms;

namespace KaiPanLaPortfolio
{
    public partial class FrmPortfolioSignalCond : Form
    {

        public PortSignalFilterCondition signalFilter1 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 0,
            SJLTP_TO = 15,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 520,
            BUY = 1200
        };
        public PortSignalFilterCondition signalFilter2 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 15,
            SJLTP_TO = 100,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = true,
            ZLJE = 400,
            BUY = 900
        };
        public PortSignalFilterCondition signalFilter3 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 100,
            SJLTP_TO = 150,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 3100,
            BUY = 4500
        };
        public PortSignalFilterCondition signalFilter4 = new PortSignalFilterCondition()
        {
            enabled = true,
            SJLTP_FROM = 150,
            SJLTP_TO = 99999,
            RATE_FROM = 1,
            RATE_TO = 6,
            SJLTP_CEFF = false,
            ZLJE = 3300,
            BUY = 4900
        };

        public FrmPortfolioSignalCond()
        {
            InitializeComponent();
        }


        private void initControls()
        {
            this.checkBox1.Checked = this.signalFilter1.enabled;
            this.textBox23.Text = this.signalFilter1.SJLTP_FROM.ToString();
            this.textBox21.Text = this.signalFilter1.SJLTP_TO.ToString();
            this.textBox24.Text = this.signalFilter1.RATE_FROM.ToString();
            this.textBox22.Text = this.signalFilter1.RATE_TO.ToString();
            this.checkBox5.Checked = this.signalFilter1.SJLTP_CEFF;
            this.textBox3.Text = this.signalFilter1.BUY.ToString();
            this.textBox4.Text = this.signalFilter1.ZLJE.ToString();

            this.checkBox2.Checked = this.signalFilter2.enabled;
            this.textBox20.Text = this.signalFilter2.SJLTP_FROM.ToString();
            this.textBox9.Text = this.signalFilter2.SJLTP_TO.ToString();
            this.textBox19.Text = this.signalFilter2.RATE_FROM.ToString();
            this.textBox10.Text = this.signalFilter2.RATE_TO.ToString();
            this.checkBox6.Checked = this.signalFilter2.SJLTP_CEFF;
            this.textBox7.Text = this.signalFilter2.BUY.ToString();
            this.textBox8.Text = this.signalFilter2.ZLJE.ToString();

            this.checkBox3.Checked = this.signalFilter3.enabled;
            this.textBox5.Text = this.signalFilter3.SJLTP_FROM.ToString();
            this.textBox1.Text = this.signalFilter3.SJLTP_TO.ToString();
            this.textBox6.Text = this.signalFilter3.RATE_FROM.ToString();
            this.textBox2.Text = this.signalFilter3.RATE_TO.ToString();
            this.checkBox7.Checked = this.signalFilter3.SJLTP_CEFF;
            this.textBox11.Text = this.signalFilter3.BUY.ToString();
            this.textBox12.Text = this.signalFilter3.ZLJE.ToString();

            this.checkBox4.Checked = this.signalFilter4.enabled;
            this.textBox13.Text = this.signalFilter4.SJLTP_FROM.ToString();
            this.textBox18.Text = this.signalFilter4.SJLTP_TO.ToString();
            this.textBox14.Text = this.signalFilter4.RATE_FROM.ToString();
            this.textBox17.Text = this.signalFilter4.RATE_TO.ToString();
            this.checkBox8.Checked = this.signalFilter4.SJLTP_CEFF;
            this.textBox15.Text = this.signalFilter4.BUY.ToString();
            this.textBox16.Text = this.signalFilter4.ZLJE.ToString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            TextBox targetTxt = (TextBox)sender;
            Int64 value;
            if (Int64.TryParse(targetTxt.Text, out value))
            {
                targetTxt.Text = value.ToString();
            }
            else
            {
                MessageBox.Show("请输入正确的数值");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter1.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox21.Enabled = true;
                this.textBox22.Enabled = true;
                this.textBox23.Enabled = true;
                this.textBox24.Enabled = true;
                this.textBox3.Enabled = true;
                this.textBox4.Enabled = true;
                this.checkBox5.Enabled = true;
            }
            else
            {
                this.textBox21.Enabled = false;
                this.textBox22.Enabled = false;
                this.textBox23.Enabled = false;
                this.textBox24.Enabled = false;
                this.textBox3.Enabled = false;
                this.textBox4.Enabled = false;
                this.checkBox5.Enabled = false;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter2.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox20.Enabled = true;
                this.textBox9.Enabled = true;
                this.textBox19.Enabled = true;
                this.textBox10.Enabled = true;
                this.textBox7.Enabled = true;
                this.textBox8.Enabled = true;
                this.checkBox6.Enabled = true;
            }
            else
            {
                this.textBox20.Enabled = false;
                this.textBox9.Enabled = false;
                this.textBox19.Enabled = false;
                this.textBox10.Enabled = false;
                this.textBox7.Enabled = false;
                this.textBox8.Enabled = false;
                this.checkBox6.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter3.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox5.Enabled = true;
                this.textBox1.Enabled = true;
                this.textBox6.Enabled = true;
                this.textBox2.Enabled = true;
                this.textBox11.Enabled = true;
                this.textBox12.Enabled = true;
                this.checkBox7.Enabled = true;
            }
            else
            {
                this.textBox5.Enabled = false;
                this.textBox1.Enabled = false;
                this.textBox6.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox11.Enabled = false;
                this.textBox12.Enabled = false;
                this.checkBox7.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter4.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox13.Enabled = true;
                this.textBox18.Enabled = true;
                this.textBox17.Enabled = true;
                this.textBox14.Enabled = true;
                this.textBox15.Enabled = true;
                this.textBox16.Enabled = true;
                this.checkBox8.Enabled = true;
            }
            else
            {
                this.textBox13.Enabled = false;
                this.textBox18.Enabled = false;
                this.textBox14.Enabled = false;
                this.textBox17.Enabled = false;
                this.textBox15.Enabled = false;
                this.textBox16.Enabled = false;
                this.checkBox8.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.signalFilter1.enabled = this.checkBox1.Checked;
            this.signalFilter1.SJLTP_FROM = Int64.Parse(this.textBox23.Text.Trim());
            this.signalFilter1.SJLTP_TO = Int64.Parse(this.textBox21.Text.Trim());
            this.signalFilter1.RATE_FROM = Int64.Parse(this.textBox24.Text.Trim());
            this.signalFilter1.RATE_TO = Int64.Parse(this.textBox22.Text.Trim());
            this.signalFilter1.SJLTP_CEFF = this.checkBox5.Checked;
            this.signalFilter1.BUY = Int64.Parse(this.textBox3.Text.Trim());
            this.signalFilter1.ZLJE = Int64.Parse(this.textBox4.Text.Trim());

            this.signalFilter2.enabled = this.checkBox2.Checked;
            this.signalFilter2.SJLTP_FROM = Int64.Parse(this.textBox20.Text.Trim());
            this.signalFilter2.SJLTP_TO = Int64.Parse(this.textBox9.Text.Trim());
            this.signalFilter2.RATE_FROM = Int64.Parse(this.textBox19.Text.Trim());
            this.signalFilter2.RATE_TO = Int64.Parse(this.textBox10.Text.Trim());
            this.signalFilter2.SJLTP_CEFF = this.checkBox6.Checked;
            this.signalFilter2.BUY = Int64.Parse(this.textBox7.Text.Trim());
            this.signalFilter2.ZLJE = Int64.Parse(this.textBox8.Text.Trim());

            this.signalFilter3.enabled = this.checkBox3.Checked;
            this.signalFilter3.SJLTP_FROM = Int64.Parse(this.textBox5.Text.Trim());
            this.signalFilter3.SJLTP_TO = Int64.Parse(this.textBox1.Text.Trim());
            this.signalFilter3.RATE_FROM = Int64.Parse(this.textBox6.Text.Trim());
            this.signalFilter3.RATE_TO = Int64.Parse(this.textBox2.Text.Trim());
            this.signalFilter3.SJLTP_CEFF = this.checkBox7.Checked;
            this.signalFilter3.BUY = Int64.Parse(this.textBox11.Text.Trim());
            this.signalFilter3.ZLJE = Int64.Parse(this.textBox12.Text.Trim());

            this.signalFilter4.enabled = this.checkBox4.Checked;
            this.signalFilter4.SJLTP_FROM = Int64.Parse(this.textBox13.Text.Trim());
            this.signalFilter4.SJLTP_TO = Int64.Parse(this.textBox18.Text.Trim());
            this.signalFilter4.RATE_FROM = Int64.Parse(this.textBox14.Text.Trim());
            this.signalFilter4.RATE_TO = Int64.Parse(this.textBox17.Text.Trim());
            this.signalFilter4.SJLTP_CEFF = this.checkBox8.Checked;
            this.signalFilter4.BUY = Int64.Parse(this.textBox15.Text.Trim());
            this.signalFilter4.ZLJE = Int64.Parse(this.textBox16.Text.Trim());
        }

        private void FrmSignalFilter_Load(object sender, EventArgs e)
        {

            this.initControls();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.initControls();
        }
    }
}
