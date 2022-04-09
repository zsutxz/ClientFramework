using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIFramework
{
    /*
       通用数据
     */
    [XLua.CSharpCallLua]
    public delegate void DeleCanvasReady(GameObject pCanvas);

    public delegate void DeleBtnListenser();

    public class UIDefine
    {
        static public readonly string hotUpdateCanvas = "HotUpdateCanvas";

        static public string GetConfigPath()
        {
            return Config.ConfigDefine.GetConfigPath() + Config.ConfigDefine.UiFormConfig;
        }
        
    }
    
    

    /// <summary> 窗体位置 </summary>
    public enum UIFormType 
    {
        /// <summary> 正常 </summary>
        Normal,
        /// <summary> 固定位置 </summary>
        Fixed,
        /// <summary> 弹窗 </summary>
        PopUp
    }

    /// <summary> 窗体显示</summary>
    public enum UIFormShowType
    {
        /// <summary> 正常打开 </summary>
        Normal,
        /// <summary> 切换 </summary>
        PushPop,
        /// <summary> 显示的时候隐藏其他面板 </summary>
        HideOther
    }

    /// <summary> 窗体显示</summary>
    public enum UIFormMaskType
    {
        /// <summary> 透明射线不穿透 </summary>
        Lucency,
        /// <summary> 透明射线穿透 </summary>
        LucencyPlus
    }

}

