using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetBundleFramework
{
    /*
      bundle 助手
      关联依赖关系及引用关系
     */
    public class BundleHelper
    {
        //当前bundle加载器
        private BundleLoader m_curBundleLoader = null;
        //模块加载器字典
        private Dictionary<string, BundleLoader> m_bundleLoaderDic = null;
        //模块关系字典
        private Dictionary<string, BundleRelation> m_bundleRelationDic = null;
        //当前bundle包名
        private string m_curBundleName = string.Empty;
        //当前模块名
        private string m_curMoudleName = string.Empty;

        private LoadAssetPackageCallBack m_loadFinishCallBack = null;


        public BundleHelper(string pModuleName, string pBundleName, LoadAssetPackageCallBack pCallBack)
        {
            m_curMoudleName = pModuleName;
            m_curBundleName = pBundleName;
            m_loadFinishCallBack = pCallBack;
            m_bundleLoaderDic = new Dictionary<string, BundleLoader>();
            m_bundleRelationDic = new Dictionary<string, BundleRelation>();
        }

        /// <summary>
        /// 加载bundle包
        /// </summary>
        /// <param name="pBundleName"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string pBundleName)
        {
            BundleRelation _relation = CheckBundleRelation(pBundleName);
            //获取全部依赖关系
            string[] _dependences = AssetBundleMgr.Instance.GetAssetBundleManifest().GetAllDependencies(pBundleName);
            for (int i = 0; i < _dependences.Length; i++)
            {
                string _dep = _dependences[i];
                //添加依赖关系
                _relation.AddDependece(_dep);
                //添加引用关系
                yield return LoadRelation(_dep, pBundleName);
            }

            //加载目标bundle包
            if (m_bundleLoaderDic.ContainsKey(pBundleName))
            {
                yield return m_bundleLoaderDic[pBundleName].LoadAssetBundle(m_curBundleName);
            }
            else
            {
                m_curBundleLoader = new BundleLoader(pBundleName, m_loadFinishCallBack);
                m_bundleLoaderDic.Add(pBundleName, m_curBundleLoader);
                yield return m_bundleLoaderDic[pBundleName].LoadAssetBundle(m_curBundleName);
            }
        }

        private IEnumerator LoadRelation(string pBundleName, string pRelationBundleName)
        {
            if (m_bundleRelationDic.ContainsKey(pBundleName))
            {
                BundleRelation _relation = m_bundleRelationDic[pBundleName];
                _relation.AddRelation(pRelationBundleName);
                yield break;
            }
            else
            {
                BundleRelation _relation = new BundleRelation(pBundleName);
                _relation.AddRelation(pRelationBundleName);
                m_bundleRelationDic.Add(pBundleName, _relation);
                //加载引用的包
                yield return LoadAssetBundle(pBundleName);
            }
        }




        public Object LoadAsset(string pBundleName,string pAssetName,bool pIsCache )
        {
            foreach (string item in m_bundleLoaderDic.Keys)
            {
                if (item.Equals(pBundleName))
                {
                    return m_bundleLoaderDic[item].LoadAsset(pAssetName, pIsCache);
                }
            }
            return null;
        }

        public Object[] LoadAllAsset(string pBundleName, bool pIsCache)
        {
            foreach (string item in m_bundleLoaderDic.Keys)
            {
                if (item.Equals(pBundleName))
                {
                    return m_bundleLoaderDic[item].LoadAllAsset();
                }
            }
            return null;
        }


        private BundleRelation CheckBundleRelation(string pBundleName)
        {
            BundleRelation _relation = null;
            m_bundleRelationDic.TryGetValue(pBundleName, out _relation);
            if (_relation == null)
            {
                _relation = new BundleRelation(pBundleName);
                m_bundleRelationDic.Add(pBundleName, _relation);
            }
            return _relation;
        }

        /// <summary>
        /// 释放全部资源
        /// </summary>
        public void DispoeAllAsset()
        {
            try
            {
                foreach (BundleLoader loader in m_bundleLoaderDic.Values)
                {
                    loader.DisposeAll();
                }
            }
            finally
            {
                m_bundleLoaderDic.Clear();
                m_bundleLoaderDic = null;
                m_bundleRelationDic.Clear();
                m_bundleRelationDic = null;
                if (m_curBundleLoader != null)
                {
                    m_curBundleLoader.DisposeAll();
                }

                m_loadFinishCallBack = null;

                Resources.UnloadUnusedAssets();

                System.GC.Collect();


            }
           
        }

    }
}

