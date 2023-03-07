using KaiPanLaCommon;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace KaiPanLaManager
{
    public partial class Form1 : Form
    {
        public Logger logger = Logger._;

        public string appid = "";
        public string subType = "";
        public string nickName = "";
        public bool hideEmptyAvatar = true;
        public bool hideExpire = true;


        public DataTable dtDatas;


        public Form1()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }


        private DataTable loadDataFromDB()
        {

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT " +
                                  "ROW_NUMBER() OVER(ORDER BY T2.[SUB_EXPIRE] DESC) as NO" +
                                  ",T2.[ID] " +
                                  ",T1.[APPID] " +
                                  ",T1.[OPENID]" +
                                  ",T1.[NICK_NAME]" +
                                  ",T1.[GENDER]" +
                                  ",T1.[CITY]" +
                                  ",T1.[PROVINCE]" +
                                  ",T1.[COUNTRY]" +
                                  ",T1.[MOBILE]" +
                                  ",T1.[CREATE_TIME]" +
                                  ",T1.[AVATAR_URL]" +
                                  ",T2.[SUB_TYPE]" +
                                  ",DATEADD(S,T2.[SUB_EXPIRE],'1970-01-01 00:00:00') as [SUB_EXPIRE] " +
                                " FROM " + Common.GetUserInfoTableName() + " as T1 " +
                                " JOIN " + Common.GetSubsccribeTableName() + " as T2 " +
                                " ON T1.[APPID] = T2.[APPID] " +
                                " AND T1.[OPENID] = T2.[OPENID] " +
                                " WHERE 1=1";
                if (!String.IsNullOrEmpty(this.appid.Trim()))
                {
                    selectString = selectString + " AND T1.[APPID] = @APPID ";
                }
                if (!String.IsNullOrEmpty(this.subType.Trim()))
                {
                    selectString = selectString + " AND T2.[SUB_TYPE] = @SUB_TYPE ";
                }
                if (!String.IsNullOrEmpty(this.nickName.Trim()))
                {
                    selectString = selectString + " AND T1.[NICK_NAME] like @NICK_NAME ";
                }
                //显示未授权用户
                if (!this.hideEmptyAvatar)
                {
                    selectString = selectString + " AND T1.[AVATAR_URL] != ''";
                }
                //显示过期用户
                if (!this.hideExpire)
                {
                    TimeSpan ts = DateTime.Today - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    string tsToday = Convert.ToInt64(ts.TotalSeconds).ToString();
                    selectString = selectString + " AND T2.[SUB_EXPIRE] >= " + tsToday + " ";
                }


                destinationConnection.Open();
                try
                {
                    logger.Info("查询数据数据开始，表名" + Common.GetUserInfoTableName() + ",表名" + Common.GetSubsccribeTableName());
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    if (!String.IsNullOrEmpty(this.appid.Trim()))
                    {
                        cmdSelect.Parameters.Add(new SqlParameter("@APPID", this.appid));
                    }
                    if (!String.IsNullOrEmpty(this.subType.Trim()))
                    {
                        cmdSelect.Parameters.Add(new SqlParameter("@SUB_TYPE", this.subType));
                    }
                    if (!String.IsNullOrEmpty(this.nickName.Trim()))
                    {
                        cmdSelect.Parameters.Add(new SqlParameter("@NICK_NAME", "%" + this.nickName + "%"));
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    logger.Info("查询数据数据完成，表名" + Common.GetUserInfoTableName() + ",表名" + Common.GetSubsccribeTableName());
                    return dt;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("查询数据失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
                return null;
            }

        }


        private int updateUserSubscribe(string expire, string type, string appid, string openid)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectUpdate = "UPDATE " + Common.GetSubsccribeTableName() + " SET " +
                                " [SUB_EXPIRE] = @SUB_EXPIRE ," +
                                " [SUB_TYPE] = @SUB_TYPE" +
                                " WHERE 1=1" +
                                " AND [APPID] = @APPID " +
                                " AND [OPENID] = @OPENID ";

                destinationConnection.Open();
                SqlTransaction transaction = destinationConnection.BeginTransaction();
                try
                {
                    logger.Info("更新数据开始,表名" + Common.GetSubsccribeTableName());
                    SqlCommand cmdUpdate = new SqlCommand(selectUpdate, destinationConnection, transaction);
                    cmdUpdate.Parameters.Add(new SqlParameter("@SUB_EXPIRE", expire));
                    cmdUpdate.Parameters.Add(new SqlParameter("@SUB_TYPE", type));
                    cmdUpdate.Parameters.Add(new SqlParameter("@APPID", appid));
                    cmdUpdate.Parameters.Add(new SqlParameter("@OPENID", openid));

                    logger.Info("更新已有数据：EXPIRE:" + expire + " APPID:" + appid + " OPENID:" + openid);
                    int result = cmdUpdate.ExecuteNonQuery();
                    if (result == 1)
                    {
                        logger.Info("更新数据成功");
                        transaction.Commit();

                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    logger.Warn("更新数据失败", ex);
                }
                finally
                {
                    if (destinationConnection.State == ConnectionState.Open)
                    {
                        destinationConnection.Close();
                    }
                }
                return -1;
            }

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void InitControl()
        {
            string[] appids = Common.GetManagedAppIDs();
            this.toolStripComboBox1.Items.Clear();
            this.toolStripComboBox1.Items.AddRange(appids);
            this.toolStripComboBox1.SelectedItem = 0;
            this.toolStripComboBox1.Text = appids[0].Trim();

            this.appid = appids[0].Split('|')[1];

            string[] subTypes = Common.GetManagedSubType();
            this.toolStripComboBox2.Items.Clear();
            this.toolStripComboBox2.Items.AddRange(subTypes);
            this.toolStripComboBox2.Text = "";

            this.subType = "";


            if (this.hideEmptyAvatar)
            {
                this.hideEmptyAvatar = false;
                this.toolStripButton1.Text = "显示未授权用户";
            }
            else
            {
                this.hideEmptyAvatar = true;
                this.toolStripButton1.Text = "隐藏未授权用户";
            }

            if (this.hideExpire)
            {
                this.hideExpire = false;
                this.toolStripButton2.Text = "显示过期用户";
            }
            else
            {
                this.hideExpire = true;
                this.toolStripButton2.Text = "隐藏过期用户";
            }

            this.dataGridView1.AutoGenerateColumns = false;
        }

        private void reLoadDataGridView()
        {
            DataTable dtDatas = this.loadDataFromDB();
            this.dataGridView1.DataSource = dtDatas;
            if (dtDatas != null)
            {
                this.toolStripStatusLabel2.Text = dtDatas.Rows.Count.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitControl();
            this.reLoadDataGridView();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strOptName = this.toolStripComboBox1.SelectedItem.ToString().Trim();
            this.appid = strOptName.Split('|')[1];

            this.reLoadDataGridView();
        }


        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strOptName = this.toolStripComboBox2.SelectedItem.ToString().Trim();
            this.subType = strOptName.Split('|')[1];

            this.reLoadDataGridView();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < this.dataGridView1.Rows.Count)
            {
                DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];
                try
                {
                    string value = dgrSingle.Cells["colExpire"].Value.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        DateTime expire = DateTime.Parse(value);
                        // 信号确认时间不为空
                        if (DateTime.Compare(DateTime.Now, expire) >= 0)
                        {
                            if (DateTime.Compare(DateTime.Today, expire) >= 0)
                            {
                                dgrSingle.Cells["colExpire"].Style.ForeColor = Color.Red;
                            }
                            else
                            {
                                dgrSingle.Cells["colExpire"].Style.ForeColor = Color.Orange;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Warn("数据转换失败", ex);
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colAvatar"))
            {
                if (e.Value != null)
                {
                    string path = e.Value.ToString();
                    e.Value = GetImage(path);
                }
            }
            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colGender"))
            {
                if (e.Value != null)
                {
                    string value = e.Value.ToString();
                    e.Value = this.convertGender(value);
                }

            }
            if (this.dataGridView1.Columns[e.ColumnIndex].Name.Equals("colSubType"))
            {
                if (e.Value != null)
                {
                    string value = e.Value.ToString();
                    e.Value = this.convertSub(value);
                }
            }
        }

        public System.Drawing.Image GetImage(string path)
        {

            if (string.IsNullOrEmpty(path))
            {
                return KaiPanLaManager.Properties.Resources.empty_head;
            }
            try
            {
                Image result = this.GetCachedImage(path);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    WebRequest webreq = WebRequest.Create(path);
                    WebResponse webres = webreq.GetResponse();
                    using (Stream stream = webres.GetResponseStream())
                    {
                        Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(stream);
                        this.SaveImageCache(path, bmp);

                        //返回
                        return bmp;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.Message);
                return KaiPanLaManager.Properties.Resources.empty_head;
            }
        }

        public string base64FilePath(string path)
        {
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();//创建SHA1对象
            byte[] bytes_in = Encoding.Default.GetBytes(path);//将待加密字符串转为byte类型
            byte[] bytes_out = sha1.ComputeHash(bytes_in);//Hash运算
            sha1.Dispose();//释放当前实例使用的所有资源
            string base64_path = Convert.ToBase64String(bytes_out);
            base64_path = base64_path.Replace("/", "@");

            return base64_path;
        }

        public string getAppBaseDir()
        {
            string base_path = "";
            if (System.Environment.CurrentDirectory == AppDomain.CurrentDomain.BaseDirectory)//Windows应用程序则相等  
            {
                base_path = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                base_path = AppDomain.CurrentDomain.BaseDirectory + "Bin";
            }
            return base_path;
        }

        public void SaveImageCache(string path, Bitmap bmp)
        {
            string base_path = this.getAppBaseDir();
            string cache_path = base_path + @"\Cache";
            // 判断路径
            if (false == System.IO.Directory.Exists(cache_path))
            {
                //不存在则创建文件夹
                Directory.CreateDirectory(cache_path);
            }
            string file_path = cache_path + @"\" + this.base64FilePath(path) + ".png";

            //文件存在则返回
            if (System.IO.File.Exists(file_path))
            {
                return;
            }
            else
            {
                bmp.Save(file_path, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public System.Drawing.Image GetCachedImage(string path)
        {
            string base_path = this.getAppBaseDir();
            string cache_path = base_path + @"\Cache";
            // 判断路径
            if (false == System.IO.Directory.Exists(cache_path))
            {
                return null;
            }
            string file_path = cache_path + @"\" + this.base64FilePath(path) + ".png";

            //文件存在则返回
            if (System.IO.File.Exists(file_path))
            {
                return Image.FromFile(file_path);
            }
            return null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (this.hideEmptyAvatar)
            {
                this.hideEmptyAvatar = false;
                this.toolStripButton1.Text = "显示未授权用户";
            }
            else
            {
                this.hideEmptyAvatar = true;
                this.toolStripButton1.Text = "隐藏未授权用户";
            }

            this.reLoadDataGridView();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < this.dataGridView1.Rows.Count)
            {
                DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];

                FrmItemEdit frmItemEdit = new FrmItemEdit();

                frmItemEdit.nickName = this.NullIsEmpty(dgrSingle.Cells["colNick"].Value);
                frmItemEdit.gender = this.convertGender(this.NullIsEmpty(dgrSingle.Cells["colGender"].Value));
                frmItemEdit.subType = this.convertSub(this.NullIsEmpty(dgrSingle.Cells["colSubType"].Value));
                frmItemEdit.mobile = this.NullIsEmpty(dgrSingle.Cells["colMobile"].Value);
                frmItemEdit.country = this.NullIsEmpty(dgrSingle.Cells["colCountry"].Value);
                frmItemEdit.province = this.NullIsEmpty(dgrSingle.Cells["colProvince"].Value);
                frmItemEdit.city = this.NullIsEmpty(dgrSingle.Cells["colCity"].Value);
                frmItemEdit.openid = this.NullIsEmpty(dgrSingle.Cells["colOpenId"].Value);
                frmItemEdit.expire = this.NullIsEmpty(dgrSingle.Cells["colExpire"].Value);

                frmItemEdit.avatar = this.GetImage(this.NullIsEmpty(dgrSingle.Cells["colAvatar"].Value));

                frmItemEdit.ShowDialog();

                if (frmItemEdit.DialogResult == DialogResult.OK)
                {
                    string parsedExpire = frmItemEdit.parsedExpire;
                    string parsedSubType = frmItemEdit.parsedSubType;
                    string openid = frmItemEdit.openid;

                    int result = this.updateUserSubscribe(parsedExpire, parsedSubType, this.appid, openid);

                    DialogResult dr;
                    if (result == 1)
                    {
                        dr = MessageBox.Show("更改用户订阅信息成功，点击确定刷新数据。");
                    }
                    else
                    {
                        dr = MessageBox.Show("更改用户订阅信息失败，点击确定刷新数据");
                    }

                    if (dr == DialogResult.OK)
                    {
                        this.reLoadDataGridView();
                    }

                }
                else if (frmItemEdit.DialogResult == DialogResult.Cancel)
                {
                }
                frmItemEdit.Close();
            }
        }
        public string NullIsEmpty(object o)
        {
            if (o == null)
            {
                return "";
            }
            else
            {
                return o.ToString();
            }
        }

        public string convertGender(string value)
        {

            if (value.Equals("1"))
            {
                return "男";
            }
            else if (value.Equals("2"))
            {
                return "女";
            }
            else if (value.Equals("0"))
            {
                return "未知";
            }
            else
            {
                return "";
            }
        }

        public string convertSub(string value)
        {
            string[] subTypes = Common.GetManagedSubType();
            // 查找
            for (int i = 0; i < subTypes.Length; i++)
            {
                string[] keyPair = subTypes[i].Split('|');
                if (value.ToUpper().Equals(keyPair[1].ToUpper()))
                {
                    return keyPair[0];
                }
            }
            return value;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            this.nickName = this.toolStripTextBox1.Text.Trim();
            this.reLoadDataGridView();
        }



        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //如果是enter
            if (e.KeyCode == Keys.Enter)
            {
                this.nickName = this.toolStripTextBox1.Text.Trim();
                this.reLoadDataGridView();
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出吗？");

            if (dr == DialogResult.OK)
            {
                this.Close();
            }
            else if (dr == DialogResult.Cancel)
            {

            }
        }

        private void 搜索SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripTextBox1.Focus();
        }

        private void toolStripComboBox2_Leave(object sender, EventArgs e)
        {
            //只在清空时触发
            if (String.IsNullOrEmpty(this.toolStripComboBox2.Text.Trim()))
            {
                this.subType = "";
                this.reLoadDataGridView();
            }
        }

        private void toolStripComboBox1_Leave(object sender, EventArgs e)
        {
            //只在清空时触发
            if (String.IsNullOrEmpty(this.toolStripComboBox1.Text.Trim()))
            {
                this.appid = "";
                this.reLoadDataGridView();
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.hideExpire)
            {
                this.hideExpire = false;
                this.toolStripButton2.Text = "显示过期用户";
            }
            else
            {
                this.hideExpire = true;
                this.toolStripButton2.Text = "隐藏过期用户";
            }

            this.reLoadDataGridView();
        }
    }

}
