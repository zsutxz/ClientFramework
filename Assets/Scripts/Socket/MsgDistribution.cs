using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketClient
{
    /// <summary>
    /// 消息分发器
    /// </summary>
    public class MsgDistribution
    {
        /// <summary>
        /// 每帧处理数量
        /// </summary>
        public int count = 15;
        //消息链表
        public List<ProtocolBase> msgList = new List<ProtocolBase>();
        private Dictionary<string,ProtocolDele> m_eventDict = new Dictionary<string,ProtocolDele>();
        private Dictionary<string,ProtocolDele> m_onceEventDict = new Dictionary<string,ProtocolDele>();


        public void OnUpdate()
        {
            for (int i = 0; i < msgList.Count; i++)
            {
                if (msgList.Count > 0)
                {
                    lock (msgList)
                    {
                        Distribution(msgList[0]);
                        msgList.RemoveAt(0);
                    }
                }
                else
                {
                    return;
                }
            }   
        }


        private void Distribution(ProtocolBase pProto)
        {
            string _protoName = pProto.GetProtoName();
            if (m_eventDict.ContainsKey(_protoName))
            {
                m_eventDict[_protoName](pProto);
            }

            if (m_onceEventDict.ContainsKey(_protoName))
            {
                m_onceEventDict[_protoName](pProto);
                m_onceEventDict[_protoName] = null;
                m_onceEventDict.Remove(_protoName);
            }
        }

        public void AddMsgListener(string pProtoName,ProtocolDele pDele)
        {
            if (m_eventDict.ContainsKey(pProtoName))
                m_eventDict[pProtoName] += pDele;
            else
                m_eventDict[pProtoName] = pDele;
        }

        public void AdMsgOnceListener(string pProtoName, ProtocolDele pDele)
        {
            if (m_onceEventDict.ContainsKey(pProtoName))
                m_onceEventDict[pProtoName] += pDele;
            else
                m_onceEventDict[pProtoName] = pDele;
        }

        public void RemoveMsgListener(string pProtoName, ProtocolDele pDele)
        {
            if (m_eventDict.ContainsKey(pProtoName))
            {
                m_eventDict[pProtoName] -= pDele;
                if (m_eventDict[pProtoName] == null)
                    m_eventDict.Remove(pProtoName);
            }
        }

        public void RemoveMsgOnceListener(string pProtoName, ProtocolDele pDele)
        {
            if (m_onceEventDict.ContainsKey(pProtoName))
            {
                m_onceEventDict[pProtoName] -= pDele;
                if (m_onceEventDict[pProtoName] == null)
                    m_onceEventDict.Remove(pProtoName);
            }
        }




    }
}

