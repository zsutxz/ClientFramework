using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// bundle���ü�¼��
    /// </summary>
    public class BundleRelation
    {
        //bundle��
        private string m_assetBundleName = string.Empty;
        //��������bundle������
        private List<string> m_dependencesList = null;
        //��������bundle������
        private List<string> m_relationList = null;

        public BundleRelation(string pBundleName)
        {
            m_assetBundleName = pBundleName;
            m_dependencesList = new List<string>();
            m_relationList = new List<string>();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="pDepName">������</param>
        public void AddDependece(string pDepName)
        {
            if (!m_dependencesList.Contains(pDepName))
                m_dependencesList.Add(pDepName);
        }

        /// <summary>
        /// �Ƴ�����
        /// </summary>
        /// <param name="pDepName">������</param>
        /// <returns>false = �ð������������� �� true = �ð����Ƴ�ȫ������ </returns>
        public bool RemoveDependence(string pDepName)
        {
            if (m_dependencesList.Contains(pDepName))
                m_dependencesList.Remove(pDepName);

            if (m_dependencesList.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="pDepName">������</param>
        public void AddRelation(string pRelName)
        {
            if (!m_relationList.Contains(pRelName))
                m_relationList.Add(pRelName);
        }

        /// <summary>
        /// �Ƴ�����
        /// </summary>
        /// <param name="pRelName">������</param>
        /// <returns>false = �ð������������� �� true = �ð����Ƴ�ȫ������ </returns>
        public bool RemoveRelation(string pRelName)
        {
            if (m_relationList.Contains(pRelName))
                m_relationList.Remove(pRelName);

            if (m_relationList.Count > 0)
                return false;
            else
                return true;
        }

        public List<string> GetAllDependence()
        {
            return m_dependencesList;
        }

        public List<string> GetAllRelation()
        {
            return m_relationList;
        }
    }
}

