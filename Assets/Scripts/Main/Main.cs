using System.Collections.Generic;
using UnityEngine;
using CustomEvent;
using SocketClient;
using System.Collections;
using System;
/// <summary>
/// 项目入口
/// </summary>
public class Main : MonoBehaviour
{
    [Tooltip("资源更新")]
    public bool isUpdateAsset = false;

    private void Awake()
    {
        SocketConnHandler.AddConnListener(SocketConnHandler.ConnState.Connected, OnConnected);
        Log.Instance.Init();
    }
    private void Start()
    {
        Client.Instance.Conn();
    }

    private void OnConnected()
    {
        if (isUpdateAsset)
        {
            NetAssetsUpdate _update = new NetAssetsUpdate();
            StartCoroutine(_update.DownloadUpdateAsset(OnStartProcess));
        }
        else
        {
            Debug.Log("[Main][Start] 未开启资源更新 isOpenServerUpdate = false");
            OnStartProcess(true, null);
        }
    }

    private void OnStartProcess(bool pUpdateResult, List<string> pMsg)
    {
        if (pUpdateResult)
        {
            Debug.Log("[资源更新完成]！");
            Debug.Log("[Main][OnStartProcess] : 开始主流程");
            EventMgr.Trigger(EventID.ProjectStartUp);
        }
        else
        {
            Debug.Log("[资源更新失败]！");
            foreach (string msg in pMsg)
            {
                Debug.Log(msg);
            }
        }
    }

    private void OnDestroy()
    {
        Log.Instance.SaveLogOnExit();
    }


}
