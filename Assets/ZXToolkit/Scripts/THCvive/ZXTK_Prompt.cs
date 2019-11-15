using System.Collections.Generic;
using UnityEngine;
using VRTK;
/// <summary>
/// 高亮配置
/// </summary>
public class HightItem
{
    public SDK_BaseController.ControllerElements element;
    public Color color;
    public float cycle;
    public bool active;
    public HightItem(SDK_BaseController.ControllerElements ele, Color color, float cycle, bool active) { this.element = ele; this.color = color; this.cycle = cycle; }
} 
/// <summary>
/// 提示（高亮和文字提示）
/// </summary>
public class ZXTK_Prompt : MonoBehaviour {

    
    public VRTK_ControllerTooltips tooltip;
    /// <summary>
    /// 简单高亮调用
    /// </summary>
    [HideInInspector]
    public VRTK_ControllerHighlighter highlighter;
    private List<HightItem> items = new List<HightItem>();
    private Material BodyInitMaterial;
    private float time;
    // Use this for initialization
    void Start () {
        time = 0;
        highlighter = gameObject.GetComponent<VRTK_ControllerHighlighter>();
        tooltip.ResetTooltip();//初始化文字提示
    }
	void Update () {
        if (items == null) return;
        foreach (var item in items)
        {
            if (item == null) continue;
            if (!item.active) continue;
            Transform e = highlighter.GetElementTransform(highlighter.GetPathForControllerElement(item.element));
            Material material = e.gameObject.GetComponent<MeshRenderer>().material;
            time += Time.deltaTime;
            item.color.a = Mathf.Abs(Mathf.Cos((1 / item.cycle) * time));//呼吸计算
            if (material == null) return;
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            material.SetColor("_Color", item.color);
        }
    }
    /// <summary>
    /// 打开某按键提示
    /// </summary>
    /// <param name="button"></param>
    /// <param name="text"></param>
    public void TooltipOn(VRTK_ControllerTooltips.TooltipButtons button,string text)
    {
        tooltip.UpdateText(button, text);
    }
    /// <summary>
    /// 关闭某按键提示
    /// </summary>
    public void TooltipOff(VRTK_ControllerTooltips.TooltipButtons button)
    {
        tooltip.UpdateText(button, "");
    }
    /// <summary>
    /// 初始化所有提示
    /// </summary>
    public void TooltipReset()
    {
        tooltip.ResetTooltip();//初始化文字提示
    }

    /// <summary>
    /// 打开按键呼吸提示
    /// </summary>
    /// <param name="elements">按键</param>
    /// <param name="color">颜色</param>
    /// <param name="cycle">周期</param>
    public void ButtonBreatheOn(SDK_BaseController.ControllerElements elements, Color color, float cycle)
    {
        if (elements == SDK_BaseController.ControllerElements.StartMenu) elements = SDK_BaseController.ControllerElements.ButtonTwo;
        HightItem hight = new HightItem(elements, color, cycle, true);
        if (items.Contains(hight)) return;
        hight.active = true;
        items.Add(hight);
        if (items == null) return;
        foreach (var item in items)
        {
            highlighter.HighlightElement(item.element, item.color);
        }
    }
    /// <summary>
    /// 关闭按键呼吸提示
    /// </summary>
    /// <param name="elements"></param>
    public void ButtonBreatheOff(SDK_BaseController.ControllerElements elements)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].element == elements)
            {
                //关闭光亮
                items[i].active = false;
                highlighter.UnhighlightElement(elements);
                items.Remove(items[i]);
            }
        }
    }
    /// <summary>
    /// 手柄透明
    /// </summary>
    /// <param name="transparence">透明度</param>
    public void ControllerSetTransparence(float transparence)
    {
        Transform e = highlighter.GetElementTransform(highlighter.GetPathForControllerElement(SDK_BaseController.ControllerElements.Body));
        if (transparence == 1&& BodyInitMaterial)
        {
            Material m = e.gameObject.GetComponent<MeshRenderer>().material;
            m = BodyInitMaterial;
            m.SetFloat("_Mode", 0);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            m.SetInt("_ZWrite", 1);
            m.DisableKeyword("_ALPHATEST_ON");
            m.DisableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = -1;
            return;
        }
        BodyInitMaterial = e.gameObject.GetComponent<MeshRenderer>().material;
        VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(gameObject), transparence);
    }
    /// <summary>
    /// 隐藏显示器
    /// </summary>
    public void ControllerHide()
    {
        VRTK_ObjectAppearance.SetRendererHidden(VRTK_DeviceFinder.GetModelAliasController(gameObject));
    }
    /// <summary>
    /// 显示控制器
    /// </summary>
    public void ControllerVisible()
    {
        VRTK_ObjectAppearance.SetRendererVisible(VRTK_DeviceFinder.GetModelAliasController(gameObject));
    }
}
