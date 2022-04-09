using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    /// <summary>
    /// 事件回调函数.
    /// </summary>
    /// <param name="pEventId">事件编号</param>
    /// <param name="pSelfArg">自传入返回参数</param>
    /// <param name="pTriggerArg">触发传入参数</param>
    public delegate void EventCallBack(int pEventId, object pSelfArg, object pTriggerArg);

    /*事件管理器*/
    public class EventMgr
    {
        static private Dictionary<int, EventPanel> m_EventDict = new Dictionary<int, EventPanel>();

        static public void Register(int pEventId, EventCallBack pEventCB, object pSelfArg = null)
        {
            EventPanel _eventPanel = null;
            if (false == m_EventDict.TryGetValue(pEventId, out _eventPanel))
            {
                _eventPanel = new EventPanel(pEventId);
                m_EventDict.Add(pEventId, _eventPanel);
            }
            _eventPanel.Add(pEventCB, pSelfArg);
        }

        static public void Remove(int pEventId, EventCallBack pEventCB)
        {
            EventPanel _eventPanel = null;
            if (true == m_EventDict.TryGetValue(pEventId, out _eventPanel))
            {
                _eventPanel.Remove(pEventCB);
            }
        }

        static public void Clear(int pEventId)
        {
            m_EventDict.Remove(pEventId);
        }

        static public void Trigger(int pEventId, params object[] pArg)
        {
            EventPanel _eventPanel = null;
            if (true == m_EventDict.TryGetValue(pEventId, out _eventPanel))
            {
                try
                {
                    _eventPanel.Trigger(pArg);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }
        }

    }
}


