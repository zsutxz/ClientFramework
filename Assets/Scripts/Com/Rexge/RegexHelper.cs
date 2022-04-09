using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RegexHelper : MonoBehaviour
{
    /// <summary>
    /// 获取string中日期
    /// </summary>
    /// <param name="pStr">字符串</param>
    /// <returns></returns>
    static public string GetStringDate(string pStr)
    {
        Regex _gex = new Regex(RegexPattern.Time);
        return _gex.Match(pStr).Value;
    }

    static public  bool IsLegalAccount(string pStr,out string pMsg)
    {


        if(pStr.Length<6 || pStr.Length > 12)
        {
            pMsg = "账号只能设置6-12为长度";
            return false;
        }
        else
        {
            string _parrent = @"[^\d]";
            string _firstLetter = pStr.Substring(0,1);
            Regex _gex = new Regex(_parrent);
            bool _resut = _gex.IsMatch(pStr);
            if (_resut)
            {
                pMsg = string.Empty;
                return true;
            }
            else
            {
                pMsg = "账号必须是字母开头";
                return false;
            }
        }
    }
}
