using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public class EventID
    {
        static private int m_id;
        static private int next { get {  m_id += 1; return m_id; } }

        /// <summary> 连接服务器 </summary>
        static public readonly int ConnSever = next;

        /// <summary> 更新进度 </summary>
        static public readonly int UpateProgress = next;

        /// <summary> 项目启动 </summary>
        static public readonly int ProjectStartUp = next;


    }
}

