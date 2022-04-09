using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AssetBundleFramework;
using XLua;

namespace LuaFramework
{
    public class LuaHelper
    {
        //�༭������
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
                Debug.LogError("[LuaHelper][GetLuaEnv] ���ش���lua����Ϊ��!");
                return null;
            }

        }
        

        //����lua�ļ��������Ӧ��lua��Ϣ��
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
        /// ִ��lua����
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
        /// ��ʼ��lua�ű�
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
                // Debug.Log("[LuaHelper][OnFinishLoadLuaScr] lua �ű����������� ");
            }
            catch (System.Exception e)
            {
                m_finishLoadCallBack?.Invoke(false);
                throw;
            }
      
            

        }

        /// <summary>
        /// �Զ���lua�ļ�·��
        /// </summary>
        /// <param name="luaFileName">lua�ļ���</param>
        /// <returns></returns>
        private byte[] CustomLoader(ref string luaFileName)
        {

            // string _luaPath = HotUpdate.HotUpdatePathTools.GetDeloyPath() + HotUpdate.HotUpdatePathTools.LuaDelpoyPath;
            //// Debug.Log(_luaPath);
            // //�����жϴ��� ����lua�ļ�·������ȡlua������
            // if (m_luaFileDict.ContainsKey(luaFileName))
            // {
            //     //����ڻ����п��Բ��ҳɹ�����ֱ�ӷ��ؽ����
            //     return m_luaFileDict[luaFileName];
            // }
            // else
            // {
            //     return ProcessDIR(new DirectoryInfo(_luaPath), luaFileName);
            // }

         
            // Debug.Log(_luaPath);
            //�����жϴ��� ����lua�ļ�·������ȡlua������
            if (m_luaFileDict.ContainsKey(luaFileName))
            {
                //����ڻ����п��Բ��ҳɹ�����ֱ�ӷ��ؽ����
                return m_luaFileDict[luaFileName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ����lua�з���
        /// </summary>
        /// <param name="pLuaName">lua�ļ���</param>
        /// <param name="pFunctionName">lua������</param>
        /// <param name="pArgs">lua�������صķ���ֵ</param>
        /// <returns></returns>
        public object[] CallLuaFunction(string pLuaName,string pFunctionName,params object[] pArgs)
        {
            LuaTable _table = m_luaEnv.Global.Get<LuaTable>(pLuaName);
            LuaFunction _function = _table.Get<LuaFunction>(pFunctionName);
            return _function.Call(pArgs);
        }


        /// <summary>
        /// ����lua�ļ����ƣ��ݹ�ȡ��lua������Ϣ,�ҷ��뻺�漯��
        /// </summary>
        /// <param name="pFileSysInfo">lua���ļ���Ϣ</param>
        /// <param name="pFileName">��ѯ��lua�ļ�����</param>
        /// <returns></returns>
        private void ProcessDIR(FileSystemInfo pFileSysInfo)
        {
            DirectoryInfo dirInfo = pFileSysInfo as DirectoryInfo;
            FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
            foreach (FileSystemInfo item in files)
            {
                FileInfo fileInfo = item as FileInfo;
                //��ʾһ���ļ���
                if (fileInfo == null)
                {
                    //�ݹ鴦��
                    ProcessDIR(item);
                }
                //��ʾ�ļ�����
                else
                {
                    //�õ��ļ�����ȥ����׺
                    string tmpName = item.Name.Split('.')[0];
                    if (item.Extension == ".lua"|| item.Extension==".Lua")
                    {
                        //��ȡlua�ļ������ֽ���Ϣ
                        byte[] bytes = File.ReadAllBytes(fileInfo.FullName);
                        //��ӵ����漯����
                        m_luaFileDict.Add(tmpName, bytes);
                    }
                }

            }
        }
    }
}


