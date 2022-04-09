using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ����·��
/// </summary>
public class EnviromentPath 
{
    /// <summary>
    /// ��ȡ��ǰƽ̨·��
    /// </summary>
    /// <returns></returns>
    static public string GetPlatformPath()
    {
        string _retPath = string.Empty;
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
                _retPath = Application.streamingAssetsPath;
                break;
      
            case RuntimePlatform.WindowsEditor:
                _retPath = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.IPhonePlayer:
                break;
            case RuntimePlatform.Android:
                _retPath = Application.persistentDataPath;
                break;
            default:
                break;
        }
        return _retPath;
    }

    /// <summary>
    /// ��ȡƽ̨����
    /// </summary>
    /// <returns></returns>
    static public string GetPlatformName()
    {
        string _retName = string.Empty;
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
                _retName = "Windows";
                break;

            case RuntimePlatform.WindowsEditor:
                _retName = "Windows";
                break;
            case RuntimePlatform.IPhonePlayer:
                _retName = "IPhone";
                break;
            case RuntimePlatform.Android:
                _retName = "Android";
                break;
            default:
                break;
        }
        return _retName;
    }

    static public string GetPlatformLogPath()
    {
        string _retPath = string.Empty;
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
                _retPath = Path.Combine(Directory.GetCurrentDirectory(), "Log/");
                break;

            case RuntimePlatform.WindowsEditor:
                _retPath = Path.Combine(Directory.GetCurrentDirectory(), "Log/");
                break;
            case RuntimePlatform.IPhonePlayer:

                break;
            case RuntimePlatform.Android:
                _retPath = "jar:file://"+ Application.persistentDataPath+"/Log/";
                break;
            default:
                break;
        }
        return _retPath;
    }
}
