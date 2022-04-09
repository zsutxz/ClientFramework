using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomEvent
{
    /*事件数据*/
    public class EventData
    {
        public object arg;
        public EventCallBack eventCallBack;

        public EventData(EventCallBack pEventCallBack, object pArg)
        {
            this.arg = pArg;
            this.eventCallBack = pEventCallBack;
        }
    }
}


