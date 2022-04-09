using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    [XLua.CSharpCallLua]
    public struct AssetPackageInfo
    {
        public string moduleName;
        public string packageName;
        public string assetName;
    }
}

