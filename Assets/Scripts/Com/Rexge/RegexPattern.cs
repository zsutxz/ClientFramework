using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegexPattern 
{
    /// <summary>
    /// 匹配时间
    /// </summary>
    public const string Time = @"\d\d\d\d/\d\d/\d\d";

    /// <summary>
    /// 注册时非法格式
    /// </summary>
    public const string AccountIllegal = @"[^.]";

}
