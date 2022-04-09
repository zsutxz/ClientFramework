using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleFramework
{
    /// <summary>
    /// bundle引用记录类
    /// </summary>
    public class BundleRelation
    {
        //bundle名
        private string m_assetBundleName = string.Empty;
        //依赖其他bundle包集合
        private List<string> m_dependencesList = null;
        //引用其他bundle包集合
        private List<string> m_relationList = null;

        public BundleRelation(string pBundleName)
        {
            m_assetBundleName = pBundleName;
            m_dependencesList = new List<string>();
            m_relationList = new List<string>();
        }

        /// <summary>
        /// 添加依赖
        /// </summary>
        /// <param name="pDepName">依赖名</param>
        public void AddDependece(string pDepName)
        {
            if (!m_dependencesList.Contains(pDepName))
                m_dependencesList.Add(pDepName);
        }

        /// <summary>
        /// 移除依赖
        /// </summary>
        /// <param name="pDepName">依赖名</param>
        /// <returns>false = 该包还有其他依赖 ， true = 该包已移除全部依赖 </returns>
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
        /// 添加引用
        /// </summary>
        /// <param name="pDepName">引用名</param>
        public void AddRelation(string pRelName)
        {
            if (!m_relationList.Contains(pRelName))
                m_relationList.Add(pRelName);
        }

        /// <summary>
        /// 移除引用
        /// </summary>
        /// <param name="pRelName">引用名</param>
        /// <returns>false = 该包还有其他引用 ， true = 该包已移除全部引用 </returns>
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

