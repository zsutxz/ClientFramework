using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config 
{

    /// <summary>
    /// 敏感字符接口
    /// </summary>
    public interface ISersitiveWordConfig
    {
        string word { get; }
    }

    public class SersitiveWord
    {
        public string msg;
        public string[] lib;

    }
}

