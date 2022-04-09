using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// 资源包加载器
    /// </summary>
    public class AssetLoader : System.IDisposable
    {
        //当前 bundle
        private AssetBundle m_curBundle = null;
        //缓存集合
        private Hashtable m_hashtable = null;

        public AssetLoader(AssetBundle pBundle)
        {
            if (pBundle != null)
            {
                m_curBundle = pBundle;
                m_hashtable = new Hashtable();
            }
        }

        /// <summary>
        /// 从资源包中加载指定资源
        /// </summary>
        /// <param name="pAssetName">资源名字</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public Object LoadAsset(string pAssetName, bool isCache )
        {
            //查询缓存集合
            if (m_hashtable.Contains(pAssetName))
            {
                return m_hashtable[pAssetName] as Object;
            }
            
            //加载资源
            Object _ret = m_curBundle.LoadAsset<Object>(pAssetName);
            if(_ret!=null && isCache)
            {
                m_hashtable.Add(pAssetName, _ret);
            }
            return _ret;
        }

        /// <summary>
        /// 从资源包中加载全部资源
        /// </summary>
        /// <param name="pAssetName">资源名字</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public Object[] LoadAllAsset()
        {
            //查询缓存集合
            //加载资源
            Object[] _ret = m_curBundle.LoadAllAssets();
            return _ret;
        }


        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="pAsset"></param>
        /// <returns></returns>
        public bool UnLoadAsset(Object pAsset)
        {
            if (pAsset != null)
            {
                Resources.UnloadAsset(pAsset);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 从内存中卸载bundle镜像资源
        /// </summary>
        public void Dispose()
        {
            if (m_curBundle != null)
                m_curBundle.Unload(false);
        }


        /// <summary>
        /// 从内存中卸载bundle镜像资源 ,且是否内存
        /// </summary>
        public void DisposeAll()
        {
            if (m_curBundle != null)
                m_curBundle.Unload(true);
        }

        /// <summary>
        /// 获取bundle包中全部资源名称
        /// </summary>
        /// <returns></returns>
        public string[] GetAllAssetName()
        {
            return m_curBundle.GetAllAssetNames();
        }
    }
}

