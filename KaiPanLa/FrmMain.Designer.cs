namespace KaiPanLa
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新登录LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗口WToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实时排行数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.数据分析窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.信号窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.量化信号窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.重新排列窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8,
            this.toolStripStatusLabel9,
            this.toolStripStatusLabel10});
            this.statusStrip1.Location = new System.Drawing.Point(0, 517);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(939, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "远程服务状态：";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel2.Text = "等待确认";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel5.Text = "数据库状态：";
            this.toolStripStatusLabel5.Click += new System.EventHandler(this.toolStripStatusLabel5_Click_1);
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel6.Text = "等待确认";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel3.Text = "当前用户：";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel4.Text = "未登录";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel7.Text = "业务时间带：";
            this.toolStripStatusLabel7.Click += new System.EventHandler(this.toolStripStatusLabel7_Click);
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(75, 17);
            this.toolStripStatusLabel8.Text = "09:30-09:45";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel9.Text = "系统时间：";
            this.toolStripStatusLabel9.Click += new System.EventHandler(this.toolStripStatusLabel9_Click);
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel10.Text = "08:01:01";
            this.toolStripStatusLabel10.Click += new System.EventHandler(this.toolStripStatusLabel10_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.窗口WToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(939, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重新登录LToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出XToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            this.文件FToolStripMenuItem.Click += new System.EventHandler(this.文件FToolStripMenuItem_Click);
            // 
            // 重新登录LToolStripMenuItem
            // 
            this.重新登录LToolStripMenuItem.Name = "重新登录LToolStripMenuItem";
            this.重新登录LToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.重新登录LToolStripMenuItem.Text = "重新登录(&L)";
            this.重新登录LToolStripMenuItem.Click += new System.EventHandler(this.重新登录LToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // 窗口WToolStripMenuItem
            // 
            this.窗口WToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.实时排行数据ToolStripMenuItem,
            this.toolStripSeparator2,
            this.数据分析窗口ToolStripMenuItem,
            this.信号窗口ToolStripMenuItem,
            this.量化信号窗口ToolStripMenuItem,
            this.toolStripSeparator3,
            this.重新排列窗口ToolStripMenuItem});
            this.窗口WToolStripMenuItem.Name = "窗口WToolStripMenuItem";
            this.窗口WToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.窗口WToolStripMenuItem.Text = "窗口(&W)";
            // 
            // 实时排行数据ToolStripMenuItem
            // 
            this.实时排行数据ToolStripMenuItem.Name = "实时排行数据ToolStripMenuItem";
            this.实时排行数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.实时排行数据ToolStripMenuItem.Text = "数据采集窗口";
            this.实时排行数据ToolStripMenuItem.Click += new System.EventHandler(this.实时排行数据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // 数据分析窗口ToolStripMenuItem
            // 
            this.数据分析窗口ToolStripMenuItem.Name = "数据分析窗口ToolStripMenuItem";
            this.数据分析窗口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.数据分析窗口ToolStripMenuItem.Text = "数据分析窗口";
            this.数据分析窗口ToolStripMenuItem.Click += new System.EventHandler(this.数据分析窗口ToolStripMenuItem_Click);
            // 
            // 信号窗口ToolStripMenuItem
            // 
            this.信号窗口ToolStripMenuItem.Name = "信号窗口ToolStripMenuItem";
            this.信号窗口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.信号窗口ToolStripMenuItem.Text = "信号窗口";
            this.信号窗口ToolStripMenuItem.Click += new System.EventHandler(this.信号窗口ToolStripMenuItem_Click);
            // 
            // 量化信号窗口ToolStripMenuItem
            // 
            this.量化信号窗口ToolStripMenuItem.Name = "量化信号窗口ToolStripMenuItem";
            this.量化信号窗口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.量化信号窗口ToolStripMenuItem.Text = "量化信号窗口";
            this.量化信号窗口ToolStripMenuItem.Click += new System.EventHandler(this.量化信号窗口ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(145, 6);
            // 
            // 重新排列窗口ToolStripMenuItem
            // 
            this.重新排列窗口ToolStripMenuItem.Name = "重新排列窗口ToolStripMenuItem";
            this.重新排列窗口ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.重新排列窗口ToolStripMenuItem.Text = "重新排列窗口";
            this.重新排列窗口ToolStripMenuItem.Click += new System.EventHandler(this.重新排列窗口ToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 539);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主窗口";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 窗口WToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新登录LToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 实时排行数据ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 数据分析窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 信号窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 重新排列窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 量化信号窗口ToolStripMenuItem;
    }
}