using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    public interface ILogSetting
    {
        LogSetting logSetting { get; }
    }

    public class LogSetting
    {
        public string msg;
        public string ScreenLog;
    }
}

