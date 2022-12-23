using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerMonitor.entity;
using TowerMonitor.util;

namespace TowerMonitor
{
    public partial class LoginForm : Form
    {
        private String startupPath = Application.StartupPath;
        private String deviceDataPath;

        private bool isLoginSuccess = false;

        public delegate void LoginFormClosedHandler(object semder, FormClosedEventArgs e, bool isLoginSuccess);
        public event LoginFormClosedHandler LoginFormClosed;

        public LoginForm()
        {
            deviceDataPath = startupPath + "\\deviceData.json";
            InitializeComponent();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            LoginFormClosed(sender, e, isLoginSuccess);
        }       

        private void OnLoginClick(object sender, EventArgs e)
        {
            string account = accountTextBox.Text;
            string password = passwordTextBox.Text;

            if (account=="" || password == "") {
                MessageBox.Show("請輸入帳號、密碼", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String jsonData = FileUtil.ReadFile(deviceDataPath);
            DeviceDataEntity deviceDataEntity = JsonConvert.DeserializeObject<DeviceDataEntity>(jsonData);

            if (account == deviceDataEntity.Account && password == deviceDataEntity.AccountPassword)
            {
                isLoginSuccess = true;
                MessageBox.Show("登入成功");
                this.Close();

            }
            else {
                MessageBox.Show("帳號或是密碼錯誤", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isLoginSuccess = false;
            }

        }
    }
}
