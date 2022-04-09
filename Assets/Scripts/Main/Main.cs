using System.Collections.Generic;
using UnityEngine;
using CustomEvent;
using SocketClient;
using System.Collections;
using System;
/// <summary>
/// ��Ŀ���
/// </summary>
public class Main : MonoBehaviour
{
    [Tooltip("��Դ����")]
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
            Debug.Log("[Main][Start] δ������Դ���� isOpenServerUpdate = false");
            OnStartProcess(true, null);
        }
    }

    private void OnStartProcess(bool pUpdateResult, List<string> pMsg)
    {
        if (pUpdateResult)
        {
            Debug.Log("[��Դ�������]��");
            Debug.Log("[Main][OnStartProcess] : ��ʼ������");
            EventMgr.Trigger(EventID.ProjectStartUp);
        }
        else
        {
            Debug.Log("[��Դ����ʧ��]��");
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
