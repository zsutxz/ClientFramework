using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace LuaFramework
{
    [CSharpCallLua]
    public delegate void DelegateLuaScrLive(GameObject pGameObject);

    /// <summary>
    /// lua生命周期脚本
    /// 把Unity的生命周期映射到Lua
    /// </summary>
    public class LuaBehaviour : MonoBehaviour
    {
        private DelegateLuaScrLive m_luaAwake;
        private DelegateLuaScrLive m_luaOnEnable;
        private DelegateLuaScrLive m_luaStart;
        private DelegateLuaScrLive m_luaUpdate;
        private DelegateLuaScrLive m_luaFixedUpdate;
        private DelegateLuaScrLive m_luaLateUpdate;
        private DelegateLuaScrLive m_luaOnDestory;

        private LuaTable m_luaTable;
        private LuaEnv m_env;

        private void Awake()
        {
            
            m_env = LuaHelper.Instance.GetLuaEnv();
            m_luaTable = m_env.NewTable();
            LuaTable _tmpTable = m_env.NewTable();
            _tmpTable.Set("__index", m_env.Global); 
            m_luaTable.SetMetaTable(_tmpTable); //设置元方法

            string objName = this.name; 
            //保证对象名与lua文件名一直才能进行映射
            if (objName.Contains("(Clone)"))
            {
                objName = objName.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0]; //去除 （Clone）
            }

            /* 查找(m_luaTable.GetInPath)指定路径下lua文件方法，映射为委托 */
            m_luaAwake = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".Awake");
            m_luaOnEnable = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".OnEnable");
            m_luaStart = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".Start");
            m_luaUpdate = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".Update");
            m_luaFixedUpdate = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".FixedUpdate");
            m_luaLateUpdate = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".LateUpdate");
            m_luaOnDestory = m_luaTable.GetInPath<DelegateLuaScrLive>(objName + ".OnDestroy");

            if (m_luaAwake != null)
            {
                m_luaAwake(gameObject);
            }
        }

        private void OnEnable()
        {
            if (m_luaOnEnable != null)
            {
                m_luaOnEnable(gameObject);
            }
        }

        private void Start()
        {
            if (m_luaStart != null)
            {
                m_luaStart(gameObject);
            }
        }

        private void Update()
        {
            if (m_luaUpdate != null)
            {
                m_luaUpdate(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (m_luaFixedUpdate != null)
            {
                m_luaFixedUpdate(gameObject);
            }
        }

        private void LateUpdate()
        {
            if (m_luaLateUpdate != null)
            {
                m_luaLateUpdate(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (m_luaOnDestory != null)
            {
                m_luaOnDestory(gameObject);
            }
        }
    }
}

