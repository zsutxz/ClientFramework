using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundlePathTools 
{
    public const string ABRescource = "ABResource";

    /// <summary>
    /// 获取 ABRescource 文件夹路径
    /// </summary>
    /// <returns></returns>
    static public string GetABRescourcePath()
    {
        return Application.dataPath + "/" + ABRescource;
    }


    /// <summary>
    /// 获取AB包输出路径
    /// </summary>
    /// <returns></returns>
    static public string GetABExportPath()
    {
        return EnviromentPath.GetPlatformPath() + "/" + EnviromentPath.GetPlatformName();
    }


    /// <summary>
    /// 获取AB包保存路径
    /// </summary>
    /// <returns></returns>
    static public string GetABSavePath()
    {
        string _retPath = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                _retPath = "file://" + GetABExportPath();
                break;
   
            case RuntimePlatform.WindowsEditor:
                _retPath = "file://" + GetABExportPath();
                break;
            case RuntimePlatform.IPhonePlayer:
                _retPath = GetABExportPath() + "/Raw/";
                break;
        
            case RuntimePlatform.Android:
                _retPath = "jar:file://" + GetABExportPath();
                break;

            default:
                break;
        }

        return _retPath;
    }


}
