using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    public class AssetBundleMgr : SingalMono<AssetBundleMgr>
    {
        private ManifestLoader m_manifestLoader;
        private Dictionary<string, BundleHelper> m_bundleHelperDic = null;

        protected override void InitMono()
        {
            DontDestroyOnLoad(this.gameObject);
            m_manifestLoader = new ManifestLoader();
            m_bundleHelperDic = new Dictionary<string, BundleHelper>();
        }

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="pPackageInfo">资源包信息</param>
        /// <param name="pCallBack">加载完成回调</param>
        public void LoadAssetPackage(AssetPackageInfo pPackageInfo, LoadAssetPackageCallBack pCallBack)
        {
            
            StartCoroutine(LoadPackage(pPackageInfo, pCallBack));
        }

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="pModuleName">模块名</param>
        /// <param name="pPackageName">ab包名</param>
        /// <param name="pCallBack">回调</param>
        public void LoadAssetPackage(string pModuleName, string pPackageName, LoadAssetPackageCallBack pCallBack)
        {
            StartCoroutine(LoadPackage(pModuleName, pPackageName, pCallBack));
        }

        /// <summary>
        /// 从资源包中加载资源
        /// </summary>
        /// <param name="pPackageInfo">资源包信息</param>
        /// <param name="pIsCache">是否缓存</param>
        /// <returns></returns>
        public Object LoadAsset(AssetPackageInfo pPackageInfo, bool pIsCache = false)
        {
            if (m_bundleHelperDic.ContainsKey(pPackageInfo.moduleName))
            {
                return m_bundleHelperDic[pPackageInfo.moduleName].LoadAsset(pPackageInfo.packageName, pPackageInfo.assetName, pIsCache);
            }
            return null;
        }

        /// <summary>
        /// 从资源包中加载资源
        /// </summary>
        /// <param name="pPackageInfo">资源包信息</param>
        /// <param name="pIsCache">是否缓存</param>
        /// <returns></returns>
        public Object[] LoadAllAsset(AssetPackageInfo pPackageInfo, bool pIsCache = false)
        {
            if (m_bundleHelperDic.ContainsKey(pPackageInfo.moduleName))
            {
                return m_bundleHelperDic[pPackageInfo.moduleName].LoadAllAsset(pPackageInfo.packageName, pIsCache);
            }
            return null;
        }



        /// <summary>
        /// 从资源包中加载资源
        /// </summary>
        /// <param name="pPackageInfo">资源包信息</param>
        /// <param name="pIsCache">是否缓存</param>
        /// <returns></returns>
        public Object LoadAsst(string pModuleName, string pPackageName,string pAssetName, bool pIsCache = false)
        {
            if (m_bundleHelperDic.ContainsKey(pModuleName))
            {
                return m_bundleHelperDic[pModuleName].LoadAsset(pPackageName, pAssetName, pIsCache);
            }
            return null;
        }


        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="pPackageInfo">资源包信息</param>
        /// <param name="pCallBack">加载完成回调</param>
        public IEnumerator LoadPackage(AssetPackageInfo pPackageInfo, LoadAssetPackageCallBack pCallBack)
        {
           
            yield return m_manifestLoader.LoadManifest();
            BundleHelper _helper = null;
            m_bundleHelperDic.TryGetValue(pPackageInfo.moduleName, out _helper);
            if (_helper == null)
            {
                _helper = new BundleHelper(pPackageInfo.moduleName, pPackageInfo.packageName, pCallBack);
                m_bundleHelperDic.Add(pPackageInfo.moduleName, _helper);
            }
            yield return _helper.LoadAssetBundle(pPackageInfo.packageName);
        }

        /// <summary>
        /// 加载资源包
        /// </summary>
        /// <param name="pModuleName">模块名</param>
        /// <param name="pPackageName">ab包名</param>
        /// <param name="pCallBack">回调</param>
        public IEnumerator LoadPackage(string pModuleName, string pPackageName, LoadAssetPackageCallBack pCallBack)
        {
            yield return m_manifestLoader.LoadManifest();
            BundleHelper _helper = null;
            m_bundleHelperDic.TryGetValue(pModuleName, out _helper);
            if (_helper == null)
            {
                _helper = new BundleHelper(pModuleName, pPackageName, pCallBack);
                m_bundleHelperDic.Add(pModuleName, _helper);
            }
            yield return _helper.LoadAssetBundle(pPackageName);
        }

        public AssetBundleManifest GetAssetBundleManifest()
        {
            if (m_manifestLoader != null)
                return m_manifestLoader.AssetBundleManifest;
            else
                return null;
        }


        public void DisposeModuleAssets(AssetPackageInfo pPackageInfo)
        {
            if (m_bundleHelperDic.ContainsKey(pPackageInfo.moduleName))
            {
                m_bundleHelperDic[pPackageInfo.moduleName].DispoeAllAsset();
                m_bundleHelperDic.Remove(pPackageInfo.moduleName);
            }
        }

        public void DisposeAllAsset()
        {
            foreach (BundleHelper helper in m_bundleHelperDic.Values)
            {
                helper.DispoeAllAsset();
            }
            m_bundleHelperDic.Clear();

        }
    }
}

