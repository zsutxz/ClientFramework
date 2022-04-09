using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent 
{
    /*注册的事件容器*/
    public class EventPanel
    {
        private int m_EventId;
        private Dictionary<EventCallBack, EventData> m_EventDict = new Dictionary<EventCallBack, EventData>();

        public EventPanel(int pEventId)
        {
            this.m_EventId = pEventId;
        }

        public void Add(EventCallBack pEventCallBack, object pArg)
        {
            EventData _eventData = null;
            if (m_EventDict.TryGetValue(pEventCallBack, out _eventData))
            {
                _eventData.arg = pArg;
            }
            else
            {
                m_EventDict.Add(pEventCallBack, new EventData(pEventCallBack, pArg));
            }
        }

        public void Remove(EventCallBack pEventCallBack)
        {
            if (true == this.m_EventDict.ContainsKey(pEventCallBack))
                this.m_EventDict.Remove(pEventCallBack);
        }

        public void Trigger(object pArg)
        {
            if (null != pArg)
            {
                object[] _triggerArgs = pArg as object[];
                if (_triggerArgs.Length == 1)
                    pArg = _triggerArgs[0];
            }
            EventData[] _eventDatas = new EventData[this.m_EventDict.Values.Count];
            this.m_EventDict.Values.CopyTo(_eventDatas, 0);
            foreach (EventData item in _eventDatas)
            {
                try
                {
                    item.eventCallBack.Invoke(this.m_EventId, item.arg, pArg);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            }
        }
    }

}


