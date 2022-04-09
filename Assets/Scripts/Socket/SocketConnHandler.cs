using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocketClient
{
    /// <summary>
    /// socket Á¬½Ó handler
    /// </summary>
    public class SocketConnHandler
    {
        public delegate void ScoketConnecntDele();
        public enum ConnState
        {
            Connecting,
            Connected,
            Disconnect,
            ConnectTimeOut,
            ConnectFail,
        }

        static private List<ConnState> m_connStateList = new List<ConnState>();
        static private Dictionary<ConnState, ScoketConnecntDele> m_dict = new Dictionary<ConnState,ScoketConnecntDele>();
        
        
        static public void AddConnState(ConnState pState)
        {
            m_connStateList.Add(pState);
        }

        static public void AddConnListener(ConnState pState, ScoketConnecntDele pDele)
        {
            if (m_dict.ContainsKey(pState))
            {
                m_dict[pState] += pDele;
            }
            else
            {
                m_dict.Add(pState, pDele);
            }
        }

        static public void OnUpdate()
        {
            if (m_connStateList.Count > 0)
            {
                InvokeListener(m_connStateList[0]);
                if (m_connStateList.Count > 0)
                {
                    lock (m_connStateList)
                    {
                        m_connStateList.RemoveAt(0);
                    }
                }
            }
        }

        static private void InvokeListener(ConnState pState)
        {
            if (m_dict.ContainsKey(pState))
            {
                m_dict[pState]();
            }
            m_dict.Remove(pState);
        }

    }
}

