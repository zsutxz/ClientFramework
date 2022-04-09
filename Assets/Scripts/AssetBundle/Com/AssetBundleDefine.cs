using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// ������Դ���ص�
    /// </summary>
    /// <param name="pPackageName">��Դ������</param>
    public delegate void LoadAssetPackageCallBack(string pPackageName);
    public class AssetBundleDefine
    {
        static public readonly string AssetBundleManifest = "AssetBundleManifest";
    }
}

