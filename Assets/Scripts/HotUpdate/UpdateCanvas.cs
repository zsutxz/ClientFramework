using UnityEngine;
using UnityEngine.UI;
using CustomEvent;
using System;
using System.Collections.Generic;

public class UpdateCanvas : MonoBehaviour
{
    private Text m_LabelUpdateProgress;
    public Text m_LabelDes;

    private List<Action> m_Actionlist = new List<Action>();
    
    private void Awake()
    {
        m_LabelUpdateProgress = Helper.TransformHelper.GetChild(transform, "LabelUpdateProgress").GetComponent<Text>();
        m_LabelDes = Helper.TransformHelper.GetChild(transform, "LabelDes").GetComponent<Text>();
        m_LabelUpdateProgress.text =string.Empty;
        SocketClient.SocketConnHandler.AddConnListener(SocketClient.SocketConnHandler.ConnState.Connecting, OnConnecting);
        SocketClient.SocketConnHandler.AddConnListener(SocketClient.SocketConnHandler.ConnState.ConnectFail, OnConnectFail);
        SocketClient.SocketConnHandler.AddConnListener(SocketClient.SocketConnHandler.ConnState.ConnectTimeOut, OnConnectTimeOut);
        SocketClient.SocketConnHandler.AddConnListener(SocketClient.SocketConnHandler.ConnState.Connected, OnConnected);
    }

    private void OnConnecting()
    {
        m_LabelDes.text = "正在连接服务器...";
    }

    private void OnConnected()
    {
        m_LabelDes.text = "资源更新中...";
    }

    private void OnConnectFail()
    {
        m_LabelDes.text = "服务器链接失败...";
    }

    private void OnConnectTimeOut()
    {
        m_LabelDes.text = "服务器链接超时...";
    }



    private void OnDestroy()
    {
        //EventMgr.Remove(EventID.UpateProgress, OnUpdateProgress);
    }
}
