using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Config;
using UIFramework;
using System.Text.RegularExpressions;

public class Test_ConfigReader : MonoBehaviour
{
    // Start is called before the first frame update

    private string jsonPath;
    void Start()
    {
        //jsonPath = Application.dataPath + "/Config/json/version.json";
        //IJsonConfig json = new ConfigReader(jsonPath, ConfigReader.ConfigType.Json);
        //foreach (string  item in json.jsoData.Keys)
        //{
        //    Debug.Log(item + " , " + json.jsoData[item]);
        //}

        //string _fsmPath = Application.dataPath + "/Config/Fsm/AIFSM.txt";
        //IFSMConfig fsm = new ConfigReader(_fsmPath, ConfigReader.ConfigType.Fsm);
        //foreach (var item in fsm.data)
        //{
        //    Debug.Log(item.Key);
        //}

        //jsonPath = UIDefine.GetConfigPath();
        //Debug.Log(jsonPath);
        //IUIFormJsonConfig json = new ConfigReader(jsonPath, ConfigReader.ConfigType.UIFormjson);
        //Debug.Log(json.formData.data.Count);
        string str = "的生大的 8011-02-21 大大";
        Regex reg = new Regex(@"\d{4}\d{1,2}-\d{1,2}");
        Match math = reg.Match(str);
        Debug.Log(math.Groups[0].Value);

        Debug.Log(str.IndexOf("1"));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
