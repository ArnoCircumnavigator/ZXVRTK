using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 通用（挂载 [CLTOOLKIT]GLOBAL  上面）
/// </summary>
public class ZXTK_Global : MonoBehaviour {

    private static ZXTK_Global instance;
    private ZXTK_Global(){}
    public static ZXTK_Global Instance
    {
        get
        {
            if (instance == null) instance = new ZXTK_Global();
            return instance;
        }
    }

    void Awake()
    {
        
    }
    void Start ()
    {
        
    }
	
	void Update ()
    {
		
	}
    /// <summary>
    /// VR切换场景
    /// </summary>
    /// <param name="SceneName">场景名字</param>
    public void LoadScenes(string SceneName)
    {
        try
        {
            SteamVR_LoadLevel level = GameObject.Find("[CLTOOLKIT]LoadScene").GetComponent<SteamVR_LoadLevel>();
            level.levelName = SceneName;
            level.Trigger();
        }
        catch (System.Exception){throw;}
    }
    /// <summary>
    /// 颜色转Hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }
    /// <summary>
    /// Hex转颜色
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
    /// <summary>
    /// TimeLine倒带
    /// </summary>
    /// <param name="director"></param>
    public void ReWinding(PlayableDirector director)
    {
        StartCoroutine(tRewind(director));
    }
    public IEnumerator tRewind(PlayableDirector director)
    {
        yield return new WaitForSeconds(0.001f * Time.deltaTime);
        director.time -= 1.0f * Time.deltaTime;  //1.0f是倒帶速度
        director.Evaluate();//director播放当前帧
        if (director.time < 0f)
        {
            director.time = 0f;
            director.Evaluate();
        }
        else
        {
            StartCoroutine(tRewind(director));
        }
    }
}
