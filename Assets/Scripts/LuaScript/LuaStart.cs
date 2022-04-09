using UnityEngine;
using LuaFramework;
using CustomEvent;
using System;

public class LuaStart : MonoBehaviour
{
    private bool m_isStart;
    private void Awake()
    {
        EventMgr.Register(EventID.ProjectStartUp, ProjectStart);
    }

    private void ProjectStart(int pEventId, object pSelfArg, object pTriggerArg)
    {
        if (m_isStart)
            return;
        LuaHelper.Instance.InitLuaScr(OnFinish);
        m_isStart = true;
    }


    private void OnFinish(bool pResult)
    {
        if (pResult)
        {
            LuaHelper.Instance.DoString("LuaStart");
            Log.Instance.LogInScreen("[luaøÚº‹∆Ù∂Ø]");
        }
        else
        {
            Debug.Log("[LuaStart][OnFinish] lua Ω≈±æº”‘ÿ ß∞‹");
            Log.Instance.LogInScreen("[LuaStart][OnFinish] lua Ω≈±æº”‘ÿ ß∞‹");
        }

    }

    private void OnDestroy()
    {
        EventMgr.Remove(EventID.ProjectStartUp, ProjectStart);
        try
        {
            LuaHelper.Instance.CallLuaFunction("HotFix", "RemoveAllHotFix");
        }
        catch
        {

        }
    }

}
