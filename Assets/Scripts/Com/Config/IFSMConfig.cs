using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// fsm½Ó¿Ú
    /// </summary>
    public interface IFSMConfig
    {
        Dictionary<string, Dictionary<string, string>> data { get; }
    }
}

