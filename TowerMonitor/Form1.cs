﻿using HikvisionDemo;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TowerMonitor
{
    public partial class Form1 : Form
    {
        //private int hasInitCamera = 0;      // 是否完成初始化鏡頭 1: 完成 0:未完成
        private bool isThreadStarted = false;        

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

            isThreadStarted = false;
        }

        
        // 因為陀螺儀丟出的訊息非常快，攝影機無法在短時間設定PTZ，
        // 所以寫個Thread 控制PTZ寫入速度
        private void StartSettingPTZ() {
            isThreadStarted = true;
            Thread myThread = new Thread(new ThreadStart(Task));
            //oGetArgThread.IsBackground = true;
            myThread.Start();
        }

        private void StopSettingPTZ() { 
            isThreadStarted = false;
        }

        private void Task()
        {
            while (isThreadStarted)
            {
                // Console.WriteLine("xNow=" + xNow + " yNow="+ yNow + " zNow="+ zNow);

                if (serialPort.IsOpen)
                {

                    string tValue = tInitValue.ToString("0.0");

                    dx = Math.Abs(xNow) - Math.Abs(xInitValue);

                    if (xNow > xInitValue)
                    {   // 陀螺儀往上轉動 (xNow等於0表示陀螺儀沒有在轉動)                        
                        tValue = (tInitValue - dx).ToString("0.0");
                    } 
                    else if (Math.Abs(xNow) > (xInitValue))
                    {    // 陀螺儀往下轉動 
                        tValue = (tInitValue + dx).ToString("0.0");
                    }

                    SetTParam(tValue);

                    yTextBox.Text = yNow.ToString();
                    xTextBox.Text = xNow.ToString();
                    zTextBox.Text = zNow.ToString();

                    ShowPTZParam();

                }// End if

                Thread.Sleep(500);
            }
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
                //hasInitCamera = 0;
                StartSettingPTZ();
                initValue();
            }

        }

        private void initValue() {
            NET_DVR_PTZPOS netDVRPTZPos = GetPTZParam();

            // 左右角度
            ushort wPanPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wPanPos, 16));
            float WPanPos = wPanPos * 0.1f;

            // 上下角度
            ushort wTiltPos = Convert.ToUInt16(Convert.ToString(netDVRPTZPos.wTiltPos, 16));
            tInitValue = wTiltPos * 0.1f;

            hasGetXInitValue = false;

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
            StopSettingPTZ();
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

        private void OnGetPTZButtonClick(object sender, EventArgs e)
        {
            ShowPTZParam();
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
         * ##### 上鍵 ######
         * #################
         */
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

            StopSettingPTZ();
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

            StartSettingPTZ();
        }

        /* #################
         * ##### 下鍵 ######
         * #################
         */
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

            StopSettingPTZ();
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

            StartSettingPTZ();
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

                // 焦距倍數
                ushort wZoomPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wZoomPos, 16));
                float WZoomPos = wZoomPos * 0.1f;
                zoomPosTextBox.Text = WZoomPos.ToString();

                //Console.WriteLine("P: " + WPanPos.ToString() + ", T: " + WTiltPos.ToString() + ", Z:" + WZoomPos.ToString());
            }
        }

        /* ##### 設定 P, T, Z #####*/
        private void SetPTZParam(string pParam, string tParam, string zParam) {
            
            String str1, str2, str3;

            if (pParam == "" || tParam == "" || zParam == "")
            {
                MessageBox.Show("請輸入 P, T, Z 值 ");
                return;
            }
            else
            {
               
                m_struPtzCfg.wAction = SET_P_T_Z_PARAM;               

                // 設定左右角度
                str1 = Convert.ToString(float.Parse(pParam) * 10);
                m_struPtzCfg.wPanPos = (ushort)(Convert.ToUInt16(str1, 16));


                // 設定上下角度
                str2 = Convert.ToString(float.Parse(tParam) * 10);
                m_struPtzCfg.wTiltPos = (ushort)(Convert.ToUInt16(str2, 16));

                // 設定焦距倍數
                str3 = Convert.ToString(float.Parse(zParam) * 10);
                m_struPtzCfg.wZoomPos = (ushort)(Convert.ToUInt16(str3, 16));
              

                // 設定
                Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
                IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_PTZPOS, 1, ptrPtzCfg, (UInt32)nSize))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SetDVRConfig failed, error code= " + iLastErr;
                    //设置POS参数失败
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    // MessageBox.Show("设置成功!");
                }

                Marshal.FreeHGlobal(ptrPtzCfg);

            }
        }

        /* ##### 設定 P #####*/
        private void SetPParam(string pParam)
        {

            String str1;

            if (pParam == "" )
            {
                MessageBox.Show("請輸入 P 值 ");
                return;
            }
            else
            {

                m_struPtzCfg.wAction = SET_P_PARAM;

                // 設定左右角度
                str1 = Convert.ToString(float.Parse(pParam) * 10);
                m_struPtzCfg.wPanPos = (ushort)(Convert.ToUInt16(str1, 16));


                // 設定
                Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
                IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_PTZPOS, 1, ptrPtzCfg, (UInt32)nSize))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SetDVRConfig failed, error code= " + iLastErr;
                    //设置POS参数失败
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //MessageBox.Show("设置成功!");
                }

                Marshal.FreeHGlobal(ptrPtzCfg);

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
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    // MessageBox.Show("设置成功!");                   
                }

                Marshal.FreeHGlobal(ptrPtzCfg);

            }
        }

        /* ##### 設定 Z #####*/
        private void SetZParam(string zParam)
        {

            String str3;

            if (zParam == "")
            {
                MessageBox.Show("請輸入 Z 值 ");
                return;
            }
            else
            {
              
                m_struPtzCfg.wAction = SET_Z_PARAM;               

                // 設定焦距倍數
                str3 = Convert.ToString(float.Parse(zParam) * 10);
                m_struPtzCfg.wZoomPos = (ushort)(Convert.ToUInt16(str3, 16));


                // 設定
                Int32 nSize = Marshal.SizeOf(m_struPtzCfg);
                IntPtr ptrPtzCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(m_struPtzCfg, ptrPtzCfg, false);

                if (!CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_PTZPOS, 1, ptrPtzCfg, (UInt32)nSize))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_SetDVRConfig failed, error code= " + iLastErr;
                    //设置POS参数失败
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    // MessageBox.Show("设置成功!");
                }

                Marshal.FreeHGlobal(ptrPtzCfg);

            }
        }

        /* ##### 設定 P, T #####*/
        private void SetPTParam(string pParam, string tParam)
        {

            String str1, str2;

            if (pParam == "" || tParam == "")
            {
                MessageBox.Show("請輸入 P, T 值 ");
                return;
            }
            else
            {

                m_struPtzCfg.wAction = SET_P_T_PARAM;

                // 設定左右角度
                str1 = Convert.ToString(float.Parse(pParam) * 10);
                m_struPtzCfg.wPanPos = (ushort)(Convert.ToUInt16(str1, 16));


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
                    MessageBox.Show(str);
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
                yNow = Convert.ToSingle(device_data.SingleNode.Eul[0].ToString("0.0"));
                xNow = Convert.ToSingle(device_data.SingleNode.Eul[1].ToString("0.0"));
                zNow = Convert.ToSingle(device_data.SingleNode.Eul[2].ToString("0.0"));

                if (!hasGetXInitValue) {
                    xInitValue = xNow;
                    hasGetXInitValue = true;
                }

                // 變化角度
                //dx = Math.Abs(xNow - xBefore);

                //if (dx >=10)
                //{  //表示x軸有轉動 


                //    NET_DVR_PTZPOS netDVRPTZPos = GetPTZParam();

                //    // 左右角度
                //    ushort wPanPos = Convert.ToUInt16(Convert.ToString(m_struPtzCfg.wPanPos, 16));
                //    float WPanPos = wPanPos * 0.1f;
                //    int panPost = Convert.ToInt16(WPanPos);
                //    //int direction = 1;

                //    // 上下角度
                //    ushort wTiltPos = Convert.ToUInt16(Convert.ToString(netDVRPTZPos.wTiltPos, 16));
                //    float WTiltPos = wTiltPos * 0.1f;
                //    int tiltPos = Convert.ToInt16(WTiltPos);


                //    //string pValue = "0";
                //    string tValue = tiltPos.ToString();

                //    // 陀螺儀往上轉動 (xNow等於0表示陀螺儀沒有在轉動)
                //    if (xNow > 0)
                //    {
                //        if (xNow > xBefore)
                //        {   // 上升
                //            tValue = (tiltPos - dx).ToString();
                //        }
                //        else if (xNow < xBefore)
                //        { // 下降
                //            tValue = (tiltPos + dx).ToString();
                //        }
                //    }
                //    else if (xNow < 0)
                //    {   // 陀螺儀往下轉動

                //    }

                //    Console.WriteLine("xBefore= " + xBefore + ", xNow=" + xNow + ", dx=" + dx + ", TiltPos=" + WTiltPos + ", tValue=" + tValue);
                //    //SetPTParam(pValue, tValue);
                //    SetTParam(tValue);

                //    xBefore = xNow;

                //    ShowPTZParam();
                //}// End if


                //yTextBox.Text = yNow.ToString();
                //xTextBox.Text = xNow.ToString();
                //zTextBox.Text = zNow.ToString();



                //string tValue = (90 - Math.Abs(xNow)).ToString();  // 角度不可為負數
                //string pValue = "0";

                //if (Math.Abs(xNow - xBefore) >= dx)
                //{

                //    if (xNow >= 0)
                //    {
                //        pValue = "0";
                //    }
                //    else
                //    {
                //        pValue = "180";

                //    }
                //    SetPTParam(pValue, tValue);

                //    xBefore = xNow;
                //}

                /*
                if (hasInitCamera == 1)
                { // 已完成鏡頭位置初始化

                    if (Math.Abs(xNow - xBefore) >= dx)
                    {
                        
                        if (xNow >= 0)
                        {
                            pValue = "0";                            
                        }
                        else
                        {
                            pValue = "180";
                           
                        }
                        SetPTParam(pValue, tValue);
                        
                        xBefore = xNow;                       
                    }

                }
                else {  // 鏡頭初始化，登入時把鏡頭轉動到目前正確的位置

                    if (xNow >= 0) {
                        pValue = "0";
                    }
                    else {
                        pValue = "180";
                    }

                    SetPTParam(pValue, tValue);
                    hasInitCamera = 1;

                }
                */
                //xNow = decimal.Round(Convert.ToDecimal(device_data.SingleNode.Eul[1]), 1) ;

                //if (xNow >= 0 && xBefore!=xNow) {
                //    string tValue = xNow.ToString();
                //    SetTParam(tValue);
                //    ShowPTZParam();
                //    xBefore = xNow;
                //}

                //if (Math.Abs(xNow - xBefore) >= dx) {
                //    //Console.WriteLine("xNow=" + xNow);
                //    //Console.WriteLine("xBefore=" + xBefore);

                //    if (xNow > xBefore)  // 往上升
                //    {  
                //        //Console.WriteLine("往上升");
                //        AutoMoveBack();
                //    }
                //    else   // 往下降
                //    {    
                //        //Console.WriteLine("往下降");
                //        AutoMoveFront();                       
                //    }

                //    xBefore = xNow;
                //}

                //yTextBox.Text = Convert.ToInt32(device_data.SingleNode.Eul[0]).ToString();
                //xTextBox.Text = xNow.ToString();
                //zTextBox.Text = Convert.ToInt32(device_data.SingleNode.Eul[2]).ToString();

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

        /* ##########################
         * ##### 陀螺儀 (End) #####
         * ##########################
         */


    }
}
