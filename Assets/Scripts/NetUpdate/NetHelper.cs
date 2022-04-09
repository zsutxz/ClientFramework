using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public delegate void NetDownloadCallBack(byte[] pBuffer, string pSavePath,bool pResulte,string pMsg);
public class NetHelper
{
    public IEnumerator DonloadAsset(string pUrl, string pSavePath, NetDownloadCallBack pCallBack)
    {
        using (UnityWebRequest _request = UnityWebRequest.Get(pUrl))
        {
            yield return _request.SendWebRequest();
            if (_request.isHttpError ==false && _request.isNetworkError ==false)
            {

                byte[] _buffer = _request.downloadHandler.data;
                pCallBack?.Invoke(_buffer, pSavePath,true,null);
            }
            else
            {
                Debug.Log("[NetHelper][DonloadAsset] œ¬‘ÿ¥ÌŒÛ £∫" + pUrl);
                pCallBack?.Invoke(null, pSavePath, false, "[NetHelper][DonloadAsset] œ¬‘ÿ¥ÌŒÛ £∫" + pUrl);
            }
        }
    }
}
