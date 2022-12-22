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

            DeviceDataEntity deviceDataEntity;
            string jsonData = "";

            if (!File.Exists(deviceDataPath))
            {                
                MessageBox.Show("設定檔不存在，請重新開啟軟體。謝謝!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            jsonData = FileUtil.ReadFile(deviceDataPath);
            deviceDataEntity = JsonConvert.DeserializeObject<DeviceDataEntity>(jsonData);

            ipTextBox.Text = deviceDataEntity.IP;
            portTextBox.Text = deviceDataEntity.Port;
            usernameTextBox.Text = deviceDataEntity.Username;
            passwordTextBox.Text = deviceDataEntity.Password;
            channelTextBox.Text = deviceDataEntity.Channel;
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            DeviceDataEntity deviceDataEntity = new DeviceDataEntity();
            deviceDataEntity.IP = ipTextBox.Text;
            deviceDataEntity.Port = portTextBox.Text;
            deviceDataEntity.Channel = "1";
            deviceDataEntity.Username = usernameTextBox.Text;
            deviceDataEntity.Password = passwordTextBox.Text;
            deviceDataEntity.Account = "admin";
            deviceDataEntity.AccountPassword = "admin";

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
