using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// �����ļ�����
    /// </summary>
    public class ConfigDefine 
    {
        /// <summary> ui�����ļ�  </summary>
        public const string UiFormConfig = "/Config/json/FormConfig.json";
        /// <summary> �����ַ������ļ�  </summary>
        public const string SensitiveWord = "/Config/json/SensitiveWord.json";
        /// <summary> ��־����������ļ�  </summary>
        public const string LogSetting = "/Config/json/LogSetting.json";


        static public string GetConfigPath()
        {
            return EnviromentPath.GetPlatformPath() + "/" + EnviromentPath.GetPlatformName();
        }
    }
}

