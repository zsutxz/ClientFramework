using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Config
{
    /// <summary>
    /// ui´°Ìå½Ó¿Ú
    /// </summary>
    public interface IUIFormJsonConfig
    {
        UIFormData formData { get; }
    }



    [Serializable]
    public class UIFormData
    {
        public string msg;
        public List<UIFormObject> data;
    }

    [Serializable]
    public class UIFormObject
    {
        public string name;
        public UIFormInfo info;
    }

    [Serializable]
    public class UIFormInfo
    {
        public string UIFormType;
        public string UIFormShowType;
        public string UIFormMaskType;
        public string Prefab;
    }
}




