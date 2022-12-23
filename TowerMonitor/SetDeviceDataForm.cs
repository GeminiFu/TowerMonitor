using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerMonitor.entity;
using TowerMonitor.util;

namespace TowerMonitor
{
    public partial class SetDeviceDataForm : Form
    {
        private String startupPath = Application.StartupPath;
        private String deviceDataPath;
        private DeviceDataEntity deviceDataEntity;

        public delegate void SetDeviceDataFormClosedHandler(object semder, FormClosedEventArgs e);
        public event SetDeviceDataFormClosedHandler SetDeviceDataFormClosed;

        public SetDeviceDataForm()
        {
            deviceDataPath = startupPath + "\\deviceData.json";

            InitializeComponent();
            InitDeviceData();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            SetDeviceDataFormClosed(sender, e);
        }

        private void InitDeviceData()
        {

           
            string jsonData = "";

            if (!File.Exists(deviceDataPath))
            {                
                MessageBox.Show("設定檔不存在，請重新開啟軟體。謝謝!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            jsonData = FileUtil.ReadFile(deviceDataPath);
            deviceDataEntity = JsonConvert.DeserializeObject<DeviceDataEntity>(jsonData);

            // 攝影機
            ipTextBox.Text = deviceDataEntity.IP;
            portTextBox.Text = deviceDataEntity.Port;
            usernameTextBox.Text = deviceDataEntity.Username;
            passwordTextBox.Text = deviceDataEntity.Password;
            channelTextBox.Text = deviceDataEntity.Channel;

            // 陀螺儀
            imuPortTextBox.Text = deviceDataEntity.IMUPort;
            baudRateTextBox.Text = deviceDataEntity.BaudRate.ToString();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            string ip = ipTextBox.Text;
            string port = portTextBox.Text;
            string channel = channelTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string imuPort = imuPortTextBox.Text;
            string bautRate = baudRateTextBox.Text;

            if (StringUtil.isEmpty(ip))
            {
                MessageBox.Show("請輸入 IP", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(port))
            {
                MessageBox.Show("請輸入 Port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(channel))
            {
                MessageBox.Show("請輸入頻道", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(username))
            {
                MessageBox.Show("請輸入帳號", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(password))
            {
                MessageBox.Show("請輸入密碼", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(imuPort))
            {
                MessageBox.Show("請輸入陀螺儀 Port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StringUtil.isEmpty(bautRate))
            {
                MessageBox.Show("請輸入陀螺儀 Baud Rate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else {
                if (!StringUtil.isInteger(bautRate)) {
                    MessageBox.Show("陀螺儀 Baud Rate 請輸入整數", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            deviceDataEntity.IP = ip;
            deviceDataEntity.Port = port;
            deviceDataEntity.Channel = channel;
            deviceDataEntity.Username = username;
            deviceDataEntity.Password = password;
            deviceDataEntity.IMUPort = imuPort;
            deviceDataEntity.BaudRate = Convert.ToInt32(bautRate);
            //deviceDataEntity.Account = "admin";
            //deviceDataEntity.AccountPassword = "admin";

            if (accountPasswordTextBox.Text!="") {
                deviceDataEntity.AccountPassword = accountPasswordTextBox.Text;
            }

            String jsonData = JsonConvert.SerializeObject(deviceDataEntity);
            bool isSuccess = FileUtil.WriteFile(deviceDataPath, jsonData);

            if (isSuccess)
            {
                MessageBox.Show("設定成功");
                this.Close();
            }
            else {
                MessageBox.Show("設定失敗", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
    }
}
