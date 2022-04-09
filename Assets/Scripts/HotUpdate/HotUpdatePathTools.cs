using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdate
{
    public class HotUpdatePathTools
    {
        /// <summary> 编辑区配置文件路径 </summary>
        public const string ConfigEditorPath = "/Config";
        /// <summary> 发布区配置文件路径 </summary>
        public const string ConfigDeployPath = "/Config";
        /// <summary> 编辑区lua文件路径 </summary>
        public const string LuaEditorPath = "/Scripts/LuaScript/src";
        /// <summary> 编辑区lua文件路径 </summary>
        public const string LuaDelpoyPath = "/Lua";
        /// <summary> 服务器地址 </summary>
        public const string ServerUrl = "http://127.0.0.1:8080/UpdateAsset/";
        /// <summary> 检验文件名称 </summary>
        public const string VerifyFile = "/VerfiyFile";

        public const string ABResourceLuaPath = "/Scr";


        /// <summary>
        /// 获取发布去路径
        /// </summary>
        /// <returns></returns>
        static public string GetDeloyPath()
        {
            return EnviromentPath.GetPlatformPath() + "/" + EnviromentPath.GetPlatformName();
        }

        static public string GetABRescourcePath()
        {
            return Application.dataPath + "/ABResource";
        }
    }
}

