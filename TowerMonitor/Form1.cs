using HikvisionDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerMonitor.IMU;
using TowerMonitor.util;
using static HikvisionDemo.CHCNetSDK;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TowerMonitor
{
    public partial class Form1 : Form
    {

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

        private int AUTO_MOVE_TIME = 500;   // 自動移動時間(ms)

        CHCNetSDK.LOGINRESULTCALLBACK loginCallBack = null;
        CHCNetSDK.NET_DVR_USER_LOGIN_INFO struLogInfo;
        CHCNetSDK.NET_DVR_DEVICEINFO_V40 DeviceInfo;
        CHCNetSDK.REALDATACALLBACK RealData = null;
        public delegate void UpdateTextStatusCallback(string strLogStatus, IntPtr lpDeviceInfo);

        // 陀螺儀
        private IMUData device_data;
        private SerialPort serialPort = new SerialPort();
        private Connection m_connection = new Connection();
        private KbootPacketDecoder KbootDecoder = new KbootPacketDecoder();

        private int xBefore = 0;    // 舊x旋轉角度
        private int xNow = 0;       // 目前取得x旋轉角度
        private int dx = 5;         // x旋轉變化角度


        public Form1()
        {
            InitializeComponent();
            InitCHCNet();
            InitIMU();
        }


        // init Hikvision
        private void InitCHCNet() {
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
        private void InitIMU() {
            m_connection.OnSendData += new Connection.SendDataEventHandler(SendSerialPort);
            KbootDecoder.OnPacketRecieved += new KbootPacketDecoder.KBootDecoderDataReceivedEventHandler(OnKbootDecoderDataReceived);
            serialPort.WriteTimeout = 1000;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox.CheckForIllegalCrossThreadCalls = false;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {

            // 關閉監視器
            CloseHikvision();
           

            // 關閉IMU
            CloseSerialPort();

        }

        private void OnLoginClick(object sender, EventArgs e)
        {
            if (ipTextBox.Text == "" || portTextBox.Text == "" ||
                usernameTextBox.Text == "" || passwordTextBox.Text == "")
            {
                MessageBox.Show("請輸入IP、Port、帳號、密碼");
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
            struLogInfo = new CHCNetSDK.NET_DVR_USER_LOGIN_INFO();

            //Device IP or web url
            byte[] byIP = System.Text.Encoding.Default.GetBytes(ipTextBox.Text);
            struLogInfo.sDeviceAddress = new byte[129];
            byIP.CopyTo(struLogInfo.sDeviceAddress, 0);

            // Device Service Port
            struLogInfo.wPort = ushort.Parse(portTextBox.Text);

            //Username
            byte[] byUserName = System.Text.Encoding.Default.GetBytes(usernameTextBox.Text);
            struLogInfo.sUserName = new byte[64];
            byUserName.CopyTo(struLogInfo.sUserName, 0);

            //Password
            byte[] byPassword = System.Text.Encoding.Default.GetBytes(passwordTextBox.Text);
            struLogInfo.sPassword = new byte[64];
            byPassword.CopyTo(struLogInfo.sPassword, 0);


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
                return;
            }
            else
            {
                //登录成功
                MessageBox.Show("登入成功!");
                loginButton.Text = "登出";
                StartPreview();
                ConnectIMU();
            }

        }

        private void DoLogout() {
            //注销登录 Logout the device
            //if (m_lRealHandle >= 0)
            //{
            //    MessageBox.Show("請先停止畫面預覽");
            //    return;
            //}

            StopPreview();
            DoDVRLogout();
            CloseSerialPort();
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }

            MessageBox.Show("登出成功!");
            loginButton.Text = "登入";

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
        }
        
        /* #################
         * ##### 左鍵 ######
         * #################
         */
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
        }

        /* #################
         * ##### 右鍵 ######
         * #################
         */
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
        }

        /* #################
         * ##### 前鍵 ######
         * #################
         */
        private void OnFrontouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
            }
        }

        private void OnFrontMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);
            }
        }

        /* #################
         * ##### 後鍵 ######
         * #################
         */
        private void OnBackMouseDown(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
            }
        }

        private void OnBackMouseUp(object sender, MouseEventArgs e)
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);
            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);
            }
        }

        /* #################
         * ##### 放大 ######
         * #################
         */
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
        }

        /* #################
         * ##### 縮小 ######
         * #################
         */
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
        }


        /* ##########################
         * ##### 往前移動/停止 ######
         * ##########################
         */

        private void AutoMoveFront() {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
                Thread.Sleep(AUTO_MOVE_TIME);
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);

            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_MOVING, PTZ_SPEED);
                Thread.Sleep(AUTO_MOVE_TIME);
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_UP, PTZ_STOP, PTZ_SPEED);
            }
        }

        /* ##########################
         * ##### 往後移動/停止 ######
         * ##########################
         */
        private void AutoMoveBack()
        {
            if (m_lRealHandle >= 0) // 有取得預覽接口
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
                Thread.Sleep(AUTO_MOVE_TIME);
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(m_lRealHandle, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);

            }
            else
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_MOVING, PTZ_SPEED);
                Thread.Sleep(AUTO_MOVE_TIME);
                CHCNetSDK.NET_DVR_PTZControlWithSpeed_Other(m_lUserID, m_lChannel, CHCNetSDK.TILT_DOWN, PTZ_STOP, PTZ_SPEED);
            }
        }

        /* ##################
         * ##### 陀螺儀 #####
         * ##################
         */

        private void ConnectIMU() {
            string port = "COM3";
            int baudRate = 115200;
            OpenSerialPort(port, baudRate);
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

                string projectName = Assembly.GetExecutingAssembly().GetName().Name;
                string projectVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string showString = projectName + " (" + port.Replace("\0", "") + ", " + baudRate.ToString() + ")" + " V" + projectVersion;

                Debug.WriteLine("Info: " + showString);

                //MessageBox.Show(showString, "Error");
                //this.Text = Assembly.GetExecutingAssembly().GetName().Name + " (" + Name.Replace("\0", "") + ", " + Baudrate.ToString() + ")" + " V" + Assembly.GetExecutingAssembly().GetName().Version;

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

                xNow = Convert.ToInt32(device_data.SingleNode.Eul[1]);

                if (Math.Abs(xNow - xBefore) >= dx) {
                    //Console.WriteLine("xNow=" + xNow);
                    //Console.WriteLine("xBefore=" + xBefore);

                    if (xNow > xBefore)  // 往上升
                    {  
                        //Console.WriteLine("往上升");
                        AutoMoveBack();
                    }
                    else   // 往下降
                    {    
                        //Console.WriteLine("往下降");
                        AutoMoveFront();                       
                    }

                    xBefore = xNow;
                }
                
                yTextBox.Text = Convert.ToInt32(device_data.SingleNode.Eul[0]).ToString();
                xTextBox.Text = xNow.ToString();
                zTextBox.Text = Convert.ToInt32(device_data.SingleNode.Eul[2]).ToString();
                //this.showDataTextBox.Text = eul;
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
                //Debug.WriteLine("== SerialPort Close ==");

            }
            catch (Exception e)
            {
                //Debug.WriteLine("Exception=" + e.ToString());
                // do nothing
            }

        }



    }
}
