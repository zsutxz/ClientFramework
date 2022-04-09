using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// ��Դ��������
    /// </summary>
    public class AssetLoader : System.IDisposable
    {
        //��ǰ bundle
        private AssetBundle m_curBundle = null;
        //���漯��
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
        /// ����Դ���м���ָ����Դ
        /// </summary>
        /// <param name="pAssetName">��Դ����</param>
        /// <param name="isCache">�Ƿ񻺴�</param>
        /// <returns></returns>
        public Object LoadAsset(string pAssetName, bool isCache )
        {
            //��ѯ���漯��
            if (m_hashtable.Contains(pAssetName))
            {
                return m_hashtable[pAssetName] as Object;
            }
            
            //������Դ
            Object _ret = m_curBundle.LoadAsset<Object>(pAssetName);
            if(_ret!=null && isCache)
            {
                m_hashtable.Add(pAssetName, _ret);
            }
            return _ret;
        }

        /// <summary>
        /// ����Դ���м���ȫ����Դ
        /// </summary>
        /// <param name="pAssetName">��Դ����</param>
        /// <param name="isCache">�Ƿ񻺴�</param>
        /// <returns></returns>
        public Object[] LoadAllAsset()
        {
            //��ѯ���漯��
            //������Դ
            Object[] _ret = m_curBundle.LoadAllAssets();
            return _ret;
        }


        /// <summary>
        /// ж��ָ����Դ
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
        /// ���ڴ���ж��bundle������Դ
        /// </summary>
        public void Dispose()
        {
            if (m_curBundle != null)
                m_curBundle.Unload(false);
        }


        /// <summary>
        /// ���ڴ���ж��bundle������Դ ,���Ƿ��ڴ�
        /// </summary>
        public void DisposeAll()
        {
            if (m_curBundle != null)
                m_curBundle.Unload(true);
        }

        /// <summary>
        /// ��ȡbundle����ȫ����Դ����
        /// </summary>
        /// <returns></returns>
        public string[] GetAllAssetName()
        {
            return m_curBundle.GetAllAssetNames();
        }
    }
}

