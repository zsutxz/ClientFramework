using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;
using System.Timers;
using CustomEvent;
namespace SocketClient
{
    /// <summary>
    /// 接连
    /// </summary>
    public class Connect
    {
        //连接状态
        public enum ConState
        {
            None,
            Connected,
        }

        //缓冲区容量
        private const int Buffer_Size = 1024;
        //缓冲区
        private byte[] m_readBuffer = new byte[Buffer_Size];
        //套节字
        private Socket m_socket;
        //buffer 长度
        private int m_bufferCount = 0;
        //消息长度，处理粘包分包
        private Int32 m_msgLen = 0;
        //消息长度buffer，处理粘包分包
        byte[] m_msgLenBuffer = new byte[sizeof(Int32)];

        public string m_id;

        private float m_lastTickTime = 0;
        public float heartBeatTime = 0;

        public ConState state = ConState.None;

        private ProtocolBase m_proto;

        private MsgDistribution m_msgDistribution = new MsgDistribution();

        private bool m_isConnected = false;

        private Timer m_timer = new Timer(5000);


        public void OnUpdate()
        {
            m_msgDistribution.OnUpdate();
            if (state == ConState.Connected)
            {
                if ((Time.time - m_lastTickTime) > heartBeatTime)
                {
                    ProtocolBase _heartBeat = ProtocolHelper.HeartBeat();
                    Send(_heartBeat);
                    m_lastTickTime = Time.time;
                }
            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="pHost">host</param>
        /// <param name="pPort">port</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void Conn(string pHost, int pPort)
        {
            OpenTimer(); //开启计时器
            try
            {
                m_proto = new ProtocolByte();  // 初始化协议类型
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //创建套节字
                m_socket.BeginConnect(pHost, pPort, AsyncConnect, m_socket);  //异步连接
            }
            catch
            {
                state = ConState.None;
                SocketConnHandler.AddConnState(SocketConnHandler.ConnState.ConnectFail);
            }
        }

        private void AsyncConnect(IAsyncResult ar)
        {
            try
            {
                m_isConnected = false;
                m_socket = ar.AsyncState as Socket;
                if (m_socket.Connected)
                {

                    m_socket.BeginReceive(m_readBuffer, m_bufferCount, Buffer_Size - m_bufferCount, SocketFlags.None, OnReceive, m_readBuffer);  //异步接收消息
                    m_id = m_socket.LocalEndPoint.ToString();
                    m_socket.EndConnect(ar);
                    m_isConnected = true;
                    state = ConState.Connected;
                    m_timer.Stop();  //连接成功停止计时器
                    SocketConnHandler.AddConnState(SocketConnHandler.ConnState.Connected);
                }
            }
            catch
            {
                SocketConnHandler.AddConnState(SocketConnHandler.ConnState.ConnectFail);
                m_isConnected = false;
            }
        }
        private void OpenTimer()
        {
            m_timer.Elapsed += new ElapsedEventHandler(TimerHandler);
            m_timer.AutoReset = false; //关闭自动重置
            m_timer.Enabled = true;  //激活计时器
        }

        private void TimerHandler(object sender, ElapsedEventArgs e)
        {
            if (m_isConnected == false)
            {
                if (m_socket != null)
                {
                    m_socket.Close();
                }
                SocketConnHandler.AddConnState(SocketConnHandler.ConnState.ConnectTimeOut);
            }
        }

        //异步接收回调
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int _count = m_socket.EndReceive(ar);
                m_bufferCount += _count;
                DeCodeData();
                m_socket.BeginReceive(m_readBuffer, m_bufferCount, Buffer_Size - m_bufferCount, SocketFlags.None, OnReceive, m_readBuffer);  //异步接收消息实现循环
            }
            catch
            {
                DisConn();
                Debug.Log("[数据接收异常]");
            }
        }

        //解码数据
        private void DeCodeData()
        {
            if (m_bufferCount < sizeof(Int32))
                return;

            //获取消息长度（处理粘包分包）
            Array.Copy(m_readBuffer, m_msgLenBuffer, sizeof(Int32));
            m_msgLen = BitConverter.ToInt32(m_msgLenBuffer, 0);
            if (m_bufferCount < m_msgLen + (sizeof(Int32)))
                return;

            ProtocolBase _proto = m_proto.DeCode(m_readBuffer, sizeof(Int32), m_msgLen);
            lock (m_msgDistribution.msgList)
            {
                m_msgDistribution.msgList.Add(_proto);
            }

            //清楚已处理的消息
            int _count = m_bufferCount - m_msgLen - sizeof(Int32);
            Array.Copy(m_readBuffer, sizeof(Int32) + m_msgLen, m_readBuffer, 0, _count);
            m_bufferCount = _count;
            if (m_bufferCount > 0)
                DeCodeData();
        }

        public bool Send(ProtocolBase pProto)
        {
            if (state != ConState.Connected)
                return false;
            if (pProto == null)
                return false;

            byte[] _protoBuffer = pProto.EnCode();
            byte[] _lenBuffer = BitConverter.GetBytes(_protoBuffer.Length);
            byte[] _sendBuffer = _lenBuffer.Concat(_protoBuffer).ToArray();
            m_socket.Send(_sendBuffer);
            return true;
        }

        public bool Send(ProtocolBase pProto, ProtocolDele pDele)
        {
            if (state != ConState.Connected)
                return false;

            if (pProto == null)
                return false;

            m_msgDistribution.AdMsgOnceListener(pProto.GetProtoName(), pDele);
            return Send(pProto);
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DisConn()
        {
            Debug.Log("socket断开连接");
            try
            {
                if (state == ConState.Connected)
                {
                    m_socket.Shutdown(SocketShutdown.Both);
                    m_socket.Close();
                }
                state = ConState.None;
                return true;
            }
            catch
            {
                throw new Exception("[Connect][DisConn] socket断开连接错误");
            }

        }
    }
}

