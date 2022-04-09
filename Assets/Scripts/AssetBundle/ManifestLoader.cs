using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundleFramework
{
    /// <summary>
    /// ��Դ�嵥������
    /// </summary>
    public class ManifestLoader
    {
        private AssetBundleManifest m_AssetBundleManifest = null;
        public AssetBundleManifest AssetBundleManifest { get { return m_AssetBundleManifest; } }
        private string m_manifestPath;
        public ManifestLoader()
        {
            m_manifestPath = AssetBundlePathTools.GetABSavePath() + "/" + EnviromentPath.GetPlatformName();
        }

        /// <summary>
        /// ������Դ�嵥
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadManifest()
        {
            if (m_AssetBundleManifest != null)
                yield break;

            using (UnityWebRequest _request = UnityWebRequestAssetBundle.GetAssetBundle(m_manifestPath))
            {
                yield return _request.SendWebRequest();
                if (_request.isHttpError == false && _request.isNetworkError == false)
                {
                    AssetBundle _assetBundle = DownloadHandlerAssetBundle.GetContent(_request);
                    m_AssetBundleManifest = _assetBundle.LoadAsset<AssetBundleManifest>(AssetBundleDefine.AssetBundleManifest);
                    _assetBundle.Unload(false);
                }
                else
                {
                    Debug.Log("[ManifestLoader][LoadManifest] ���� ��Դ�嵥 ���� �� " + m_manifestPath);
                }
            }


        }
    }
}

