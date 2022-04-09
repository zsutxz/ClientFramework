using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketClient
{
    public class ProtocolHelper
    {
        /// <summary> 心跳协议 </summary>
        static public ProtocolBase HeartBeat()
        {
            ProtocolByte _protocol = new ProtocolByte();
            _protocol.AddString(ProtocolDefine.HeartBeat);
            return _protocol;
        }

        /// <summary> 登陆协议 </summary>
        static public ProtocolBase Login(string pUser,string pPw)
        {
            ProtocolByte _protocol = new ProtocolByte();
            _protocol.AddString(ProtocolDefine.Login);
            _protocol.AddString(pUser);
            _protocol.AddString(pPw);
            return _protocol;
        }


        /// <summary> 登出 </summary>
        static public ProtocolBase Logout(string pID)
        {
            ProtocolByte _protocol = new ProtocolByte();
            _protocol.AddString(ProtocolDefine.Logout);
            _protocol.AddString(pID);
            return _protocol;
        }

        /// <summary> 注册协议 </summary>
        static public ProtocolBase Regsiter(string pUser, string pPw)
        {
            ProtocolByte _protocol = new ProtocolByte();
            _protocol.AddString(ProtocolDefine.Regsiter);
            _protocol.AddString(pUser);
            _protocol.AddString(pPw);
            return _protocol;
        }

    }

}
