using KaiPanLaCommon;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace KaiPanLaPortfolioManager
{
    public partial class FrmPortMgrMain : Form
    {
        public Logger logger = Logger._;


        private string QueryDate = DateTime.Today.ToString("yyyyMMdd");
        private string SearchString = "";

        public FrmPortMgrMain()
        {
            InitializeComponent();
        }

        private DataTable loadDataFromDB()
        {

            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectString = "SELECT " +
                                  "ROW_NUMBER() OVER(ORDER BY [CREATE_TIME]) as NO" +
                                  ",[ID]" +
                                  ",[DATE]" +
                                  ",[TIME]" +
                                  ",[XHGXSJ]" +
                                  ",[XHQRSJ]" +
                                  ",[CODE]" +
                                  ",[NAME]" +
                                  ",[RATE]" +
                                  ",[PRICE]" +
                                  ",[CJE]" +
                                  ",[RATIO]" +
                                  ",[SPEED]" +
                                  ",[SJLTP]" +
                                  ",[TUDE]" +
                                  ",[BUY]" +
                                  ",[SELL]" +
                                  ",[ZLJE]" +
                                  ",[QJZF]" +
                                  ",[TAG]" +
                                  ",[CREATE_TIME]" +
                                  ",[UPDATE_TIME]" +
                                " FROM " + Common.GetPortfolioSignalTableName() +
                                " WHERE 1=1";
                if (!String.IsNullOrEmpty(this.QueryDate.Trim()))
                {
                    selectString = selectString + " AND [DATE] = @DATE ";
                }
                if (!String.IsNullOrEmpty(this.SearchString.Trim()))
                {
                    selectString = selectString + " AND [NAME] like @NAME ";
                }

                selectString = selectString + " ORDER BY [CREATE_TIME] ";

                destinationConnection.Open();
                try
                {
                    logger.Info("查询数据数据开始，表名" + Common.GetPortfolioSignalTableName());
                    SqlCommand cmdSelect = new SqlCommand(selectString, destinationConnection);
                    if (!String.IsNullOrEmpty(this.QueryDate.Trim()))
                    {
                        cmdSelect.Parameters.Add(new SqlParameter("@DATE", this.QueryDate));
                    }
                    if (!String.IsNullOrEmpty(this.SearchString.Trim()))
                    {
                        cmdSelect.Parameters.Add(new SqlParameter("@NAME", "%" + this.SearchString + "%"));
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    logger.Info("查询数据数据完成，表名" + Common.GetPortfolioSignalTableName());
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


        private int updateDataTag(string date, string code, string tag)
        {
            string connectionString = Common.GetDatabaseConnectString();

            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                String selectUpdate = "UPDATE " + Common.GetPortfolioSignalTableName() + " SET " +
                                " [TAG] = @TAG ," +
                                " [UPDATE_TIME] = (getdate())" +
                                " WHERE 1=1" +
                                " AND [DATE] = @DATE " +
                                " AND [CODE] = @CODE ";

                destinationConnection.Open();
                SqlTransaction transaction = destinationConnection.BeginTransaction();
                try
                {
                    logger.Info("更新数据开始,表名" + Common.GetPortfolioSignalTableName());
                    SqlCommand cmdUpdate = new SqlCommand(selectUpdate, destinationConnection, transaction);
                    cmdUpdate.Parameters.Add(new SqlParameter("@TAG", tag));
                    cmdUpdate.Parameters.Add(new SqlParameter("@DATE", date));
                    cmdUpdate.Parameters.Add(new SqlParameter("@CODE", code));

                    logger.Info("更新已有数据：DATE:" + date + " CODE:" + code + " TAG:" + tag);
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

        private void InitControls()
        {
            this.toolStripTextBox2.Text = DateTime.ParseExact(this.QueryDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
            this.toolStripTextBox1.Text = this.SearchString;

            this.panel1.Visible = false;
        }

        private void FrmPortMgrMain_Load(object sender, EventArgs e)
        {
            this.InitControls();
            this.reLoadDataGridView();
        }

        private void reLoadDataGridView()
        {
            DataTable dtDatas = this.loadDataFromDB();
            this.dataGridView1.DataSource = dtDatas;
            if (dtDatas != null)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.ParseExact(this.QueryDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime newDate = current.AddDays(-1);

            this.QueryDate = newDate.ToString("yyyyMMdd");
            this.toolStripTextBox2.Text = newDate.ToString("yyyy-MM-dd");

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.ParseExact(this.QueryDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime newDate = current.AddDays(1);

            this.QueryDate = newDate.ToString("yyyyMMdd");
            this.toolStripTextBox2.Text = newDate.ToString("yyyy-MM-dd");

        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FrmComDgvDisSetting frmAnaDisSetting = new FrmComDgvDisSetting();
            frmAnaDisSetting.dgvData = this.dataGridView1;
            frmAnaDisSetting.ShowDialog();
            if (frmAnaDisSetting.DialogResult == DialogResult.OK)
            {

            }
            else if (frmAnaDisSetting.DialogResult == DialogResult.Cancel)
            {
            }
            frmAnaDisSetting.Close();
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < this.dataGridView1.Rows.Count)
            {
                DataGridViewRow dgrSingle = this.dataGridView1.Rows[e.RowIndex];

                FrmPortSignalCond frmItemEdit = new FrmPortSignalCond();

                frmItemEdit.strCode = this.NullIsEmpty(dgrSingle.Cells["colCode"].Value);
                frmItemEdit.strName = this.NullIsEmpty(dgrSingle.Cells["colName"].Value);
                frmItemEdit.strPrice = this.NullIsEmpty(dgrSingle.Cells["colPrice"].Value);
                frmItemEdit.strRate = this.NullIsEmpty(dgrSingle.Cells["colRate"].Value);
                frmItemEdit.strTag = this.NullIsEmpty(dgrSingle.Cells["colTag"].Value);

                frmItemEdit.ShowDialog();

                if (frmItemEdit.DialogResult == DialogResult.OK)
                {
                    string strTag = frmItemEdit.strTag;
                    string strCode = frmItemEdit.strCode;
                    string strDate = this.QueryDate.Replace("-", "");

                    int result = this.updateDataTag(strDate, strCode, strTag);

                    DialogResult dr;
                    if (result == 1)
                    {
                        dr = MessageBox.Show("更新策略成功。");
                    }
                    else
                    {
                        dr = MessageBox.Show("更新策略失败");
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

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            this.QueryDate = DateTime.ParseExact(this.toolStripTextBox2.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyyMMdd");
            this.reLoadDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = this.monthCalendar1.SelectionEnd;
            this.QueryDate = dt.ToString("yyyyMMdd");
            this.toolStripTextBox2.Text = dt.ToString("yyyy-MM-dd");

            this.panel1.Visible = false;
            this.toolStripTextBox2.Enabled = true;

        }

        private void toolStripTextBox2_Enter(object sender, EventArgs e)
        {
            this.panel1.Visible = true;
            this.panel1.Size = new Size(220, 230);
            this.panel1.Location = new Point(145, 55);

            DateTime selectedDate = DateTime.ParseExact(this.QueryDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            this.monthCalendar1.SelectionStart = selectedDate;
            this.monthCalendar1.SelectionEnd = selectedDate;

            this.toolStripTextBox2.Enabled = false;
        }

        private void toolStripTextBox2_Leave(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime dt = this.monthCalendar1.SelectionEnd;
            this.QueryDate = dt.ToString("yyyyMMdd");
            this.toolStripTextBox2.Text = dt.ToString("yyyy-MM-dd");

            //this.panel1.Visible = false;
            //this.toolStripTextBox2.Enabled = true;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime dt = this.monthCalendar1.SelectionEnd;
            this.QueryDate = dt.ToString("yyyyMMdd");
            this.toolStripTextBox2.Text = dt.ToString("yyyy-MM-dd");

            //this.panel1.Visible = false;
            //this.toolStripTextBox2.Enabled = true;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            this.SearchString = this.toolStripTextBox1.Text.Trim();
            this.reLoadDataGridView();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.reLoadDataGridView();
        }
    }
}
