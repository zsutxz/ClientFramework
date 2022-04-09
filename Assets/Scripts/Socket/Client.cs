using System;
using UnityEngine;
using CustomEvent;
namespace SocketClient
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Client : SingalMono<Client>
    {
        //链接对象
        private Connect m_conn;
        protected override void InitMono()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Conn()
        {
            SocketConnHandler.AddConnState( SocketConnHandler.ConnState.Connecting);
            m_conn = new Connect() { heartBeatTime = SocketDefine.HeartBeatTime };
            m_conn.Conn(SocketDefine.Host, SocketDefine.Port);
        }

        private void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                Test();
            }

            SocketConnHandler.OnUpdate();
            if (m_conn != null)
                m_conn.OnUpdate();
        }

        private void OnDestroy()
        {
            if (m_conn != null)
                m_conn.DisConn();

        }

        public void Test()
        {
            ProtocolByte protocol = (ProtocolByte)ProtocolHelper.Login("111", "222");
            m_conn.Send(protocol, Call);

        }

        private void Call(ProtocolBase pProto)
        {
            ProtocolByte protocol = (ProtocolByte)pProto;
            int _start = 0;
            string name = protocol.GetString(_start, ref _start);
            int des = protocol.GetInt(_start, ref _start);
            Debug.Log(name + "   " + des);
        }
    }
}

