using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SocketClient
{
    public delegate void ProtocolDele(ProtocolBase pProto);

    public class ProtocolDefine
    {
        /// <summary> ���� </summary>
        public const string HeartBeat = "HeartBeat";
        /// <summary> ��½ </summary>
        public const string Login = "Login";
        /// <summary> �ǳ� </summary>
        public const string Logout = "Logout";
        /// <summary> ע�� </summary>
        public const string Regsiter = "Regsiter";
    }
}

