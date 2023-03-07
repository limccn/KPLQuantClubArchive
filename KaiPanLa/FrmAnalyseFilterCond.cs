using System;
using System.Windows.Forms;

namespace KaiPanLa
{
    public partial class FrmAnalyseFilterCond : Form
    {
        public double dZLMDY { get; set; } = 100;
        public double dJEYZ { get; set; } = 2000;
        public double dLTSZ { get; set; } = 10;
        public double dLTSZYZ { get; set; } = 30;
        public double dJEZHDY { get; set; } = 406;
        public double dJEZHYZ { get; set; } = 400;
        public double dJEJSL { get; set; } = 5;

        public FrmAnalyseFilterCond()
        {
            InitializeComponent();
        }

        private void initControl()
        {
            this.textBox1.Text = this.dZLMDY.ToString();
            this.textBox2.Text = this.dJEYZ.ToString();
            this.textBox3.Text = this.dLTSZ.ToString();
            this.textBox4.Text = this.dJEZHDY.ToString();
            this.textBox5.Text = this.dJEZHYZ.ToString();
            this.textBox6.Text = this.dJEJSL.ToString();
            this.textBox7.Text = this.dLTSZYZ.ToString();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str1 = this.textBox1.Text.Trim();
            if (String.IsNullOrEmpty(str1))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str1) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str2 = this.textBox2.Text.Trim();
            if (String.IsNullOrEmpty(str2))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str2) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str3 = this.textBox3.Text.Trim();
            if (String.IsNullOrEmpty(str3))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str3) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str4 = this.textBox4.Text.Trim();
            if (String.IsNullOrEmpty(str4))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str4) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str5 = this.textBox5.Text.Trim();
            if (String.IsNullOrEmpty(str5))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str5) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str6 = this.textBox6.Text.Trim();
            if (String.IsNullOrEmpty(str6))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str6) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            string str7 = this.textBox7.Text.Trim();
            if (String.IsNullOrEmpty(str7))
            {
                MessageBox.Show("请输入正确的数据");
                return;
            }
            if (Double.Parse(str7) <= 0)
            {
                MessageBox.Show("请输入正确的数值");
                return;
            }

            this.dZLMDY = Double.Parse(str1);
            this.dJEYZ = Double.Parse(str2);
            this.dLTSZ = Double.Parse(str3);
            this.dJEZHDY = Double.Parse(str4);
            this.dJEZHYZ = Double.Parse(str5);
            this.dJEJSL = Double.Parse(str6);
            this.dLTSZYZ = Double.Parse(str6);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.initControl();
        }

        private void FrmFilterCondition_Load(object sender, EventArgs e)
        {
            this.initControl();
        }

    }
}
