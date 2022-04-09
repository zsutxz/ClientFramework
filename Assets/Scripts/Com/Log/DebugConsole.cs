using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using Config;
public class DebugConsole : MonoBehaviour
{
    static private DebugConsole m_instance;
    static public DebugConsole Instance { get { return m_instance; } }
    public int maxMsgRow = 15;
    public Text m_msgText;
    private List<string> m_msgList;

    private void Awake()
    {
        m_instance = this;
        m_msgList = new List<string>(25);
        DontDestroyOnLoad(gameObject);
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    public void Log(string pMsg)
    {
        if (m_msgList == null)
            return;

        if (m_msgList.Count == maxMsgRow)
        {
            m_msgList.RemoveAt(0);
        }
        m_msgList.Add(pMsg);
        ShowLogToScreen();
    }


    private void ShowLogToScreen()
    {
        if (m_msgList == null)
            return;

        m_msgText.text = string.Empty;
        for (int i = 0; i < m_msgList.Count; i++)
        {
            m_msgText.text += m_msgList[i] + "\n";
        }
    }


   

}
