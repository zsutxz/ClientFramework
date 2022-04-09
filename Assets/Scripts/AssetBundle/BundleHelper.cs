using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetBundleFramework
{
    /*
      bundle ����
      ����������ϵ�����ù�ϵ
     */
    public class BundleHelper
    {
        //��ǰbundle������
        private BundleLoader m_curBundleLoader = null;
        //ģ��������ֵ�
        private Dictionary<string, BundleLoader> m_bundleLoaderDic = null;
        //ģ���ϵ�ֵ�
        private Dictionary<string, BundleRelation> m_bundleRelationDic = null;
        //��ǰbundle����
        private string m_curBundleName = string.Empty;
        //��ǰģ����
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
        /// ����bundle��
        /// </summary>
        /// <param name="pBundleName"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string pBundleName)
        {
            BundleRelation _relation = CheckBundleRelation(pBundleName);
            //��ȡȫ��������ϵ
            string[] _dependences = AssetBundleMgr.Instance.GetAssetBundleManifest().GetAllDependencies(pBundleName);
            for (int i = 0; i < _dependences.Length; i++)
            {
                string _dep = _dependences[i];
                //���������ϵ
                _relation.AddDependece(_dep);
                //������ù�ϵ
                yield return LoadRelation(_dep, pBundleName);
            }

            //����Ŀ��bundle��
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
                //�������õİ�
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
        /// �ͷ�ȫ����Դ
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

