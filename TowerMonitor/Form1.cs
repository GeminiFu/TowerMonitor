using HikvisionDemo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TowerMonitor.entity;
using TowerMonitor.IMU;
using TowerMonitor.util;
using static HikvisionDemo.CHCNetSDK;

namespace TowerMonitor
{
    public partial class Form1 : Form
    {
        private String startupPath = System.Windows.Forms.Application.StartupPath;
        private String deviceDataPath;
        private string nowPhotoPath = "";
        private string oldPhotoPath = "";

        private bool isSettingPTZRunning = false;        

        // 海康威視
        public uint PTZ_MOVING = 0;         // 雲台移動
        public uint PTZ_STOP = 1;           // 雲台停止
        public uint PTZ_SPEED = 3;          // 雲台移動速度 (預設 3)
        private Int32 m_lRealHandle = -1;   // 預覽接口
        public int m_lChannel = 1;          // 通道號碼 (預設 1)
        private uint iLastErr = 0;          // Error Code
        private bool m_bInitSDK = false;
        private Int32 m_lUserID = -1;       // User ID
        private string str;
        private bool isAuto = false;        // 雲台自動移動
        private ushort SET_P_T_Z_PARAM = 1;
        private ushort SET_P_PARAM = 2;
        private ushort SET_T_PARAM = 3;
        private ushort SET_Z_PARAM = 4;
        private ushort SET_P_T_PARAM = 5;

        private float tInitValue = 0f;    // 雲台 T 初始角度
        private float WPanPosMax = 359f;    // 雲台 左右最大角度
        private float WPanPosMin = 0f;      // 雲台 左右最小角度

        private float WTiltPosMax = 90f;    // 雲台 上下最大角度
        private float WTiltPosMin = 0f;     // 雲台 上下最小角度

        private float WZoomPosMax = 23f;     // 鏡頭 焦距最大倍數
        private float WZoomPosMin = 1f;     // 鏡頭 焦距最小倍數


        CHCNetSDK.LOGINRESULTCALLBACK loginCallBack = null;
        CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;
        CHCNetSDK.REALDATACALLBACK RealData = null;
        public delegate void UpdateTextStatusCallback(string strLogStatus, IntPtr lpDeviceInfo);

        private delegate void UpdateCameraPanelUICallBack(bool isShow, Control ctl);
        private delegate void UpdateControlPanelUICallBack(bool isShow, Control ctl);

        // 陀螺儀
        string imuPort = "";
        int baudRate = 0;
        private IMUData device_data;
        private SerialPort serialPort = new SerialPort();
        private Connection m_connection = new Connection();
        private KbootPacketDecoder KbootDecoder = new KbootPacketDecoder();

        private float xInitValue = 0f;          // 陀螺儀 x 初始角度
        private bool hasGetXInitValue = false;  // 是否有得x初始化角度
        private float xBefore = 0f;    // 舊x旋轉角度
        private float xNow = 0f;       // 目前取得x旋轉角度
        private float dx = 0f;         // x旋轉變化角度
        private float yNow = 0f;       // 目前取得y旋轉角度
        private float zNow = 0f;       // 目前取得z旋轉角度

        public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;

        public Form1()
        {
            InitializeComponent();
            InitCHCNet();
            InitIMU();
            InitDeviceData();
            
        }

        private void OnLoad(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox.CheckForIllegalCrossThreadCalls = false;

            
           
            if (!StringUtil.isEmpty(ipTextBox.Text) && 
                !StringUtil.isEmpty(portTextBox.Text) &&
                !StringUtil.isEmpty(usernameTextBox.Text) &&
                !StringUtil.isEmpty(passwordTextBox.Text) &&
                !StringUtil.isEmpty(channelTextBox.Text) &&
                !StringUtil.isEmpty(imuPort) &&
                !StringUtil.isEmpty(baudRate.ToString())) {
                AutoLogin();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {

            // 關閉監視器
            CloseHikvision();
           
            // 關閉IMU
            CloseSerialPort();

            isSettingPTZRunning = false;
        }

        // init Hikvision
        private void InitCHCNet()
        {
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                // To save the SDK log
                //CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);
            }
        }

        // init IMU
        private void InitIMU()
        {
            m_connection.OnSendData += new Connection.SendDataEventHandler(SendSerialPort);
            KbootDecoder.OnPacketRecieved += new KbootPacketDecoder.KBootDecoderDataReceivedEventHandler(OnKbootDecoderDataReceived);
            serialPort.WriteTimeout = 1000;
        }

        // 載入設備預設資料
        private void InitDeviceData() {
            deviceDataPath = startupPath + "\\deviceData.json";

            DeviceDataEntity deviceDataEntity;
            string jsonData = "";

            // 如果沒有deviceData.json 產生預設資料
            if (!File.Exists(deviceDataPath))
            {
                deviceDataEntity = new DeviceDataEntity();
                deviceDataEntity.IP = "";
                deviceDataEntity.Port = "";
                deviceDataEntity.Channel = "1";
                deviceDataEntity.Username = "";
                deviceDataEntity.Password = "";
                deviceDataEntity.IMUPort = "COM3";
                deviceDataEntity.BaudRate = 115200;
                deviceDataEntity.Account = "admin";
                deviceDataEntity.AccountPassword = "admin";

                jsonData = JsonConvert.SerializeObject(deviceDataEntity);
                bool isSuccess = FileUtil.WriteFile(deviceDataPath, jsonData);
            }
            
            jsonData = FileUtil.ReadFile(deviceDataPath);
            deviceDataEntity = JsonConvert.DeserializeObject<DeviceDataEntity>(jsonData);

            ipTextBox.Text = deviceDataEntity.IP;
            portTextBox.Text = deviceDataEntity.Port;
            usernameTextBox.Text = deviceDataEntity.Username;
            passwordTextBox.Text = deviceDataEntity.Password;
            channelTextBox.Text = deviceDataEntity.Channel;
            imuPort = deviceDataEntity.IMUPort;
            baudRate = deviceDataEntity.BaudRate;
        }

        // 取得目前 一開始的 雲台的 T 值, 陀螺儀的 一開始的 x 值
        private void InitValue()
        {
            NET_DVR_PTZPOS netDVRPTZPos = GetPTZParam();

            // 左右角度
            ushort wPanPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wPanPos, 16));
            float WPanPos = wPanPos * 0.1f;

            // 上下角度
            ushort wTiltPos = Convert.ToUInt16(Convert.ToString(netDVRPTZPos.wTiltPos, 16));
            tInitValue = wTiltPos * 0.1f;

            hasGetXInitValue = false;

        }

        private void InitCapturePhoto() {
            //nowPhotoPath = startupPath + "\\nowPhoto.jpg";
            //CapturePhoto(nowPhotoPath);
            //oldPhotoPath = startupPath + "\\oldPhoto.jpg";
            //FileUtil.CopyFile(nowPhotoPath, oldPhotoPath);
        }

        private void AutoLogin() {
            Thread myThread = new Thread(new ThreadStart(DoLoginTask));
            myThread.Start();
        }

        private void DoLoginTask() {
            DoLogin();           
        }

        private void StartAutoTrack() {
            bool isChecked = autoTrackCheckBox.Checked;
            if (isChecked) {
                InitValue();
                InitCapturePhoto();
                StartSettingPTZ();
            }
        }

        private void StopAutoTrack() {
            StopSettingPTZ();
        }

        // 因為陀螺儀丟出的訊息非常快，攝影機無法在短時間設定PTZ，
        // 所以寫個Thread 控制PTZ寫入速度
        private void StartSettingPTZ() {
            isSettingPTZRunning = true;
            Thread myThread = new Thread(new ThreadStart(SettingPTZTask));
            //oGetArgThread.IsBackground = true;
            myThread.Start();
        }

        private void StopSettingPTZ() { 
            isSettingPTZRunning = false;          
        }

        private void SettingPTZTask()
        {
            while (isSettingPTZRunning)
            {
                // Console.WriteLine("xNow=" + xNow + " yNow="+ yNow + " zNow="+ zNow);

                if (m_lRealHandle >= 0 && serialPort.IsOpen)
                {

                    string tValue = tInitValue.ToString("0.0");
                    dx = Math.Abs(xNow) - Math.Abs(xInitValue);

                    if (xNow > xInitValue)
                    {   // 陀螺儀往上轉動 (xNow等於0表示陀螺儀沒有在轉動)                        
                        tValue = (tInitValue - dx).ToString("0.0");
                    } 
                    else if (xNow < xInitValue)
                    {    // 陀螺儀往下轉動 
                        tValue = (tInitValue + dx).ToString("0.0");
                    }

                    if (Convert.ToSingle(tValue) < WTiltPosMin)
                    {
                        tValue = WTiltPosMin.ToString();
                    }

                    SetTParam(tValue);

                    yTextBox.Text = yNow.ToString();
                    xTextBox.Text = xNow.ToString();
                    zTextBox.Text = zNow.ToString();
                    armDegreeTextBox.Text = xTextBox.Text;

                    ShowPTZParam();
                    
                }// End if

                Thread.Sleep(500);
            }
        }

        private void CapturePhoto(string filePath) {
            
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档

            //JPEG抓图 Capture a JPEG picture
            if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, m_lChannel, ref lpJpegPara, filePath))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                str = "Successful to capture the JPEG file and the saved file is " + nowPhotoPath;
                //MessageBox.Show(str);
            }
        }

        private void OnLoginClick(object sender, EventArgs e)
        {
            if (ipTextBox.Text == "" || portTextBox.Text == "" ||
                usernameTextBox.Text == "" || passwordTextBox.Text == "" || channelTextBox.Text == "")
            {
                MessageBox.Show("請輸入IP、Port、帳號、密碼、頻道");
                return;
            }

            if (m_lUserID < 0)
            {
                DoLogin();
            }
            else
            {
                DoLogout();
            }
            return;
        }

        private void CloseHikvision() {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }
        }
        private void DoLogin() {

            string ip = ipTextBox.Text;
            string port = portTextBox.Text;
            string username = usernameTextBox.Text;
            string passwrod = passwordTextBox.Text;
            string channel = channelTextBox.Text;

            bool isConnectIMUScuess = ConnectIMU();
            bool isConnectCameraSuccess = ConnectCamera(ip, port, username, passwrod, channel);

            if (isConnectIMUScuess && isConnectCameraSuccess)
            {
                //登录成功
                //MessageBox.Show("連線成功!");
                statusTextBox.Text = "連線中...";
                loginButton.Text = "離線";
                StartPreview();               
                StartAutoTrack();
                ShowCameraPanel(false, cameraPanel);
                ShowControlPanel(true, controlPanel);
            }
            else 
            {
                DoDVRLogout();
                CloseSerialPort();
            }           

        }

        private void DoLogout() {
            //注销登录 Logout the device
            //if (m_lRealHandle >= 0)
            //{
            //    MessageBox.Show("請先停止畫面預覽");
            //    return;
            //}
            StopAutoTrack();
            StopPreview();
            DoDVRLogout();
            CloseSerialPort();
            //MessageBox.Show("離線成功!");
            statusTextBox.Text = "離線";
            loginButton.Text = "連線";

            ShowCameraPanel(true, cameraPanel);
            ShowControlPanel(false, controlPanel);

        }

        private void ShowCameraPanel(bool isShow, Control control)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    UpdateCameraPanelUICallBack callBack = new UpdateCameraPanelUICallBack(ShowCameraPanel);
                    this.Invoke(callBack, isShow, control);
                }
                else
                {
                    int height = cameraPanel.Height;
                    if (isShow)
                    {
                        loginPanel.Top = loginPanel.Top + height;
                    }
                    else
                    {
                        loginPanel.Top = loginPanel.Top - height;
                    }
                    control.Visible = isShow;
                }

            }
            catch
            {
                //MessageBox.Show("執行緒被關閉");
            }
        }

        private void ShowControlPanel(bool isShow, Control control)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    UpdateControlPanelUICallBack callBack = new UpdateControlPanelUICallBack(ShowControlPanel);
                    this.Invoke(callBack, isShow, control);
                }
                else
                {
                    int height = cameraPanel.Height;
                    if (isShow)
                    {
                        controlPanel.Top = controlPanel.Top - height;
                    }
                    else
                    {
                        controlPanel.Top = controlPanel.Top + height;
                    }

                    control.Visible = isShow;

                }
            }
            catch
            {
                //MessageBox.Show("執行緒被關閉");
            }
        }

        public void cbLoginCallBack(int lUserID, int dwResult, IntPtr lpDeviceInfo, IntPtr pUser)
        {
            string strLoginCallBack = "登入設備，lUserID：" + lUserID + "，dwResult：" + dwResult;

            if (dwResult == 0)
            {
                uint iErrCode = CHCNetSDK.NET_DVR_GetLastError();
                strLoginCallBack = strLoginCallBack + "，錯誤號:" + iErrCode;
            }

            //下面代码注释掉也会崩溃
            if (InvokeRequired)
            {
                object[] paras = new object[2];
                paras[0] = strLoginCallBack;
                paras[1] = lpDeviceInfo;
                //statusLabel.BeginInvoke(new UpdateTextStatusCallback(UpdateClientList), paras);
            }
            else
            {
                //创建该控件的主线程直接更新信息列表 
                UpdateClientList(strLoginCallBack, lpDeviceInfo);
            }

        }

        public void UpdateClientList(string strLogStatus, IntPtr lpDeviceInfo)
        {
            //列表新增报警信息
            //statusLabel.Text = "登入狀態: " + strLogStatus;
        }

        private bool ConnectCamera(string ip, string port, string username, string passwrod, string channel)
        {
            struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

            //Device IP or web url
            byte[] byIP = System.Text.Encoding.Default.GetBytes(ip);
            struLogInfo.sDeviceAddress = new byte[129];
            byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

            // Device Service Port
            struLogInfo.wPort = ushort.Parse(port);

            //Username
            byte[] byUserName = System.Text.Encoding.Default.GetBytes(username);
            struLogInfo.sUserName = new byte[64];
            byUserName.CopyTo(struLogInfo.sUserName, 0);

            //Password
            byte[] byPassword = System.Text.Encoding.Default.GetBytes(passwrod);
            struLogInfo.sPassword = new byte[64];
            byPassword.CopyTo(struLogInfo.sPassword, 0);

            // channel
            m_lChannel = Convert.ToInt16(channel);

            if (loginCallBack == null)
            {
                loginCallBack = new CHCNetSDK.LOGINRESULTCALLBACK(cbLoginCallBack);     //注册回调函数                    
            }
            struLogInfo.cbLoginResult = loginCallBack;
            struLogInfo.bUseAsynLogin = false; //是否异步登录：0- 否，1- 是 

            DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V40();

            // Login the device
            m_lUserID = CHCNetSDK.NET_DVR_Login_V40(ref struLogInfo, ref DeviceInfo);
            if (m_lUserID < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_Login_V40 failed, error code= " + iLastErr; //登录失败，输出错误号
                MessageBox.Show(str);
                return false;
            }

            return true;
        }


        private void StartPreview() {
            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = realtimePictureBox.Handle;         // 预览窗口
                lpPreviewInfo.lChannel = Int16.Parse(channelTextBox.Text);  // 预览的设备通道
                lpPreviewInfo.dwStreamType = 0;                             // 码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;                               // 连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true;                              // 0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 1;                          // 播放库播放缓冲区最大缓冲帧数
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                //if (idTextBox.Text != "")
                //{
                //    lpPreviewInfo.lChannel = -1;
                //    byte[] byStreamID = System.Text.Encoding.Default.GetBytes(idTextBox.Text);
                //    lpPreviewInfo.byStreamID = new byte[32];
                //    byStreamID.CopyTo(lpPreviewInfo.byStreamID, 0);
                //}


                if (RealData == null)
                {
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);    //预览实时流回调函数
                }

                IntPtr pUser = new IntPtr();//用户数据

                //Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);

                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                    MessageBox.Show(str);
                    return;
                }
            }
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            if (dwBufSize > 0)
            {
                byte[] sData = new byte[dwBufSize];
                Marshal.Copy(pBuffer, sData, 0, (Int32)dwBufSize);

                string str = "实时流数据.ps";
                FileStream fs = new FileStream(str, FileMode.Create);
                int iLen = (int)dwBufSize;
                fs.Write(sData, 0, iLen);
                fs.Close();
            }
        }

        private void StopPreview() {

            if (m_lRealHandle >= 0) {
                // Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lRealHandle = -1;
                //priviewButton.Text = "顯示及時影像";
            }

        }

        private void DoDVRLogout() {
            if (m_lUserID==-1) {
                return;
            }
            
            if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_Logout failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            m_lUserID = -1;

            panPosTextBox.Text = "";
            tiltPosTextBox.Text = "";
            zoomPosTextBox.Text = "";

            cameraDegreeTextBox.Text = "";
        }
        
        // ##### 左 (Start) ######  
        private void OnLeftMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_LEFT, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.PAN_LEFT, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();           
        }

        private void OnLeftMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_LEFT, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.PAN_LEFT, PTZ_STOP, PTZ_SPEED);
            }

            StartAutoTrack();
        }
        // ##### 左 (End) ######  


        // ##### 右 (Start) ######  
        private void OnRightMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_RIGHT, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.PAN_RIGHT, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();
        }

        private void OnRightMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.PAN_RIGHT, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.PAN_RIGHT, PTZ_STOP, PTZ_SPEED);
            }

            StartAutoTrack();
        }

        // ##### 右 (End) ######  

        // ##### 上 (Start) ######  
        private void OnUpMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();
        }

        private void OnUpMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);
            }
            
            StartAutoTrack();
        }
        // ##### 上 (End) ######  

        // ##### 下 (Start) ######  
        private void OnDownMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();
        }

        private void OnDownMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);
            }

            StartAutoTrack();
        }
        // ##### 下 (End) ######

        // ##### 放大 (Start) ######        
        private void OnZoomInMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.ZOOM_IN, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.ZOOM_IN, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();
        }

        private void OnZoomInMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.ZOOM_IN, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.ZOOM_IN, PTZ_STOP, PTZ_SPEED);
            }

            StartAutoTrack();

        }
        // ##### 放大 (End) ######        

        // ##### 縮小 (Start) ######              
        private void OnZoomOutMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.ZOOM_OUT, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.ZOOM_OUT, PTZ_MOVING, PTZ_SPEED);
            }

            StopAutoTrack();
        }

        private void OnZoomOutMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.ZOOM_OUT, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.ZOOM_OUT, PTZ_STOP, PTZ_SPEED);
            }

            StartAutoTrack();

        }
        // ##### 縮小 (End) ######              

        private NET_DVR_PTZPOS GetPTZParam() {

            NET_DVR_PTZPOS netDVRPTZPOS = new NET_DVR_PTZPOS();
            UInt32 dwReturn = 0;
            Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
            IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);
           
            CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_PTZPOS, -1, ptrPtzCfg, (UInt32)nSize, ref dwReturn);            
            netDVRPTZPOS = (CHCNetSDK.NET_DVR_PTZPOS)Marshal.PtrToStructure(ptrPtzCfg, typeof(CHCNetSDK.NET_DVR_PTZPOS));   //成功获取显示ptz参数
            return netDVRPTZPOS;
        }


        private void ShowPTZParam() {
            UInt32 dwReturn = 0;
            Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
            IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);
            //获取参数失败
            if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_PTZPOS, -1, ptrPtzCfg, (UInt32)nSize, ref dwReturn))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_GetDVRConfig failed, error code= " + iLastErr;
                MessageBox.Show(str);
                return;
            }
            else
            {
                m_struPtzCfg = (CHCNetSDK.NET_DVR_PTZPOS)Marshal.PtrToStructure(ptrPtzCfg, typeof(CHCNetSDK.NET_DVR_PTZPOS));
                //成功获取显示ptz参数

                // 左右角度
                ushort wPanPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wPanPos, 16));
                float WPanPos = wPanPos * 0.1f;
                panPosTextBox.Text = WPanPos.ToString();

                // 上下角度
                ushort wTiltPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wTiltPos, 16));
                float WTiltPos = wTiltPos * 0.1f;
                tiltPosTextBox.Text = WTiltPos.ToString();
                cameraDegreeTextBox.Text = WTiltPos.ToString();

                // 焦距倍數
                ushort wZoomPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wZoomPos, 16));
                float WZoomPos = wZoomPos * 0.1f;
                zoomPosTextBox.Text = WZoomPos.ToString();

                //Console.WriteLine("P: " + WPanPos.ToString() + ", T: " + WTiltPos.ToString() + ", Z:" + WZoomPos.ToString());
            }
        }

      
        /* ##### 設定 T #####*/
        private void SetTParam(string tParam)
        {

            String str2;

            if (tParam == "")
            {
                MessageBox.Show("請輸入 T 值 ");
                return;
            }
            else
            {

               m_struPtzCfg.wAction = SET_T_PARAM;


                // 設定上下角度
                str2 = Convert.ToString(float.Parse(tParam) * 10);
                m_struPtzCfg.wTiltPos = (ushort)(Convert.ToUInt16(str2, 16));


                // 設定
                Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
                IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_PTZPOS, 1, ptrPtzCfg, (UInt32)nSize))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SetDVRConfig failed, error code= " + iLastErr;
                    //设置POS参数失败
                    //MessageBox.Show(str);
                    return;
                }
                else
                {
                    // MessageBox.Show("设置成功!");                   
                }

                Marshal.FreeHGlobal(ptrPtzCfg);

            }
        }

      

        /* ##########################
         * ##### 陀螺儀 (Start) #####
         * ##########################
         */

        private bool ConnectIMU() {
            //imuPort = "COM3";
            //baudRate = 115200;            
            return OpenSerialPort(imuPort, baudRate);
        }

        private bool OpenSerialPort(string port, int baudRate)
        {
            // Open serial port
            CloseSerialPort();
            try
            {
                serialPort = new SerialPort(port, baudRate, Parity.None, 8, StopBits.One);
                serialPort.DataReceived += new SerialDataReceivedEventHandler(IMUDataReceived);
                serialPort.Open();

                //string projectName = Assembly.GetExecutingAssembly().GetName().Name;
                //string projectVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                //string showString = projectName + " (" + port.Replace("\0", "") + ", " + baudRate.ToString() + ")" + " V" + projectVersion;

                //Debug.WriteLine("Info: " + showString);

                //MessageBox.Show(showString, "Error");
                //this.Text = Assembly.GetExecutingAssembly().GetName().Name + " (" + Name.Replace("\0", "") + ", " + Baudrate.ToString() + ")" + " V" + Assembly.GetExecutingAssembly().GetName().Version;

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("陀螺儀設備: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }

        /// serialPort DataReceived event to print characters to terminal and process bytes through serialDecoder.
        private void IMUDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Get bytes from serial port
            if (serialPort.IsOpen)
            {

                int bytesToRead = serialPort.BytesToRead;
                byte[] readBuffer = new byte[bytesToRead];
                //Debug.WriteLine("bytesToRead=" + bytesToRead.ToString());
                //Debug.WriteLine("#####################");
                //Debug.WriteLine("readBuffer=" + readBuffer.ToString());

                if (serialPort.IsOpen)
                {
                    serialPort.Read(readBuffer, 0, bytesToRead);
                }

                m_connection.Input(readBuffer);
                KbootDecoder.Input(readBuffer);


            }
        }

        // 陀螺儀-送訊息
        public bool SendSerialPort(byte[] buffer, int offset, int count)
        {
            bool ret = true;
            try
            {
                //Debug.WriteLine("send");
                serialPort.Write(buffer, offset, count);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                ret = false;
                return ret;
            }
            return ret;
        }
        
        //  陀螺儀-回傳訊息
        private void OnKbootDecoderDataReceived(object sender, byte[] buf, int len)
        {
            device_data = IMUData.Decode(buf, len);
            //Debug.WriteLine("device_data=" + device_data.ToString());

            // Eul(RPY) 陀螺儀
            //string eul = "旋轉 Y 軸 eul[0]: " + device_data.SingleNode.Eul[0] + Environment.NewLine;
            //eul += "旋轉 X 軸 eul[1]: " + device_data.SingleNode.Eul[1] + "\r\n";
            //eul += "旋轉 Z 軸 eul[2]: " + device_data.SingleNode.Eul[2];
            //Debug.WriteLine(eul);
            //Debug.WriteLine("================");


            if (serialPort.IsOpen)
            {
                yNow = Convert.ToSingle(device_data.SingleNode.Eul[0].ToString("0.0"));
                xNow = Convert.ToSingle(device_data.SingleNode.Eul[1].ToString("0.0"));
                zNow = Convert.ToSingle(device_data.SingleNode.Eul[2].ToString("0.0"));

                if (!hasGetXInitValue) {
                    xInitValue = xNow;
                    hasGetXInitValue = true;
                }
               
            }
        }

        // 斷線
        private void CloseSerialPort()
        {

            try
            {
                serialPort.Close();
                xTextBox.Text = "";
                yTextBox.Text = "";
                zTextBox.Text = "";

                armDegreeTextBox.Text = "";
                //Debug.WriteLine("== SerialPort Close ==");

            }
            catch (Exception e)
            {
                //Debug.WriteLine("Exception=" + e.ToString());
                // do nothing
            }

        }


        /* ##########################
         * ##### 陀螺儀 (End) #####
         * ##########################
         */

        private void OnCheckedClick(object sender, EventArgs e)
        {
            bool isChecked = showDataCheckBox.Checked;
            if (isChecked)
            {
                dataPanel.Visible = true;
            }
            else 
            {
                dataPanel.Visible = false;
            }
        }

        private void OnOpenLoginFormClick(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();

            loginForm.LoginFormClosed += LoginFormClosed;
            loginForm.ShowDialog();

        }

        private void LoginFormClosed(object semder, FormClosedEventArgs e, bool isLoginSuccess) {
            if (isLoginSuccess) {                
                OpenSettingDataForm();
            }
        }

        private void OpenSettingDataForm()
        {
            deviceDataPath = startupPath + "\\deviceData.json";

            if (!File.Exists(deviceDataPath))
            {
                MessageBox.Show("設定檔不存在，請重新開啟軟體。謝謝!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                SetDeviceDataForm setDeviceDataForm = new SetDeviceDataForm();
                setDeviceDataForm.SetDeviceDataFormClosed += CloseListener;
                setDeviceDataForm.ShowDialog();
            }

        }

        private void CloseListener(object sender, EventArgs e)
        {
            InitDeviceData();
        }

        private void OnAutoTrackCheckedClick(object sender, EventArgs e)
        {
            bool isChecked = autoTrackCheckBox.Checked;
            if (isChecked)
            {
               StartAutoTrack();
            }
            else
            {
                StopAutoTrack();
                cameraDegreeTextBox.Text = "";
                armDegreeTextBox.Text = "";
            }
        }
    }
}
