using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KaiPanLaManager
{
    public partial class FrmItemEdit : Form
    {

        public Image avatar { get; set; } = null;
        public string nickName { get; set; } = "";
        public string gender { get; set; } = "";
        public string subType { get; set; } = "";

        public string mobile { get; set; } = "";
        public string country { get; set; } = "";
        public string province { get; set; } = "";
        public string city { get; set; } = "";
        public string expire { get; set; } = "";
        public string openid { get; set; } = "";


        public string parsedExpire { get; set; } = "";
        public string parsedSubType { get; set; } = "";


        public FrmItemEdit()
        {
            InitializeComponent();
        }

        public void InitContrl()
        {
            this.picAvatar.Image = this.avatar;

            this.lblCity.Text = this.city;
            this.lblProvince.Text = this.province;
            this.lblCountry.Text = this.country;
            this.lblGender.Text = this.gender;
            this.lblMobile.Text = this.mobile;
            this.lblNickName.Text = this.nickName;

            this.txtOpenId.Text = this.openid;
            this.txtExpire.Text = this.expire;
            this.txtExpireTo.Text = this.expire;

            string[] subTypes = Common.GetManagedSubType();
            this.cmbSubTypeTo.Items.Clear();
            this.cmbSubTypeTo.Items.AddRange(subTypes);

            this.cmbSubTypeTo.SelectedItem = 0;

            for (int i = 0; i < subTypes.Length; i++)
            {
                string[] keyPair = subTypes[i].Split('|');
                if (this.subType.Equals(keyPair[0]))
                {
                    this.lblSubType.Text = subTypes[i];
                    this.cmbSubTypeTo.Text = subTypes[i];
                }
            }
        }

        private void FrmItemEdit_Load(object sender, EventArgs e)
        {
            this.InitContrl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //处理订阅日期
            try
            {
                bool checkresult = false;
                string value = this.txtExpireTo.Text.Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime expireTo = DateTime.Parse(value);
                    if (DateTime.Compare(DateTime.Now, expireTo) >= 0)
                    {
                        MessageBox.Show("请输入正确的有效日期，有效期必须大于当前时间");
                        this.txtExpireTo.Focus();
                    }
                    else
                    {

                        TimeSpan ts = expireTo - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        this.parsedExpire = Convert.ToInt64(ts.TotalSeconds).ToString();
                        checkresult = true;

                    }
                }
                else
                {
                    MessageBox.Show("请输入有效日期");
                    this.txtExpireTo.Focus();

                }

                //处理订阅类型
                string plainSubType = this.cmbSubTypeTo.Text.Trim();
                if (plainSubType.Contains('|'))
                {
                    string[] keyPair = plainSubType.Split('|');
                    if (keyPair.Length > 1)
                    {

                        if (keyPair[1].Trim().Length > 0)
                        {
                            this.parsedSubType = keyPair[1].Trim();
                            checkresult = true;
                        }
                        else
                        {
                            MessageBox.Show("用户类型数据不正确");
                        }
                    }
                    else
                    {
                        MessageBox.Show("用户类型数据不正确");
                        this.cmbSubTypeTo.Focus();
                    }
                }
                else
                {
                    if (plainSubType.Length > 10)
                    {
                        MessageBox.Show("用户类型长度不要超过10个字符");
                        this.cmbSubTypeTo.Focus();
                    }
                    else
                    {
                        this.parsedSubType = plainSubType;
                        checkresult = true;
                    }
                }

                if (checkresult)
                {
                    // 关闭对话框
                    this.DialogResult = DialogResult.OK;
                }
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                MessageBox.Show("请输入正确的数据");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.InitContrl();
        }
    }
}
