using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
namespace AssetBundleFramework
{
    /// <summary>
    /// bundle��������
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
        /// ����AB��
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
                        //ʵ����������
                        m_assetLoader = new AssetLoader(_bundle);

                        //���Ŀ��bundle���ͱ���bundle������ͬ�Ž��лص�
                        if (pTargetBundleName.Equals(m_bundleName))
                        {
                            //�ص�
                            m_callBack?.Invoke(m_bundleName);
                        }
                    
                    }

                }
                else
                {
                    Debug.Log("[BundleLoader][LoadAssetBundle] assetbundle����ʧ�� ·���� " + m_loadPath);
                }
            }
        }

        /// <summary>
        /// ����ab����ָ����Դ
        /// </summary>
        /// <param name="pAssetName">��Դ��</param>
        /// <param name="pIsCache">�Ƿ���뻺��</param>
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
        /// ж��ָ����Դ
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
        /// �Ƿ���Դ
        /// </summary>
        public void Dispose()
        {
            if (m_assetLoader != null)
            {
                m_assetLoader.Dispose();
            }
        }

        /// <summary>
        /// �ͷŵ�ǰAssetBundle��Դ��,��ж��������Դ
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
        /// ��ȡ��bundle����ȫ����Դ����
        /// </summary>
        /// <returns></returns>
        public string[] GetAllAssetNames()
        {
            return m_assetLoader.GetAllAssetName();
        }
    }
}

