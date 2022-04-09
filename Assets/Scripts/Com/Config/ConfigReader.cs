using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Config
{
    /// <summary>
    /// ≈‰÷√Œƒº˛∂¡»°
    /// </summary>
    public partial class ConfigReader
    {
        public enum ConfigType
        {
            Json,
            UIFormjson,
            Fsm,
            Xml,
            Csv,
            SensitiveWord,
            LogSetting
        }


        public ConfigReader(string pConfigPath, ConfigType pConfigType)
        {
            pConfigPath = ConfigDefine.GetConfigPath() + pConfigPath;
            switch (pConfigType)
            {
                case ConfigType.Json:
                    jsoData = new Dictionary<string, string>();
                    ReadJsoData(pConfigPath);
                    break;
                case ConfigType.UIFormjson:
                    ReadUIFormJson(pConfigPath);
                    break;
                case ConfigType.Fsm:
                    data = new Dictionary<string, Dictionary<string, string>>();
                    ReadFsm(pConfigPath);
                    break;
                case ConfigType.Xml:
                    break;
                case ConfigType.Csv:
                    break;
                case ConfigType.SensitiveWord:
                    ReadSersitiveWord(pConfigPath);
                    break;
                case ConfigType.LogSetting:
                    ReadLogSetting(pConfigPath);
                    break;
                default:
                    break;
            }
        }
    }

    public partial class ConfigReader : IJsonConfig
    {

        public Dictionary<string, string> jsoData { get; private set; }
        private void ReadJsoData(string pJsonPath)
        {
            JsonInfo _json = null;
            try
            {
                string _readContent = File.ReadAllText(pJsonPath);
                _json = JsonUtility.FromJson<JsonInfo>(_readContent);
            }
            catch 
            {
                Debug.Log("[ConfigReader][ReadJsoData] : Ω‚Œˆjson¥ÌŒÛ £∫" + pJsonPath);
            }

            foreach (JsonNode node in _json.ConfigInfo)
            {
                jsoData.Add(node.key, node.value);
            }
        }
    }

    public partial class ConfigReader : IUIFormJsonConfig
    {
        public UIFormData formData { get; private set; }

        private void ReadUIFormJson(string pJsonPath)
        {
            try
            {
                string _readContent = File.ReadAllText(pJsonPath);
                formData = JsonUtility.FromJson<UIFormData>(_readContent);
            }
            catch
            {
                Debug.Log("[ConfigReader][ReadUIFormJson] : Ω‚Œˆui¥∞ÃÂjson¥ÌŒÛ £∫" + pJsonPath);
            }
        }

    }


    public partial class ConfigReader : IFSMConfig
    {

        public Dictionary<string, Dictionary<string, string>> data { get; private set; }

        private void ReadFsm(string pFsmPath)
        {
            
            StreamReader _sr = null;
            try
            {
                _sr = new StreamReader(pFsmPath);
                string _line = string.Empty;
                string _mainKey = string.Empty;
                while (!string.IsNullOrEmpty(_line = _sr.ReadLine()))
                {
                    if (_line.StartsWith("[") && _line.EndsWith("]"))
                    {
                        _mainKey = _line.Replace("[", "").Replace("]", "");
                        data.Add(_mainKey, new Dictionary<string, string>());
                    }
                    else
                    {
                        string[] _values = _line.Split('>');
                        if (!string.IsNullOrEmpty(_mainKey))
                            data[_mainKey].Add(_values[0].Trim(), _values[1].Trim());
                    }
                }
            }
            catch
            {
                Debug.Log("[ConfigReader][ReadFsm] : Ω‚Œˆui¥∞ÃÂjson¥ÌŒÛ £∫" + pFsmPath);
            }
            finally
            {
                if (_sr != null)
                {
                    _sr.Close();
                    _sr.Dispose();
                }
            }
        }
    }

    public partial class ConfigReader : ISersitiveWordConfig
    {
        public string word { get; private set; }

        public void ReadSersitiveWord(string pConfigPath)
        {
            string _readContent = File.ReadAllText(pConfigPath);
            SersitiveWord _sw = JsonUtility.FromJson<SersitiveWord>(_readContent);
            word = string.Join("|", _sw.lib);
        }
    }

    public partial class ConfigReader : ILogSetting
    {
        public LogSetting logSetting { get; private set; }

        public void ReadLogSetting(string pConfigPath)
        {
            string _readContent = File.ReadAllText(pConfigPath);
            logSetting = JsonUtility.FromJson<LogSetting>(_readContent);
       
        }
    }

}
