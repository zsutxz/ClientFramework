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
    /// lua�������ڽű�
    /// ��Unity����������ӳ�䵽Lua
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
            m_luaTable.SetMetaTable(_tmpTable); //����Ԫ����

            string objName = this.name; 
            //��֤��������lua�ļ���һֱ���ܽ���ӳ��
            if (objName.Contains("(Clone)"))
            {
                objName = objName.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0]; //ȥ�� ��Clone��
            }

            /* ����(m_luaTable.GetInPath)ָ��·����lua�ļ�������ӳ��Ϊί�� */
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

