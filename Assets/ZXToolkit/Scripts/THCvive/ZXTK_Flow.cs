using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 流程号
/// </summary>
public class ZXTK_Flow {

    private static ZXTK_Flow instance;
    public static ZXTK_Flow Instance
    {
        get
        {
            if (instance == null) instance = new ZXTK_Flow();
            return instance;
        }
    }
    private ZXTK_Flow()
    {

    }
    /// <summary>
    /// 流程号
    /// </summary>
    public int Index = 0; 
}
