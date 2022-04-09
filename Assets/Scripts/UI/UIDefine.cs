using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIFramework
{
    /*
       ͨ������
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
    
    

    /// <summary> ����λ�� </summary>
    public enum UIFormType 
    {
        /// <summary> ���� </summary>
        Normal,
        /// <summary> �̶�λ�� </summary>
        Fixed,
        /// <summary> ���� </summary>
        PopUp
    }

    /// <summary> ������ʾ</summary>
    public enum UIFormShowType
    {
        /// <summary> ������ </summary>
        Normal,
        /// <summary> �л� </summary>
        PushPop,
        /// <summary> ��ʾ��ʱ������������� </summary>
        HideOther
    }

    /// <summary> ������ʾ</summary>
    public enum UIFormMaskType
    {
        /// <summary> ͸�����߲���͸ </summary>
        Lucency,
        /// <summary> ͸�����ߴ�͸ </summary>
        LucencyPlus
    }

}

