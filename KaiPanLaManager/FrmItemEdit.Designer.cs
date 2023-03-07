namespace KaiPanLaManager
{
    partial class FrmItemEdit
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtExpire = new System.Windows.Forms.TextBox();
            this.txtExpireTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMobile = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblProvince = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblNickName = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.txtOpenId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblSubType = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbSubTypeTo = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbSubTypeTo);
            this.groupBox2.Controls.Add(this.lblSubType);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtExpire);
            this.groupBox2.Controls.Add(this.txtExpireTo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(513, 89);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "订阅信息";
            // 
            // txtExpire
            // 
            this.txtExpire.Location = new System.Drawing.Point(85, 19);
            this.txtExpire.Name = "txtExpire";
            this.txtExpire.ReadOnly = true;
            this.txtExpire.Size = new System.Drawing.Size(161, 22);
            this.txtExpire.TabIndex = 17;
            // 
            // txtExpireTo
            // 
            this.txtExpireTo.Location = new System.Drawing.Point(334, 19);
            this.txtExpireTo.Name = "txtExpireTo";
            this.txtExpireTo.Size = new System.Drawing.Size(157, 22);
            this.txtExpireTo.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "调整为：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "有效期：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMobile);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lblGender);
            this.groupBox1.Controls.Add(this.lblCity);
            this.groupBox1.Controls.Add(this.lblProvince);
            this.groupBox1.Controls.Add(this.lblCountry);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblNickName);
            this.groupBox1.Controls.Add(this.picAvatar);
            this.groupBox1.Controls.Add(this.txtOpenId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(513, 174);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户信息";
            // 
            // lblMobile
            // 
            this.lblMobile.AutoSize = true;
            this.lblMobile.Location = new System.Drawing.Point(288, 72);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(52, 16);
            this.lblMobile.TabIndex = 17;
            this.lblMobile.Text = "label15";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(206, 72);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(76, 16);
            this.label16.TabIndex = 16;
            this.label16.Text = "手机号码：";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(130, 72);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(52, 16);
            this.lblGender.TabIndex = 15;
            this.lblGender.Text = "label14";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(409, 107);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(52, 16);
            this.lblCity.TabIndex = 14;
            this.lblCity.Text = "label13";
            // 
            // lblProvince
            // 
            this.lblProvince.AutoSize = true;
            this.lblProvince.Location = new System.Drawing.Point(249, 107);
            this.lblProvince.Name = "lblProvince";
            this.lblProvince.Size = new System.Drawing.Size(52, 16);
            this.lblProvince.TabIndex = 13;
            this.lblProvince.Text = "label12";
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(82, 107);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(52, 16);
            this.lblCountry.TabIndex = 12;
            this.lblCountry.Text = "label11";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(65, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 16);
            this.label9.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(344, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 16);
            this.label10.TabIndex = 10;
            this.label10.Text = "城市：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 16);
            this.label8.TabIndex = 9;
            this.label8.Text = "国  家：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(167, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "省/地区：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "性别：";
            // 
            // lblNickName
            // 
            this.lblNickName.AutoSize = true;
            this.lblNickName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNickName.Location = new System.Drawing.Point(82, 36);
            this.lblNickName.Name = "lblNickName";
            this.lblNickName.Size = new System.Drawing.Size(86, 24);
            this.lblNickName.TabIndex = 6;
            this.lblNickName.Text = "用户昵称";
            // 
            // picAvatar
            // 
            this.picAvatar.Location = new System.Drawing.Point(12, 31);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(64, 64);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 5;
            this.picAvatar.TabStop = false;
            // 
            // txtOpenId
            // 
            this.txtOpenId.Location = new System.Drawing.Point(85, 132);
            this.txtOpenId.Name = "txtOpenId";
            this.txtOpenId.ReadOnly = true;
            this.txtOpenId.Size = new System.Drawing.Size(406, 22);
            this.txtOpenId.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "用户ID：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(335, 286);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 42);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblSubType
            // 
            this.lblSubType.Location = new System.Drawing.Point(85, 53);
            this.lblSubType.Name = "lblSubType";
            this.lblSubType.ReadOnly = true;
            this.lblSubType.Size = new System.Drawing.Size(161, 22);
            this.lblSubType.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(267, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "调整为：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 16);
            this.label11.TabIndex = 18;
            this.label11.Text = "客户类型:";
            // 
            // cmbSubTypeTo
            // 
            this.cmbSubTypeTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubTypeTo.FormattingEnabled = true;
            this.cmbSubTypeTo.Location = new System.Drawing.Point(334, 53);
            this.cmbSubTypeTo.MaxLength = 10;
            this.cmbSubTypeTo.Name = "cmbSubTypeTo";
            this.cmbSubTypeTo.Size = new System.Drawing.Size(157, 24);
            this.cmbSubTypeTo.TabIndex = 23;
            // 
            // FrmItemEdit
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(536, 340);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmItemEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "编辑用户信息";
            this.Load += new System.EventHandler(this.FrmItemEdit_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtExpire;
        private System.Windows.Forms.TextBox txtExpireTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblProvince;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblNickName;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.TextBox txtOpenId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblMobile;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox lblSubType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbSubTypeTo;
    }
}