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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.realtimePictureBox = new System.Windows.Forms.PictureBox();
            this.loginPanel = new System.Windows.Forms.Panel();
            this.autoTrackCheckBox = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.cameraPanel = new System.Windows.Forms.Panel();
            this.settingButton = new System.Windows.Forms.Button();
            this.ipLabel = new System.Windows.Forms.Label();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.accountLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.resetButton = new System.Windows.Forms.Button();
            this.showDataCheckBox = new System.Windows.Forms.CheckBox();
            this.armDegreeTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cameraDegreeTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.rightButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.zTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.zoomPosTextBox = new System.Windows.Forms.TextBox();
            this.panPosTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tiltPosTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.fullPictureBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realtimePictureBox)).BeginInit();
            this.loginPanel.SuspendLayout();
            this.cameraPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.dataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fullPictureBox)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.loginPanel);
            this.splitContainer1.Panel2.Controls.Add(this.cameraPanel);
            this.splitContainer1.Panel2.Controls.Add(this.controlPanel);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.dataPanel);
            this.splitContainer1.Size = new System.Drawing.Size(1484, 761);
            this.splitContainer1.SplitterDistance = 1059;
            this.splitContainer1.TabIndex = 0;
            // 
            // realtimePictureBox
            // 
            this.realtimePictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realtimePictureBox.Location = new System.Drawing.Point(0, 0);
            this.realtimePictureBox.Name = "realtimePictureBox";
            this.realtimePictureBox.Size = new System.Drawing.Size(1059, 761);
            this.realtimePictureBox.TabIndex = 0;
            this.realtimePictureBox.TabStop = false;
            this.realtimePictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.realtimePictureBox_MouseDoubleClick);
            // 
            // loginPanel
            // 
            this.loginPanel.Controls.Add(this.autoTrackCheckBox);
            this.loginPanel.Controls.Add(this.label14);
            this.loginPanel.Controls.Add(this.statusTextBox);
            this.loginPanel.Controls.Add(this.loginButton);
            this.loginPanel.Location = new System.Drawing.Point(6, 179);
            this.loginPanel.Name = "loginPanel";
            this.loginPanel.Size = new System.Drawing.Size(412, 74);
            this.loginPanel.TabIndex = 36;
            // 
            // autoTrackCheckBox
            // 
            this.autoTrackCheckBox.AutoSize = true;
            this.autoTrackCheckBox.Checked = true;
            this.autoTrackCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoTrackCheckBox.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.autoTrackCheckBox.Location = new System.Drawing.Point(238, 10);
            this.autoTrackCheckBox.Name = "autoTrackCheckBox";
            this.autoTrackCheckBox.Size = new System.Drawing.Size(113, 25);
            this.autoTrackCheckBox.TabIndex = 35;
            this.autoTrackCheckBox.Text = "自動追蹤";
            this.autoTrackCheckBox.UseVisualStyleBackColor = true;
            this.autoTrackCheckBox.CheckedChanged += new System.EventHandler(this.OnAutoTrackCheckedClick);
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(18, 10);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 21);
            this.label14.TabIndex = 13;
            this.label14.Text = "狀態:";
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.statusTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.statusTextBox.Location = new System.Drawing.Point(81, 6);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(130, 27);
            this.statusTextBox.TabIndex = 14;
            // 
            // loginButton
            // 
            this.loginButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loginButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.loginButton.Location = new System.Drawing.Point(20, 39);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(379, 28);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "連線";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.OnLoginClick);
            // 
            // cameraPanel
            // 
            this.cameraPanel.AutoSize = true;
            this.cameraPanel.Controls.Add(this.settingButton);
            this.cameraPanel.Controls.Add(this.ipLabel);
            this.cameraPanel.Controls.Add(this.ipTextBox);
            this.cameraPanel.Controls.Add(this.channelTextBox);
            this.cameraPanel.Controls.Add(this.portLabel);
            this.cameraPanel.Controls.Add(this.label2);
            this.cameraPanel.Controls.Add(this.portTextBox);
            this.cameraPanel.Controls.Add(this.accountLabel);
            this.cameraPanel.Controls.Add(this.passwordTextBox);
            this.cameraPanel.Controls.Add(this.usernameTextBox);
            this.cameraPanel.Controls.Add(this.label1);
            this.cameraPanel.Location = new System.Drawing.Point(3, 45);
            this.cameraPanel.Name = "cameraPanel";
            this.cameraPanel.Size = new System.Drawing.Size(415, 128);
            this.cameraPanel.TabIndex = 1;
            // 
            // settingButton
            // 
            this.settingButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.settingButton.Location = new System.Drawing.Point(273, 89);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(129, 27);
            this.settingButton.TabIndex = 12;
            this.settingButton.Text = "設備資料維護";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.OnOpenLoginFormClick);
            // 
            // ipLabel
            // 
            this.ipLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ipLabel.AutoSize = true;
            this.ipLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ipLabel.Location = new System.Drawing.Point(43, 14);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(33, 21);
            this.ipLabel.TabIndex = 0;
            this.ipLabel.Text = "IP:";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ipTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ipTextBox.Location = new System.Drawing.Point(82, 14);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.ReadOnly = true;
            this.ipTextBox.Size = new System.Drawing.Size(130, 27);
            this.ipTextBox.TabIndex = 1;
            // 
            // channelTextBox
            // 
            this.channelTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.channelTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.channelTextBox.Location = new System.Drawing.Point(82, 89);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.ReadOnly = true;
            this.channelTextBox.Size = new System.Drawing.Size(130, 27);
            this.channelTextBox.TabIndex = 11;
            // 
            // portLabel
            // 
            this.portLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.portLabel.Location = new System.Drawing.Point(221, 14);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(48, 21);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(19, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 21);
            this.label2.TabIndex = 10;
            this.label2.Text = "頻道:";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.portTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.portTextBox.Location = new System.Drawing.Point(272, 14);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.ReadOnly = true;
            this.portTextBox.Size = new System.Drawing.Size(130, 27);
            this.portTextBox.TabIndex = 3;
            // 
            // accountLabel
            // 
            this.accountLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.accountLabel.AutoSize = true;
            this.accountLabel.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.accountLabel.Location = new System.Drawing.Point(19, 50);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(57, 21);
            this.accountLabel.TabIndex = 4;
            this.accountLabel.Text = "帳號:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.passwordTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.passwordTextBox.Location = new System.Drawing.Point(272, 52);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.ReadOnly = true;
            this.passwordTextBox.Size = new System.Drawing.Size(130, 27);
            this.passwordTextBox.TabIndex = 7;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.usernameTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.usernameTextBox.Location = new System.Drawing.Point(82, 51);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.ReadOnly = true;
            this.usernameTextBox.Size = new System.Drawing.Size(130, 27);
            this.usernameTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(212, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "密碼:";
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.controlPanel.Controls.Add(this.button1);
            this.controlPanel.Controls.Add(this.resetButton);
            this.controlPanel.Controls.Add(this.showDataCheckBox);
            this.controlPanel.Controls.Add(this.armDegreeTextBox);
            this.controlPanel.Controls.Add(this.label13);
            this.controlPanel.Controls.Add(this.cameraDegreeTextBox);
            this.controlPanel.Controls.Add(this.label12);
            this.controlPanel.Controls.Add(this.rightButton);
            this.controlPanel.Controls.Add(this.downButton);
            this.controlPanel.Controls.Add(this.zoomOutButton);
            this.controlPanel.Controls.Add(this.zoomInButton);
            this.controlPanel.Controls.Add(this.leftButton);
            this.controlPanel.Controls.Add(this.upButton);
            this.controlPanel.Location = new System.Drawing.Point(6, 259);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(415, 285);
            this.controlPanel.TabIndex = 35;
            this.controlPanel.Visible = false;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(297, 248);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 35;
            this.resetButton.Text = "俯瞰視角";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // showDataCheckBox
            // 
            this.showDataCheckBox.AutoSize = true;
            this.showDataCheckBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.showDataCheckBox.Location = new System.Drawing.Point(272, 207);
            this.showDataCheckBox.Name = "showDataCheckBox";
            this.showDataCheckBox.Size = new System.Drawing.Size(122, 20);
            this.showDataCheckBox.TabIndex = 34;
            this.showDataCheckBox.Text = "顯示完整資訊";
            this.showDataCheckBox.UseVisualStyleBackColor = true;
            this.showDataCheckBox.CheckedChanged += new System.EventHandler(this.OnCheckedClick);
            // 
            // armDegreeTextBox
            // 
            this.armDegreeTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.armDegreeTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.armDegreeTextBox.Location = new System.Drawing.Point(204, 246);
            this.armDegreeTextBox.Name = "armDegreeTextBox";
            this.armDegreeTextBox.Size = new System.Drawing.Size(45, 27);
            this.armDegreeTextBox.TabIndex = 33;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(15, 246);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(183, 21);
            this.label13.TabIndex = 32;
            this.label13.Text = "目前吊臂上下角度:";
            // 
            // cameraDegreeTextBox
            // 
            this.cameraDegreeTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cameraDegreeTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cameraDegreeTextBox.Location = new System.Drawing.Point(204, 201);
            this.cameraDegreeTextBox.Name = "cameraDegreeTextBox";
            this.cameraDegreeTextBox.Size = new System.Drawing.Size(45, 27);
            this.cameraDegreeTextBox.TabIndex = 31;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(15, 207);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(183, 21);
            this.label12.TabIndex = 27;
            this.label12.Text = "目前鏡頭上下角度:";
            // 
            // rightButton
            // 
            this.rightButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rightButton.Location = new System.Drawing.Point(243, 66);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(60, 60);
            this.rightButton.TabIndex = 24;
            this.rightButton.Text = "右";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnRightMouseDown);
            this.rightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRightMouseUp);
            // 
            // downButton
            // 
            this.downButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.downButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.downButton.Location = new System.Drawing.Point(177, 130);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(60, 60);
            this.downButton.TabIndex = 22;
            this.downButton.Text = "下";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnDownMouseDown);
            this.downButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnDownMouseUp);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.zoomOutButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zoomOutButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomOutButton.Image")));
            this.zoomOutButton.Location = new System.Drawing.Point(346, 130);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Size = new System.Drawing.Size(60, 60);
            this.zoomOutButton.TabIndex = 25;
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZoomOutMouseDown);
            this.zoomOutButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnZoomOutMouseUp);
            // 
            // zoomInButton
            // 
            this.zoomInButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.zoomInButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zoomInButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomInButton.Image")));
            this.zoomInButton.Location = new System.Drawing.Point(346, 44);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Size = new System.Drawing.Size(60, 60);
            this.zoomInButton.TabIndex = 26;
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZoomInMouseDown);
            this.zoomInButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnZoomInMouseUp);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.leftButton.Location = new System.Drawing.Point(113, 66);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(60, 60);
            this.leftButton.TabIndex = 23;
            this.leftButton.Text = "左";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnLeftMouseDown);
            this.leftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnLeftMouseUp);
            // 
            // upButton
            // 
            this.upButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.upButton.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.upButton.Location = new System.Drawing.Point(177, 0);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(60, 60);
            this.upButton.TabIndex = 21;
            this.upButton.Text = "上";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnUpMouseDown);
            this.upButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnUpMouseUp);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(167, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "攝影機";
            // 
            // dataPanel
            // 
            this.dataPanel.Controls.Add(this.label11);
            this.dataPanel.Controls.Add(this.zTextBox);
            this.dataPanel.Controls.Add(this.label10);
            this.dataPanel.Controls.Add(this.zoomPosTextBox);
            this.dataPanel.Controls.Add(this.panPosTextBox);
            this.dataPanel.Controls.Add(this.label7);
            this.dataPanel.Controls.Add(this.label9);
            this.dataPanel.Controls.Add(this.label4);
            this.dataPanel.Controls.Add(this.yTextBox);
            this.dataPanel.Controls.Add(this.label5);
            this.dataPanel.Controls.Add(this.label8);
            this.dataPanel.Controls.Add(this.tiltPosTextBox);
            this.dataPanel.Controls.Add(this.label6);
            this.dataPanel.Controls.Add(this.xTextBox);
            this.dataPanel.Location = new System.Drawing.Point(9, 550);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(415, 192);
            this.dataPanel.TabIndex = 1;
            this.dataPanel.Visible = false;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(114, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(187, 25);
            this.label11.TabIndex = 34;
            this.label11.Text = "攝影機雲台位置";
            // 
            // zTextBox
            // 
            this.zTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.zTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zTextBox.Location = new System.Drawing.Point(329, 148);
            this.zTextBox.Name = "zTextBox";
            this.zTextBox.Size = new System.Drawing.Size(40, 27);
            this.zTextBox.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(6, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "左右 (P) :";
            // 
            // zoomPosTextBox
            // 
            this.zoomPosTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.zoomPosTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.zoomPosTextBox.Location = new System.Drawing.Point(363, 56);
            this.zoomPosTextBox.Name = "zoomPosTextBox";
            this.zoomPosTextBox.Size = new System.Drawing.Size(45, 27);
            this.zoomPosTextBox.TabIndex = 32;
            // 
            // panPosTextBox
            // 
            this.panPosTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panPosTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panPosTextBox.Location = new System.Drawing.Point(77, 56);
            this.panPosTextBox.Name = "panPosTextBox";
            this.panPosTextBox.Size = new System.Drawing.Size(45, 27);
            this.panPosTextBox.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(267, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Z 旋轉:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(128, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 16);
            this.label9.TabIndex = 29;
            this.label9.Text = "上下 (T) :";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(131, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "陀螺儀資訊";
            // 
            // yTextBox
            // 
            this.yTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.yTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.yTextBox.Location = new System.Drawing.Point(221, 148);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(40, 27);
            this.yTextBox.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(47, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "X 旋轉:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(255, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 16);
            this.label8.TabIndex = 31;
            this.label8.Text = "放大倍數 (Z) :";
            // 
            // tiltPosTextBox
            // 
            this.tiltPosTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tiltPosTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tiltPosTextBox.Location = new System.Drawing.Point(204, 56);
            this.tiltPosTextBox.Name = "tiltPosTextBox";
            this.tiltPosTextBox.Size = new System.Drawing.Size(45, 27);
            this.tiltPosTextBox.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(157, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Y 旋轉:";
            // 
            // xTextBox
            // 
            this.xTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.xTextBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.xTextBox.Location = new System.Drawing.Point(111, 148);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(40, 27);
            this.xTextBox.TabIndex = 15;
            // 
            // fullPictureBox
            // 
            this.fullPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fullPictureBox.Location = new System.Drawing.Point(0, 0);
            this.fullPictureBox.Name = "fullPictureBox";
            this.fullPictureBox.Size = new System.Drawing.Size(1484, 761);
            this.fullPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.fullPictureBox.TabIndex = 1;
            this.fullPictureBox.TabStop = false;
            this.fullPictureBox.Visible = false;
            this.fullPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fullPictureBox_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 36;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 761);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.fullPictureBox);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "塔吊輔助控制系統";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.realtimePictureBox)).EndInit();
            this.loginPanel.ResumeLayout(false);
            this.loginPanel.PerformLayout();
            this.cameraPanel.ResumeLayout(false);
            this.cameraPanel.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.dataPanel.ResumeLayout(false);
            this.dataPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fullPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox xTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox zTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button zoomOutButton;
        private System.Windows.Forms.Button zoomInButton;
        private System.Windows.Forms.TextBox zoomPosTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tiltPosTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox panPosTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel cameraPanel;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.TextBox cameraDegreeTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox armDegreeTextBox;
        private System.Windows.Forms.CheckBox showDataCheckBox;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.TextBox statusTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel loginPanel;
        private System.Windows.Forms.CheckBox autoTrackCheckBox;
        private System.Windows.Forms.PictureBox fullPictureBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button button1;
    }
}

