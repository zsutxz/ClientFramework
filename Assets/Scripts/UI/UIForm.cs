using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIFramework
{


    /// <summary>
    /// UI´°Ìå
    /// </summary>
    public class UIForm
    {
        public UIFormType formType { get; private set; }
        public UIFormShowType showType { get; private set; }
        public UIFormMaskType maskType { get; private set; }
        private GameObject m_gameobject;
        public bool isShow { get; private set; }

        public UIForm(GameObject pGameobject,Config.UIFormInfo pInfo)
        {
            m_gameobject = pGameobject;
            isShow = false;
            if (pInfo != null)
            {
                formType = (UIFormType)Enum.Parse(typeof(UIFormType), pInfo.UIFormType);
                showType = (UIFormShowType)Enum.Parse(typeof(UIFormShowType), pInfo.UIFormShowType);
                maskType = (UIFormMaskType)Enum.Parse(typeof(UIFormMaskType), pInfo.UIFormMaskType);

                Transform _canvas= UIManager.Instance.GetCanvas().transform;
                if (_canvas)
                {
                   Transform _parent = Helper.TransformHelper.GetChild(_canvas, formType.ToString());
                    m_gameobject.transform.SetParent(_parent, false);
                }
            }
            else
            {
                Debug.Log("[UIForm][UIForm] UIFormInfo Îª¿Õ");
            }
        }

        /// <summary>
        /// Òþ²Ø
        /// </summary>
        public void Hide()
        {
            if (m_gameobject)
            {
                if(maskType== UIFormMaskType.Lucency)
                {
                    UIManager.Instance.GetMaskPanel().SetActive(false);
                }
                m_gameobject.SetActive(false);
                isShow = false;
            }
           
        }

        /// <summary>
        /// ÏÔÊ¾
        /// </summary>
        public void Show()
        {
            if (m_gameobject)
            {
                if (maskType == UIFormMaskType.Lucency)
                {
                    UIManager.Instance.GetMaskPanel().SetActive(true);
                }
                m_gameobject.SetActive(true);
                isShow = true;
            }
        }

        public void Dispose()
        {
            m_gameobject = null;
            isShow = false;
        }
    }

}
