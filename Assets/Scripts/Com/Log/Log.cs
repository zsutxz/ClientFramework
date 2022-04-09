using Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using CustomEvent;

public class Log
{
    static private Log m_instance;
    static public Log Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new Log();
            return m_instance;
        }
    }

    private bool m_isExceptionLog = true;
    private bool m_isProcessLog = true;
    private bool m_isScreenLog = false;
    //  private int m_maxLogMsgCount = 20;
    private bool m_isSaving = false;
    private Queue<string> m_ExceptionQueue;
    private Queue<string> m_ProcessQueue;
    public ILogSetting logSett;

    private string exceptionLogFileName
    {
        get { return "/ExceptionLog.txt"; }
    }

    private string processLogFileName
    {
        get { return "/processLog.txt"; }
    }

    private string NowTime { get { return DateTime.Now.ToString("[HH:mm:ss]"); } }
    private string LogDirectoryName
    {
        get { return DateTime.Now.ToString("yyyy-M-dd"); }
    }

    private string LogDirectoryPath
    {
        get { return EnviromentPath.GetPlatformLogPath() + LogDirectoryName; }
    }

    public void Init()
    {
        m_ProcessQueue = new Queue<string>();
        m_ExceptionQueue = new Queue<string>();
        EventMgr.Register(EventID.ProjectStartUp, OnProjectStart);
        DelMoreThanThreeDayDir();
        InitLogDir();

    }

   

    //删除超过三天的log文件夹
    private void DelMoreThanThreeDayDir()
    {
        int _todays = 3;
        if (Directory.Exists(EnviromentPath.GetPlatformLogPath()))
        {
            DirectoryInfo _dirInfo = new DirectoryInfo(EnviromentPath.GetPlatformLogPath());
            DirectoryInfo[] _dirArr = _dirInfo.GetDirectories();
            DateTime _todayTime = DateTime.Now;
            for (int i = 0; i < _dirArr.Length; i++)
            {
                DateTime _creataTime = Directory.GetCreationTime(_dirArr[i].FullName);
                TimeSpan _span = new TimeSpan(_todayTime.Ticks - _creataTime.Ticks);
                if(_span.TotalDays> _todays)
                {
                   Directory.Delete(_dirArr[i].FullName, true);
                }
            }
   
        }

    }


    //初始化当天log文件夹
    private void InitLogDir()
    {
        string _path = LogDirectoryPath.Replace("/", "\\");
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }



        if (!File.Exists(_path + exceptionLogFileName))
        {
            File.Create(_path + exceptionLogFileName);
        }
        if (!File.Exists(_path + processLogFileName))
        {
            File.Create(_path + processLogFileName);
        }
    }

    private void OnProjectStart(int pEventId, object pSelfArg, object pTriggerArg)
    {
        logSett = new ConfigReader(ConfigDefine.LogSetting, ConfigReader.ConfigType.LogSetting);
        m_isScreenLog = bool.Parse(logSett.logSetting.ScreenLog);
    }
 

    public Log()
    {
        Application.logMessageReceivedThreaded += OnLogMessageReceivedThreadedCallBack;

    }

    private void OnLogMessageReceivedThreadedCallBack(string condition, string stackTrace, LogType type)
    {
        /*
         condition   --> log内容
         stackTrace  --> 堆栈信息
         type        --> 输出类型
         */


        switch (type)
        {
            case LogType.Error:
            case LogType.Warning:
            case LogType.Log:
            case LogType.Assert:
                m_ProcessQueue.Enqueue(condition);
                break;
            case LogType.Exception:  //异常报错，如空指针
                m_ExceptionQueue.Enqueue(stackTrace);
                ExceptionLog();
                LogInScreen(stackTrace);
                break;
            default:
                break;
        }

    }

    static public void LogColor(object pMsg, Color pColor)
    {
        string _strColor = ColorUtility.ToHtmlStringRGB(pColor);
        string _format = string.Format("<color=#{0}>{1}</color>", _strColor, pMsg.ToString());
        Debug.Log(_format);
    }

    //异常log
    private void ExceptionLog()
    {
        //if (m_ExceptionQueue.Count <= m_maxLogMsgCount)
        //    return;

        if (!m_isExceptionLog)
            return;

        if (m_isSaving)
            return;
        m_isSaving = true;

        if (string.IsNullOrEmpty(EnviromentPath.GetPlatformLogPath()))
            return;
        WriteExceptionLog();
    }


    private void WriteExceptionLog()
    {

        string _content = string.Empty;
        StringBuilder _sbu = new StringBuilder();

        while (m_ExceptionQueue.Count > 0)
        {
            _sbu.Append(NowTime);
            _sbu.Append(m_ExceptionQueue.Dequeue());
            _sbu.Append("\r");
            _sbu.Append("--------------------------------------------------------------------------");
            _sbu.Append("\r");
        }
        _content = _sbu.ToString();
        string _path = LogDirectoryPath.Replace("/", "\\");
        Helper.FileHelper.AsyncWriteFileData(_path + exceptionLogFileName, _content, CallBack);
    }

    private void CallBack()
    {
        m_isSaving = false;
    }

    public void LogInScreen(string pContent)
    {
        if (m_isScreenLog)
            DebugConsole.Instance.Log(pContent);
    }


    private void SaveProcessLog()
    {
        StringBuilder _sbu = new StringBuilder();
        while (m_ProcessQueue.Count > 0)
        {

            _sbu.Append(NowTime);
            _sbu.Append(m_ProcessQueue.Dequeue());
            _sbu.Append("\r");
            _sbu.Append("--------------------------------------------------------------------------");
            _sbu.Append("\r");
        }
        string _path = LogDirectoryPath.Replace("/", "\\");
        File.AppendAllText(_path + processLogFileName, _sbu.ToString());
    }

    private void SaveExceptionLog()
    {
        StringBuilder _sbu = new StringBuilder();
        while (m_ExceptionQueue.Count > 0)
        {

            _sbu.Append(NowTime);
            _sbu.Append(m_ExceptionQueue.Dequeue());
            _sbu.Append("\r");
            _sbu.Append("--------------------------------------------------------------------------");
            _sbu.Append("\r");
        }
        string _path = LogDirectoryPath.Replace("/", "\\");
        File.AppendAllText(_path + exceptionLogFileName, _sbu.ToString());
    }

    public void SaveLogOnExit()
    {
        EventMgr.Remove(EventID.ProjectStartUp, OnProjectStart);
        if (m_isProcessLog)
            SaveProcessLog();
        if (m_isExceptionLog)
            SaveExceptionLog();
    }
}
