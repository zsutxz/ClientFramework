using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdate
{
    public class HotUpdatePathTools
    {
        /// <summary> �༭�������ļ�·�� </summary>
        public const string ConfigEditorPath = "/Config";
        /// <summary> �����������ļ�·�� </summary>
        public const string ConfigDeployPath = "/Config";
        /// <summary> �༭��lua�ļ�·�� </summary>
        public const string LuaEditorPath = "/Scripts/LuaScript/src";
        /// <summary> �༭��lua�ļ�·�� </summary>
        public const string LuaDelpoyPath = "/Lua";
        /// <summary> ��������ַ </summary>
        public const string ServerUrl = "http://127.0.0.1:8080/UpdateAsset/";
        /// <summary> �����ļ����� </summary>
        public const string VerifyFile = "/VerfiyFile";

        public const string ABResourceLuaPath = "/Scr";


        /// <summary>
        /// ��ȡ����ȥ·��
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

