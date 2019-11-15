using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

/// <summary>
/// 资源解析容器
/// </summary>
public class ZXTK_Analysis
{
    #region  全局单例 --> Analysis.Instance
    private static ZXTK_Analysis instance;
    public static ZXTK_Analysis Instance { get { if (instance == null) instance = new ZXTK_Analysis(); return instance; } }
    private ZXTK_Analysis() { }
    #endregion

    /// <summary>
    /// 全路径！加载NPC语音，路径样例：Application.dataPath + "/Resources/Config/小亚.xml";
    /// </summary>
    /// <param name="Path">请输入全路径，例如Application.dataPath + "/Resources/Config/小亚.xml"</param>
    /// <returns></returns>
    public List<NpcWord> LoadNpcWorld(string Path)
    {
        //路径样例：xdoc.Load(Application.dataPath + "/Resources/Config/" + NpcName + ".xml");
        List<NpcWord> npcWorlds = new List<NpcWord>();
        XmlDocument xdoc = new XmlDocument();
        xdoc.Load(Path);
        XmlElement root = xdoc.DocumentElement;
        XmlNodeList nodeList = root.SelectNodes("World");
        string AudioPath = null;
        AudioPath = root.SelectSingleNode("AudioPath").InnerText;
        try
        {
            foreach (XmlNode item in nodeList)
            {
                NpcWord npcWord = new NpcWord(
                    int.Parse(item.SelectSingleNode("number").InnerText),
                    item.SelectSingleNode("Text").InnerText,
                    (AudioClip)Resources.Load(AudioPath + item.SelectSingleNode("AudioName").InnerText, typeof(AudioClip))
                    );
                npcWorlds.Add(npcWord);
            }
            return npcWorlds;
        }
        catch (System.Exception e)
        {
            File.AppendAllText(Application.dataPath + "/Error/LoadNpcWord.txt", e.ToString());
            throw;
        }
        finally{    }
    }
}
/// <summary>
/// NPC语音和文字
/// </summary>
public class NpcWord
{
    readonly int number;
    string text;
    public int Number { get { return number; } }
    public string Text { get { return text; } set { text = text.Trim(); text = value; } }
    public AudioClip Clip { get; set; }
    public NpcWord(int number, string text, AudioClip clip)
    {
        this.number = number;
        this.text = text;
        Clip = clip;
    }
}
