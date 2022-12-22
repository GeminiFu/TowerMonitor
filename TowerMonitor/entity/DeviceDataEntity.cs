using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerMonitor.entity
{
    public class DeviceDataEntity
    {
        // 攝影機 IP
        public string IP { get; set; }
        // 攝影機 Port
        public string Port { get; set; }
        // 攝影機 登入帳號
        public string Username { get; set; }
        // 攝影機 登入密碼
        public string Password { get; set; }
        // 攝影機 頻道
        public string Channel { get; set; }

        // 管理者帳號
        public string Account { get; set; }
        // 管理者密碼
        public string AccountPassword { get; set; }
    }
}
