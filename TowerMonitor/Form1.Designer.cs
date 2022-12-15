namespace TowerMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.realtimePictureBox = new System.Windows.Forms.PictureBox();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.frontButton = new System.Windows.Forms.Button();
            this.autonCheckBox = new System.Windows.Forms.CheckBox();
            this.zTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.accountLabel = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.ipLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realtimePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.realtimePictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zoomInButton);
            this.splitContainer1.Panel2.Controls.Add(this.zoomOutButton);
            this.splitContainer1.Panel2.Controls.Add(this.rightButton);
            this.splitContainer1.Panel2.Controls.Add(this.leftButton);
            this.splitContainer1.Panel2.Controls.Add(this.backButton);
            this.splitContainer1.Panel2.Controls.Add(this.frontButton);
            this.splitContainer1.Panel2.Controls.Add(this.autonCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.zTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.yTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.xTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.channelTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.loginButton);
            this.splitContainer1.Panel2.Controls.Add(this.passwordTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.usernameTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.accountLabel);
            this.splitContainer1.Panel2.Controls.Add(this.portTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.portLabel);
            this.splitContainer1.Panel2.Controls.Add(this.ipTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.ipLabel);
            this.splitContainer1.Size = new System.Drawing.Size(989, 610);
            this.splitContainer1.SplitterDistance = 728;
            this.splitContainer1.TabIndex = 0;
            // 
            // realtimePictureBox
            // 
            this.realtimePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realtimePictureBox.Location = new System.Drawing.Point(0, 0);
            this.realtimePictureBox.Name = "realtimePictureBox";
            this.realtimePictureBox.Size = new System.Drawing.Size(728, 610);
            this.realtimePictureBox.TabIndex = 0;
            this.realtimePictureBox.TabStop = false;
            // 
            // zoomInButton
            // 
            this.zoomInButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomInButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zoomInButton.Location = new System.Drawing.Point(134, 414);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(108, 27);
            this.zoomInButton.TabIndex = 26;
            this.zoomInButton.Text = "+";
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZoomInMouseDown);
            this.zoomInButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnZoomInMouseUp);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zoomOutButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zoomOutButton.Location = new System.Drawing.Point(20, 414);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(108, 27);
            this.zoomOutButton.TabIndex = 25;
            this.zoomOutButton.Text = "-";
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZoomOutMouseDown);
            this.zoomOutButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnZoomOutMouseUp);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rightButton.Location = new System.Drawing.Point(171, 341);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(61, 27);
            this.rightButton.TabIndex = 24;
            this.rightButton.Text = "右";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRightMouseDown);
            this.rightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRightMouseUp);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leftButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.leftButton.Location = new System.Drawing.Point(20, 341);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(61, 27);
            this.leftButton.TabIndex = 23;
            this.leftButton.Text = "左";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnLeftMouseDown);
            this.leftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnLeftMouseUp);
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.backButton.Location = new System.Drawing.Point(95, 372);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(61, 27);
            this.backButton.TabIndex = 22;
            this.backButton.Text = "後";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnBackMouseDown);
            this.backButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnBackMouseUp);
            // 
            // frontButton
            // 
            this.frontButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frontButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.frontButton.Location = new System.Drawing.Point(95, 309);
            this.frontButton.Name = "frontButton";
            this.frontButton.Size = new System.Drawing.Size(61, 27);
            this.frontButton.TabIndex = 21;
            this.frontButton.Text = "前";
            this.frontButton.UseVisualStyleBackColor = true;
            this.frontButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnFrontouseDown);
            this.frontButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnFrontMouseUp);
            // 
            // autonCheckBox
            // 
            this.autonCheckBox.AutoSize = true;
            this.autonCheckBox.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.autonCheckBox.Location = new System.Drawing.Point(28, 235);
            this.autonCheckBox.Name = "autonCheckBox";
            this.autonCheckBox.Size = new System.Drawing.Size(71, 25);
            this.autonCheckBox.TabIndex = 20;
            this.autonCheckBox.Text = "自動";
            this.autonCheckBox.UseVisualStyleBackColor = true;
            // 
            // zTextBox
            // 
            this.zTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zTextBox.Location = new System.Drawing.Point(136, 568);
            this.zTextBox.Name = "zTextBox";
            this.zTextBox.Size = new System.Drawing.Size(82, 27);
            this.zTextBox.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(33, 568);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 21);
            this.label7.TabIndex = 18;
            this.label7.Text = "Z 軸旋轉:";
            // 
            // yTextBox
            // 
            this.yTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.yTextBox.Location = new System.Drawing.Point(136, 531);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(82, 27);
            this.yTextBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(33, 537);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 21);
            this.label6.TabIndex = 16;
            this.label6.Text = "Y 軸旋轉:";
            // 
            // xTextBox
            // 
            this.xTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.xTextBox.Location = new System.Drawing.Point(136, 493);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(82, 27);
            this.xTextBox.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(33, 499);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 21);
            this.label5.TabIndex = 14;
            this.label5.Text = "X 軸旋轉:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(62, 454);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "陀螺儀資訊";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(78, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "監視器";
            // 
            // channelTextBox
            // 
            this.channelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.channelTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.channelTextBox.Location = new System.Drawing.Point(82, 190);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(150, 27);
            this.channelTextBox.TabIndex = 11;
            this.channelTextBox.Text = "1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(24, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 21);
            this.label2.TabIndex = 10;
            this.label2.Text = "頻道:";
            // 
            // loginButton
            // 
            this.loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.loginButton.Location = new System.Drawing.Point(28, 266);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(204, 28);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "登入";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.OnLoginClick);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.passwordTextBox.Location = new System.Drawing.Point(82, 156);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(150, 27);
            this.passwordTextBox.TabIndex = 7;
            this.passwordTextBox.Text = "Seafoodlab7812";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(24, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "密碼:";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.usernameTextBox.Location = new System.Drawing.Point(82, 123);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(150, 27);
            this.usernameTextBox.TabIndex = 5;
            this.usernameTextBox.Text = "admin";
            // 
            // accountLabel
            // 
            this.accountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountLabel.AutoSize = true;
            this.accountLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.accountLabel.Location = new System.Drawing.Point(24, 124);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(57, 21);
            this.accountLabel.TabIndex = 4;
            this.accountLabel.Text = "帳號:";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.portTextBox.Location = new System.Drawing.Point(82, 90);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(150, 27);
            this.portTextBox.TabIndex = 3;
            this.portTextBox.Text = "8000";
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.portLabel.Location = new System.Drawing.Point(33, 91);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(48, 21);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ipTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ipTextBox.Location = new System.Drawing.Point(82, 57);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(150, 27);
            this.ipTextBox.TabIndex = 1;
            this.ipTextBox.Text = "192.168.50.137";
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ipLabel.Location = new System.Drawing.Point(48, 57);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(33, 21);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "IP:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 610);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.realtimePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox realtimePictureBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label accountLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox xTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox zTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.CheckBox autonCheckBox;
        private System.Windows.Forms.Button frontButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button zoomOutButton;
        private System.Windows.Forms.Button zoomInButton;
    }
}

