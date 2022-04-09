using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// 配置文件常量
    /// </summary>
    public class ConfigDefine 
    {
        /// <summary> ui配置文件  </summary>
        public const string UiFormConfig = "/Config/json/FormConfig.json";
        /// <summary> 敏感字符配置文件  </summary>
        public const string SensitiveWord = "/Config/json/SensitiveWord.json";
        /// <summary> 日志与调试配置文件  </summary>
        public const string LogSetting = "/Config/json/LogSetting.json";


        static public string GetConfigPath()
        {
            return EnviromentPath.GetPlatformPath() + "/" + EnviromentPath.GetPlatformName();
        }
    }
}

