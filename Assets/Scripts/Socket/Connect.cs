using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;
using System.Timers;
using CustomEvent;
namespace SocketClient
{
    /// <summary>
    /// ����
    /// </summary>
    public class Connect
    {
        //����״̬
        public enum ConState
        {
            None,
            Connected,
        }

        //����������
        private const int Buffer_Size = 1024;
        //������
        private byte[] m_readBuffer = new byte[Buffer_Size];
        //�׽���
        private Socket m_socket;
        //buffer ����
        private int m_bufferCount = 0;
        //��Ϣ���ȣ�����ճ���ְ�
        private Int32 m_msgLen = 0;
        //��Ϣ����buffer������ճ���ְ�
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
        /// ���ӷ�����
        /// </summary>
        /// <param name="pHost">host</param>
        /// <param name="pPort">port</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void Conn(string pHost, int pPort)
        {
            OpenTimer(); //������ʱ��
            try
            {
                m_proto = new ProtocolByte();  // ��ʼ��Э������
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //�����׽���
                m_socket.BeginConnect(pHost, pPort, AsyncConnect, m_socket);  //�첽����
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

                    m_socket.BeginReceive(m_readBuffer, m_bufferCount, Buffer_Size - m_bufferCount, SocketFlags.None, OnReceive, m_readBuffer);  //�첽������Ϣ
                    m_id = m_socket.LocalEndPoint.ToString();
                    m_socket.EndConnect(ar);
                    m_isConnected = true;
                    state = ConState.Connected;
                    m_timer.Stop();  //���ӳɹ�ֹͣ��ʱ��
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
            m_timer.AutoReset = false; //�ر��Զ�����
            m_timer.Enabled = true;  //�����ʱ��
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

        //�첽���ջص�
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int _count = m_socket.EndReceive(ar);
                m_bufferCount += _count;
                DeCodeData();
                m_socket.BeginReceive(m_readBuffer, m_bufferCount, Buffer_Size - m_bufferCount, SocketFlags.None, OnReceive, m_readBuffer);  //�첽������Ϣʵ��ѭ��
            }
            catch
            {
                DisConn();
                Debug.Log("[���ݽ����쳣]");
            }
        }

        //��������
        private void DeCodeData()
        {
            if (m_bufferCount < sizeof(Int32))
                return;

            //��ȡ��Ϣ���ȣ�����ճ���ְ���
            Array.Copy(m_readBuffer, m_msgLenBuffer, sizeof(Int32));
            m_msgLen = BitConverter.ToInt32(m_msgLenBuffer, 0);
            if (m_bufferCount < m_msgLen + (sizeof(Int32)))
                return;

            ProtocolBase _proto = m_proto.DeCode(m_readBuffer, sizeof(Int32), m_msgLen);
            lock (m_msgDistribution.msgList)
            {
                m_msgDistribution.msgList.Add(_proto);
            }

            //����Ѵ������Ϣ
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
        /// �Ͽ�����
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool DisConn()
        {
            Debug.Log("socket�Ͽ�����");
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
                throw new Exception("[Connect][DisConn] socket�Ͽ����Ӵ���");
            }

        }
    }
}

