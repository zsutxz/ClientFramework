using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundleFramework
{
    public class Build
    {
        [MenuItem("Tools/AssetBundle/Build")]
        static private void BuildAssetBundle()
        {
            string _exportPath = AssetBundlePathTools.GetABExportPath();
            if (string.IsNullOrEmpty(_exportPath))
                return;
            if (!Directory.Exists(_exportPath))
                Directory.CreateDirectory(_exportPath);

            BuildPipeline.BuildAssetBundles(_exportPath, BuildAssetBundleOptions.None, GetBuildPlatform());

            AssetDatabase.Refresh();
            Log.LogColor("[assetbundle打包完成]", Color.green);
        }

        [MenuItem("Tools/AssetBundle/Clear")]
        static private void ClearAssetBundle()
        {
            string _exportPath = AssetBundlePathTools.GetABExportPath();
            if (string.IsNullOrEmpty(_exportPath))
                return;
            if (Directory.Exists(_exportPath))
                Directory.Delete(_exportPath, true);
            if (File.Exists(_exportPath + ".meta"))
                File.Delete(_exportPath + ".meta");
            AssetDatabase.Refresh();
        }
        static private BuildTarget GetBuildPlatform()
        {
            BuildTarget _ret = BuildTarget.StandaloneWindows64;
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    _ret = BuildTarget.iOS;
                    break;

                case RuntimePlatform.Android:
                    _ret = BuildTarget.Android;
                    break;

                default:
                    break;
            }
            return _ret;
        }

     
    }


}

