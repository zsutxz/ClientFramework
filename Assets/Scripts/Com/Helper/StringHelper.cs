using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Helper 
{
    public class StringHelper
    {
        
        /// <summary>
        /// �滻�ַ�����������
        /// </summary>
        /// <param name="pContent">ԭ����</param>
        /// <param name="pReplaceStr">�滻�������ַ���</param>
        /// <returns></returns>
        static public string ReplaceSensitiveWords(string pContent,string pReplaceStr)
        {
            Config.ISersitiveWordConfig sersitiveWordConfig = new Config.ConfigReader(Config.ConfigDefine.SensitiveWord, Config.ConfigReader.ConfigType.SensitiveWord);
            return  Regex.Replace(pContent, sersitiveWordConfig.word, pReplaceStr);
        }
    }

}

