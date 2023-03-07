using KaiPanLaCommon;
using System;
using System.Windows.Forms;

namespace KaiPanLa
{
    public partial class FrmSignalFilterCond : Form
    {

        public SignalFilterCondition signalFilter1 = new SignalFilterCondition() { enabled = true, QD = 100, BUY = 1200 };
        public SignalFilterCondition signalFilter2 = new SignalFilterCondition() { enabled = true, QD = 60, ZLJE = 2000 };
        public SignalFilterCondition signalFilter3 = new SignalFilterCondition() { enabled = true, TL = 1200, ZLJE = 2000 };
        public SignalFilterCondition signalFilter4 = new SignalFilterCondition();

        public FrmSignalFilterCond()
        {
            InitializeComponent();
        }


        private void initControls()
        {
            this.checkBox1.Checked = this.signalFilter1.enabled;
            this.textBox1.Text = this.signalFilter1.QD.ToString();
            this.textBox2.Text = this.signalFilter1.TL.ToString();
            this.textBox3.Text = this.signalFilter1.BUY.ToString();
            this.textBox4.Text = this.signalFilter1.ZLJE.ToString();

            this.checkBox2.Checked = this.signalFilter2.enabled;
            this.textBox5.Text = this.signalFilter2.QD.ToString();
            this.textBox6.Text = this.signalFilter2.TL.ToString();
            this.textBox7.Text = this.signalFilter2.BUY.ToString();
            this.textBox8.Text = this.signalFilter2.ZLJE.ToString();

            this.checkBox3.Checked = this.signalFilter3.enabled;
            this.textBox9.Text = this.signalFilter3.QD.ToString();
            this.textBox10.Text = this.signalFilter3.TL.ToString();
            this.textBox11.Text = this.signalFilter3.BUY.ToString();
            this.textBox12.Text = this.signalFilter3.ZLJE.ToString();

            this.checkBox4.Checked = this.signalFilter4.enabled;
            this.textBox13.Text = this.signalFilter4.QD.ToString();
            this.textBox14.Text = this.signalFilter4.TL.ToString();
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
                this.textBox1.Enabled = true;
                this.textBox2.Enabled = true;
                this.textBox3.Enabled = true;
                this.textBox4.Enabled = true;
            }
            else
            {
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox3.Enabled = false;
                this.textBox4.Enabled = false;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter2.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox5.Enabled = true;
                this.textBox6.Enabled = true;
                this.textBox7.Enabled = true;
                this.textBox8.Enabled = true;
            }
            else
            {
                this.textBox5.Enabled = false;
                this.textBox6.Enabled = false;
                this.textBox7.Enabled = false;
                this.textBox8.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter3.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox9.Enabled = true;
                this.textBox10.Enabled = true;
                this.textBox11.Enabled = true;
                this.textBox12.Enabled = true;
            }
            else
            {
                this.textBox9.Enabled = false;
                this.textBox10.Enabled = false;
                this.textBox11.Enabled = false;
                this.textBox12.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            this.signalFilter4.enabled = chkbox.Checked;
            if (chkbox.Checked)
            {
                this.textBox13.Enabled = true;
                this.textBox14.Enabled = true;
                this.textBox15.Enabled = true;
                this.textBox16.Enabled = true;
            }
            else
            {
                this.textBox13.Enabled = false;
                this.textBox14.Enabled = false;
                this.textBox15.Enabled = false;
                this.textBox16.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.signalFilter1.enabled = this.checkBox1.Checked;
            this.signalFilter1.QD = Int64.Parse(this.textBox1.Text.Trim());
            this.signalFilter1.TL = Int64.Parse(this.textBox2.Text.Trim());
            this.signalFilter1.BUY = Int64.Parse(this.textBox3.Text.Trim());
            this.signalFilter1.ZLJE = Int64.Parse(this.textBox4.Text.Trim());

            this.signalFilter2.enabled = this.checkBox2.Checked;
            this.signalFilter2.QD = Int64.Parse(this.textBox5.Text.Trim());
            this.signalFilter2.TL = Int64.Parse(this.textBox6.Text.Trim());
            this.signalFilter2.BUY = Int64.Parse(this.textBox7.Text.Trim());
            this.signalFilter2.ZLJE = Int64.Parse(this.textBox8.Text.Trim());

            this.signalFilter3.enabled = this.checkBox3.Checked;
            this.signalFilter3.QD = Int64.Parse(this.textBox9.Text.Trim());
            this.signalFilter3.TL = Int64.Parse(this.textBox10.Text.Trim());
            this.signalFilter3.BUY = Int64.Parse(this.textBox11.Text.Trim());
            this.signalFilter3.ZLJE = Int64.Parse(this.textBox12.Text.Trim());

            this.signalFilter4.enabled = this.checkBox4.Checked;
            this.signalFilter4.QD = Int64.Parse(this.textBox13.Text.Trim());
            this.signalFilter4.TL = Int64.Parse(this.textBox14.Text.Trim());
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
