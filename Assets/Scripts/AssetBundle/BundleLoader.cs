using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace AssetBundleFramework
{
    /// <summary>
    /// bundle包加载器
    /// </summary>
    public class BundleLoader : System.IDisposable
    {
        private AssetLoader m_assetLoader = null;

        private LoadAssetPackageCallBack m_callBack;

        private string m_bundleName = string.Empty;

        private string m_loadPath = string.Empty;

        public BundleLoader(string pBundleName, LoadAssetPackageCallBack pCallBack)
        {
            m_bundleName = pBundleName;
            m_callBack = pCallBack;
            m_loadPath = AssetBundlePathTools.GetABSavePath() + "/" + m_bundleName;
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string pTargetBundleName)
        {
            using (UnityWebRequest _request = UnityWebRequestAssetBundle.GetAssetBundle(m_loadPath))
            {
                yield return _request.SendWebRequest();
                if (_request.isHttpError == false && _request.isNetworkError == false)
                {
                    AssetBundle _bundle = DownloadHandlerAssetBundle.GetContent(_request);
                    if (_bundle != null)
                    {
                        //实例化加载器
                        m_assetLoader = new AssetLoader(_bundle);

                        //如果目标bundle包和本身bundle包名相同才进行回调
                        if (pTargetBundleName.Equals(m_bundleName))
                        {
                            //回调
                            m_callBack?.Invoke(m_bundleName);
                        }
                    
                    }

                }
                else
                {
                    Debug.Log("[BundleLoader][LoadAssetBundle] assetbundle加载失败 路径： " + m_loadPath);
                }
            }
        }

        /// <summary>
        /// 加载ab包中指定资源
        /// </summary>
        /// <param name="pAssetName">资源名</param>
        /// <param name="pIsCache">是否加入缓存</param>
        /// <returns></returns>
        public Object LoadAsset(string pAssetName,bool pIsCache)
        {
            return m_assetLoader.LoadAsset(pAssetName, pIsCache);
        }

        public Object[] LoadAllAsset()
        {
            return m_assetLoader.LoadAllAsset();
        }


        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="pAsset"></param>
        /// <returns></returns>
        public bool UnLoadAsset(Object pAsset)
        {
            if (m_assetLoader == null)
                return false;
            return  m_assetLoader.UnLoadAsset(pAsset);
        }

        /// <summary>
        /// 是否资源
        /// </summary>
        public void Dispose()
        {
            if (m_assetLoader != null)
            {
                m_assetLoader.Dispose();
            }
        }

        /// <summary>
        /// 释放当前AssetBundle资源包,且卸载所有资源
        /// </summary>
        public void DisposeAll()
        {
            if (m_assetLoader != null)
            {
                m_assetLoader.DisposeAll();
                m_assetLoader = null;
            }
        }

        /// <summary>
        /// 获取该bundle包中全部资源名称
        /// </summary>
        /// <returns></returns>
        public string[] GetAllAssetNames()
        {
            return m_assetLoader.GetAllAssetName();
        }
    }
}

