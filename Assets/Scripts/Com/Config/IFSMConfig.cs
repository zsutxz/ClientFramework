using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// fsm�ӿ�
    /// </summary>
    public interface IFSMConfig
    {
        Dictionary<string, Dictionary<string, string>> data { get; }
    }
}

