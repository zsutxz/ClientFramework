using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;
using System;
using Config;

namespace UIFramework
{
    public class UIManager : SingalMono<UIManager>
    {
        //ui窗体
        private Dictionary<string, UIForm> m_formObjDic;
        //窗体信息
        private Dictionary<string, UIFormInfo> m_formInfoDic;
        //当前打开的窗体
        private Dictionary<string, UIForm> m_curOpenFormDic;
        private GameObject m_canvas;
        private GameObject m_uiMaskPanel;
        private GameObject m_hotUpdateCanvas;
        private AssetPackageInfo m_CanvaInfo;
        private DeleCanvasReady m_finsihCloneCanvas;
        private UIForm m_CurOperateForm; //当前操作窗体
        private Stack<UIForm> m_stackForm;
        protected override void InitMono()
        {
            m_stackForm = new Stack<UIForm>();
            m_formObjDic = new Dictionary<string, UIForm>();
            m_formInfoDic = new Dictionary<string, UIFormInfo>();
            m_curOpenFormDic = new Dictionary<string, UIForm>();
            m_hotUpdateCanvas = GameObject.Find(UIDefine.hotUpdateCanvas);
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// 加载UI Canvas
        /// </summary>
        /// <param name="pCallBack"></param>
        public void LoadCanvas(DeleCanvasReady pCallBack)
        {
            m_finsihCloneCanvas = pCallBack;
            m_CanvaInfo = new AssetPackageInfo();
            m_CanvaInfo.moduleName = "ui";
            m_CanvaInfo.packageName = "prefabs/ui.ab";
            m_CanvaInfo.assetName = "Canvas.prefab";

            //读取uiformjson数据
            IUIFormJsonConfig _config = new ConfigReader(ConfigDefine.UiFormConfig, ConfigReader.ConfigType.UIFormjson);
            for (int i = 0; i < _config.formData.data.Count; i++)
            {
                string _name = _config.formData.data[i].name;
                UIFormInfo _info = _config.formData.data[i].info;

                if (m_formInfoDic.ContainsKey(_name))
                    continue;
                m_formInfoDic.Add(_name, _info);

            }
            //加载 Canvas;
            AssetBundleMgr.Instance.LoadAssetPackage(m_CanvaInfo, OnFinishLoadCanvas);
        }

        private void OnFinishLoadCanvas(string pPackageName)
        {

            UnityEngine.Object _obj = AssetBundleMgr.Instance.LoadAsset(m_CanvaInfo);
            if (_obj != null)
            {
                m_canvas = Instantiate(_obj) as GameObject;
                DontDestroyOnLoad(m_canvas);
                m_uiMaskPanel = Helper.TransformHelper.GetChild(m_canvas.transform, "UIMaskPanel").gameObject;
                m_uiMaskPanel.SetActive(false);
                m_finsihCloneCanvas?.Invoke(m_canvas);
                CloseHotUpdateCanvas();
            }
            else
            {
                Debug.Log("[UIManager][OnFinishLoadCanvas]  canvas 加载失败");
            }
        }

        //打开窗体
        public void OpenForm(string pFormName)
        {
            UIForm _form = CreateForm(pFormName);
            
            if (_form.isShow)
                return;

            //暂时用不到，有待完善里面的逻辑
            // OnOpenFormShowTypeLogic(_form);

            _form.Show();

            //加入已经打开了的窗体集合
            if (!m_curOpenFormDic.ContainsKey(pFormName))
                m_curOpenFormDic.Add(pFormName, _form);

            //当前操作的窗体
            m_CurOperateForm = _form;

        }


        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="pFormName"></param>
        public void CloseForm(string pFormName)
        {
            UIForm _form = null;
            m_curOpenFormDic.TryGetValue(pFormName, out _form);
            if (_form == null)
                return;

            //暂时用不到，有待完善里面的逻辑
            //OnCloseFormShowTypeLogic(_form);
            _form.Hide();
            m_CurOperateForm = null;
            //从当前打开了的集合移除
            m_curOpenFormDic.Remove(pFormName);
        }

        

        //private void 

        /// <summary>
        /// 获取遮罩
        /// </summary>
        /// <returns></returns>
        public GameObject GetMaskPanel()
        {
            return m_uiMaskPanel;
        }

        /// <summary>
        /// 获取 Canva
        /// </summary>
        /// <returns></returns>
        public GameObject GetCanvas()
        {
            return m_canvas;
        }

        //克隆窗体资源对象
        private GameObject CloneForm(string pFormName)
        {
            GameObject _ret = null;
            UnityEngine.Object _clone = AssetBundleMgr.Instance.LoadAsst(m_CanvaInfo.moduleName, m_CanvaInfo.packageName, pFormName);
            if (_clone)
            {
                _ret = Instantiate(_clone) as GameObject;
            }
            return _ret;
        }

        //创建窗体
        private UIForm CreateForm(string pFormName)
        {
            UIForm _form = null;

            m_formObjDic.TryGetValue(pFormName, out _form);
            if (_form == null)
            {
                if (m_formInfoDic.ContainsKey(pFormName))
                {
                    UIFormInfo _info = m_formInfoDic[pFormName];
                    GameObject _clone = CloneForm(_info.Prefab);
                    _clone.SetActive(false);
                    if (_clone)
                    {
                        if (_clone.GetComponent<LuaFramework.LuaBehaviour>() == null)
                        {
                            _clone.AddComponent<LuaFramework.LuaBehaviour>();
                        }
                        _form = new UIForm(_clone, _info);
                        m_formObjDic.Add(pFormName, _form);
                    }
                    else
                    {
                        Debug.Log("[UIManager][OpenForm] : 窗体加载为空 ");
                    }
                }
            }
            return _form;
        }
        public void Clear()
        {
            if (m_formObjDic != null)
                m_formObjDic.Clear();
            if (m_formInfoDic != null)
                m_formInfoDic.Clear();
            if (m_curOpenFormDic != null)
                m_curOpenFormDic.Clear();
            if (m_stackForm != null)
                m_stackForm.Clear();
        }

        /// <summary>
        /// 打开时UI窗体显示类型逻辑
        /// </summary>
        private void OnOpenFormShowTypeLogic(UIForm pForm)
        {

            switch (pForm.showType)
            {
                case UIFormShowType.Normal:

                    break;
                case UIFormShowType.PushPop: //暂时不用

                    if (m_CurOperateForm != null)
                    {
                        m_stackForm.Push(m_CurOperateForm);
                    }

                    break;
                case UIFormShowType.HideOther:

                    foreach (string key in m_curOpenFormDic.Keys)
                    {
                        m_curOpenFormDic[key].Hide();
                    }

                    break;
            }
        }

        /// <summary>
        /// 关闭时UI窗体显示类型逻辑
        /// </summary>
        private void OnCloseFormShowTypeLogic(UIForm pForm)
        {

            switch (pForm.showType)
            {
                case UIFormShowType.Normal:

                    break;
                case UIFormShowType.PushPop: 

                   
                    break;
                case UIFormShowType.HideOther:


                    break;
            }
        }

        private void CloseHotUpdateCanvas()
        {
            if (m_hotUpdateCanvas != null)
                m_hotUpdateCanvas.SetActive(false);
        }
    }

    
}


