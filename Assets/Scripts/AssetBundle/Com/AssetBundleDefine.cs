using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 加载资源包回调
    /// </summary>
    /// <param name="pPackageName">资源包名称</param>
    public delegate void LoadAssetPackageCallBack(string pPackageName);
    public class AssetBundleDefine
    {
        static public readonly string AssetBundleManifest = "AssetBundleManifest";
    }
}

