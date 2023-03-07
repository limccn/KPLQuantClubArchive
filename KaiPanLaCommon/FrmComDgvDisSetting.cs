using System;
using System.Drawing;
using System.Windows.Forms;

namespace KaiPanLaCommon
{
    public partial class FrmComDgvDisSetting : Form
    {
        public DataGridView dgvData;

        public FrmComDgvDisSetting()
        {
            InitializeComponent();
        }


        private void FrmAnaDisSetting_Load(object sender, EventArgs e)
        {
            int index = 0;
            foreach (DataGridViewColumn column in dgvData.Columns)
            {
                if (column.HeaderText.EndsWith("_"))
                {

                }
                else
                {
                    this.addCheckBoxForColumn(column, index);
                    index = index + 1;
                    int cols = index / 10 + 1;

                    // 调整宽度
                    this.Size = new Size(120 * cols + 30, this.Size.Height);

                }
            }
        }


        private void addCheckBoxForColumn(DataGridViewColumn column, int index)
        {
            int col = index / 10;
            int row = index % 10;

            CheckBox chkItem = new CheckBox();
            chkItem.AutoSize = true;
            chkItem.Location = new Point(12 + col * 120, 12 + row * 30);
            chkItem.Size = new System.Drawing.Size(60, 20);
            chkItem.TabIndex = column.Index;
            chkItem.Tag = column.Index;
            chkItem.Text = column.HeaderText;
            chkItem.Checked = column.Visible;
            chkItem.UseVisualStyleBackColor = true;

            chkItem.CheckedChanged += new System.EventHandler(this.checkBoxItem_CheckedChanged);

            this.Controls.Add(chkItem);
        }

        private void checkBoxItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkItem = (CheckBox)sender;
            int colIndex = chkItem.TabIndex;
            this.dgvData.Columns[colIndex].Visible = chkItem.Checked;
        }
    }
}
