using System;
using System.Windows.Forms;

namespace KaiPanLaCommon
{
    public partial class FrmComLogin : Form
    {
        public string UserId
        {
            get; set;
        }

        public string UserToken
        {
            get;
            set;
        }

        public FrmComLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtUserId.Text = "";
            this.txtUserToken.Text = "";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userId = this.txtUserId.Text.Trim();
            if (String.IsNullOrEmpty(userId))
            {
                MessageBox.Show("请输入正确的用户编号");
                return;
            }

            string userToken = this.txtUserToken.Text.Trim();
            if (String.IsNullOrEmpty(userToken))
            {
                MessageBox.Show("请输入正确的用户令牌");
                return;
            }

            this.UserId = userId;
            this.UserToken = userToken;
        }

    }
}
