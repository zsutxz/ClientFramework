using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RegexHelper : MonoBehaviour
{
    /// <summary>
    /// ��ȡstring������
    /// </summary>
    /// <param name="pStr">�ַ���</param>
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
            pMsg = "�˺�ֻ������6-12Ϊ����";
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
                pMsg = "�˺ű�������ĸ��ͷ";
                return false;
            }
        }
    }
}
