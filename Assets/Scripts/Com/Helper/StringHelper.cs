using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Helper 
{
    public class StringHelper
    {
        
        /// <summary>
        /// Ìæ»»×Ö·û´®ÖÐÃô¸Ð×Ö
        /// </summary>
        /// <param name="pContent">Ô­ÄÚÈÝ</param>
        /// <param name="pReplaceStr">Ìæ»»Ãô¸Ð×Ö×Ö·û´®</param>
        /// <returns></returns>
        static public string ReplaceSensitiveWords(string pContent,string pReplaceStr)
        {
            Config.ISersitiveWordConfig sersitiveWordConfig = new Config.ConfigReader(Config.ConfigDefine.SensitiveWord, Config.ConfigReader.ConfigType.SensitiveWord);
            return  Regex.Replace(pContent, sersitiveWordConfig.word, pReplaceStr);
        }
    }

}

