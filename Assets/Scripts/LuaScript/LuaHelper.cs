using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AssetBundleFramework;
using XLua;

namespace LuaFramework
{
    public class LuaHelper
    {
        //编辑器环境
        static public bool isEditorEnvironment = true;

        static private LuaHelper m_instance;

       
        static public LuaHelper Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new LuaHelper();
                return m_instance;
            }
        }

        
        static private LuaEnv m_luaEnv;
       

        public LuaEnv GetLuaEnv()
        {
            if (m_luaEnv != null)
            {
                return m_luaEnv;
            }
            else
            {
                Debug.LogError("[LuaHelper][GetLuaEnv] 严重错误，lua环境为空!");
                return null;
            }

        }
        

        //缓存lua文件名称与对应的lua信息。
        private Dictionary<string, byte[]> m_luaFileDict = new Dictionary<string, byte[]>();
        private AssetPackageInfo m_packInfo;
        private System.Action<bool> m_finishLoadCallBack;
        private bool m_finishLoad = false;

        public LuaHelper()
        {
            m_luaEnv = new LuaEnv();
            m_luaEnv.AddLoader(CustomLoader);
        }

        /// <summary>
        /// 执行lua代码
        /// </summary>
        /// <param name="pChunk"></param>
        /// <param name="pChunkName"></param>
        /// <param name="pTable"></param>
        public void DoString(string pluaName,string pChunkName = "chunk", LuaTable pTable =null)
        {
            if (!m_finishLoad)
                return;

            string _requireLua = string.Format("require('{0}')", pluaName.Trim());
            m_luaEnv.DoString(_requireLua, pChunkName, pTable);
        }

        /// <summary>
        /// 初始化lua脚本
        /// </summary>
        /// <param name="pCallBack"></param>
        public void InitLuaScr(System.Action<bool> pCallBack)
        {
            if (m_finishLoad)
                return;

            m_finishLoad = true;
            m_finishLoadCallBack = pCallBack;

            if (isEditorEnvironment)
            {
                string _luaPath = Application.dataPath + HotUpdate.HotUpdatePathTools.LuaEditorPath;
                ProcessDIR(new DirectoryInfo(_luaPath));
                m_finishLoadCallBack?.Invoke(true);
            }
            else
            {
                m_packInfo = new AssetPackageInfo();
                m_packInfo.moduleName = "scr";
                m_packInfo.packageName = "scr/scr.ab";
                AssetBundleMgr.Instance.LoadAssetPackage(m_packInfo, OnFinishLoadLuaScr);
            }
   
        }

        private void OnFinishLoadLuaScr(string pPackageName)
        {
            try
            {
                Object[] _objs = AssetBundleMgr.Instance.LoadAllAsset(m_packInfo);
                foreach (Object scr in _objs)
                {
                    TextAsset _scrAsset = scr as TextAsset;
                    byte[] _buffer = System.Text.Encoding.UTF8.GetBytes(_scrAsset.ToString());
                    if (!m_luaFileDict.ContainsKey(scr.name))
                    {
                        m_luaFileDict.Add(scr.name, _buffer);
                    }
                }
                m_finishLoadCallBack?.Invoke(true);
                // Debug.Log("[LuaHelper][OnFinishLoadLuaScr] lua 脚本加载完成完成 ");
            }
            catch (System.Exception e)
            {
                m_finishLoadCallBack?.Invoke(false);
                throw;
            }
      
            

        }

        /// <summary>
        /// 自定义lua文件路径
        /// </summary>
        /// <param name="luaFileName">lua文件名</param>
        /// <returns></returns>
        private byte[] CustomLoader(ref string luaFileName)
        {

            // string _luaPath = HotUpdate.HotUpdatePathTools.GetDeloyPath() + HotUpdate.HotUpdatePathTools.LuaDelpoyPath;
            //// Debug.Log(_luaPath);
            // //缓存判断处理： 根据lua文件路径，获取lua的内容
            // if (m_luaFileDict.ContainsKey(luaFileName))
            // {
            //     //如果在缓存中可以查找成功，则直接返回结果。
            //     return m_luaFileDict[luaFileName];
            // }
            // else
            // {
            //     return ProcessDIR(new DirectoryInfo(_luaPath), luaFileName);
            // }

         
            // Debug.Log(_luaPath);
            //缓存判断处理： 根据lua文件路径，获取lua的内容
            if (m_luaFileDict.ContainsKey(luaFileName))
            {
                //如果在缓存中可以查找成功，则直接返回结果。
                return m_luaFileDict[luaFileName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 调用lua中方法
        /// </summary>
        /// <param name="pLuaName">lua文件名</param>
        /// <param name="pFunctionName">lua方法名</param>
        /// <param name="pArgs">lua方法返回的返回值</param>
        /// <returns></returns>
        public object[] CallLuaFunction(string pLuaName,string pFunctionName,params object[] pArgs)
        {
            LuaTable _table = m_luaEnv.Global.Get<LuaTable>(pLuaName);
            LuaFunction _function = _table.Get<LuaFunction>(pFunctionName);
            return _function.Call(pArgs);
        }


        /// <summary>
        /// 根据lua文件名称，递归取得lua内容信息,且放入缓存集合
        /// </summary>
        /// <param name="pFileSysInfo">lua的文件信息</param>
        /// <param name="pFileName">查询的lua文件名称</param>
        /// <returns></returns>
        private void ProcessDIR(FileSystemInfo pFileSysInfo)
        {
            DirectoryInfo dirInfo = pFileSysInfo as DirectoryInfo;
            FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo item in files)
            {
                FileInfo fileInfo = item as FileInfo;
                //表示一个文件夹
                if (fileInfo == null)
                {
                    //递归处理
                    ProcessDIR(item);
                }
                //表示文件本身
                else
                {
                    //得到文件本身，去掉后缀
                    string tmpName = item.Name.Split('.')[0];
                    if (item.Extension == ".lua"|| item.Extension==".Lua")
                    {
                        //读取lua文件内容字节信息
                        byte[] bytes = File.ReadAllBytes(fileInfo.FullName);
                        //添加到缓存集合中
                        m_luaFileDict.Add(tmpName, bytes);
                    }
                }

            }
        }
    }
}


