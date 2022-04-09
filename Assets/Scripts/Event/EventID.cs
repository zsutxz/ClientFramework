using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEvent
{
    public class EventID
    {
        static private int m_id;
        static private int next { get {  m_id += 1; return m_id; } }

        /// <summary> ���ӷ����� </summary>
        static public readonly int ConnSever = next;

        /// <summary> ���½��� </summary>
        static public readonly int UpateProgress = next;

        /// <summary> ��Ŀ���� </summary>
        static public readonly int ProjectStartUp = next;


    }
}

