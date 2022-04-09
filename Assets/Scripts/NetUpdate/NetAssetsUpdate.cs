using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CustomEvent;
public class NetAssetsUpdate
{

    private string m_updatePath;  //本地路径
    private string m_serverUrl;  //服务器路径
    private string m_verityContent; //检验文件内容
    private bool m_updateResult = false;
    private List<string> m_downLoadMsg;
    public NetAssetsUpdate()
    {
        m_updateResult = true;
        m_downLoadMsg = new List<string>();
        m_updatePath = HotUpdate.HotUpdatePathTools.GetDeloyPath();
        m_serverUrl = HotUpdate.HotUpdatePathTools.ServerUrl + EnviromentPath.GetPlatformName();
    }

    /// <summary>
    /// 下载更新
    /// </summary>
    /// <param name="pFinishUpdateCallBack">更新完成回调</param>
    /// <returns></returns>
    public IEnumerator DownloadUpdateAsset(Action<bool, List<string>> pFinishUpdateCallBack)
    {
        NetHelper _helper = new NetHelper();
        string _verfyFileUrl = m_serverUrl + HotUpdate.HotUpdatePathTools.VerifyFile;

        if (!Directory.Exists(m_updatePath))
        {
            Directory.CreateDirectory(m_updatePath);
        }

        string _veriftySavePath = m_updatePath + HotUpdate.HotUpdatePathTools.VerifyFile;

        //下载校验文件
        yield return _helper.DonloadAsset(_verfyFileUrl, _veriftySavePath, DownVerifyCallBack);
        //新的校验文件信息
        string[] _newVeriftInfos = m_verityContent.Split('\n');


        for (int i = 0; i < _newVeriftInfos.Length; i++)
        {
            if (string.IsNullOrEmpty(_newVeriftInfos[i]))
                continue;
            string[] _md5Info = _newVeriftInfos[i].Split('|');
            string _fileName = _md5Info[0].Trim(); //去除前后空格
            string _serverfileMd5 = _md5Info[1].Trim();//去除前后空格
            string _localFilePath = m_updatePath + "/" + _fileName;

            //下载增量包
            if (!File.Exists(_localFilePath))
            {
                string _dir = Path.GetDirectoryName(_localFilePath);
                if (!string.IsNullOrEmpty(_dir))
                    Directory.CreateDirectory(_dir);
                string _fileUrl = m_serverUrl + "/" + _fileName;
                Debug.Log("下载增量包 ：" + _fileUrl);
                yield return _helper.DonloadAsset(_fileUrl, _localFilePath, DownloadFileCallBack);
            }
            else
            {
                string _localFileMd5 = Helper.FileHelper.GetFileMd5(_localFilePath);
                //如果Md5不一样则下载更新
                if (_localFileMd5.Equals(_serverfileMd5) == false)
                {
                    //下载原文件
                    File.Delete(_localFileMd5);
                    string _fileUrl = m_serverUrl + "/" + _fileName;
                    Debug.Log("下载更改包 ：" + _fileUrl);
                    //下载新文件
                    yield return _helper.DonloadAsset(_fileUrl, _localFilePath, DownloadFileCallBack);
                }
            }
        }
        yield return new WaitForSeconds(1.0f);
        pFinishUpdateCallBack?.Invoke(m_updateResult, m_downLoadMsg);
    }

    private void DownVerifyCallBack(byte[] pBuffer, string pSavePath, bool pResult, string pMsg)
    {
        m_updateResult = pResult;
        if (pResult)
        {
            try
            {
                m_verityContent = System.Text.Encoding.UTF8.GetString(pBuffer);
                File.WriteAllBytes(pSavePath, pBuffer);
            }
            catch (Exception e)
            {
                m_updateResult = false;
                m_downLoadMsg.Add("校验文件写入失败 ：错误信息 ：" + e.Message + " , 路径" + pSavePath);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(pMsg))
            {
                m_downLoadMsg.Add(pMsg);
            }
        }
    }

    private void DownloadFileCallBack(byte[] pBuffer, string pSavePath, bool pResult, string pMsg)
    {
        m_updateResult = pResult;
        if (pResult)
        {
            try
            {
              //  EventMgr.Trigger(EventID.UpateProgress, "Progress", 1);
                File.WriteAllBytes(pSavePath, pBuffer);
            }
            catch (Exception e)
            {
                m_updateResult = false;
                m_downLoadMsg.Add("资源文件写入失败 ：错误信息 ：" + e.Message + " , 路径" + pSavePath);

            }
        }
        else
        {
            if (!string.IsNullOrEmpty(pMsg))
            {
                m_downLoadMsg.Add(pMsg);
            }
        }

    }



}
