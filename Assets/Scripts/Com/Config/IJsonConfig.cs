using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config 
{
    /// <summary>
    /// ¼òµ¥json½Ó¿Ú
    /// </summary>
    public interface IJsonConfig
    {
        Dictionary<string, string> jsoData { get; }
    }

    [Serializable]
    public class JsonInfo
    {
        public List<JsonNode> ConfigInfo = null;
    }

    [Serializable]
    public class JsonNode
    {

        public string key = null;

        public string value = null;
    }
}




