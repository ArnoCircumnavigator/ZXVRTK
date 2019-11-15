using UnityEngine;
using VRTK;
/// <summary>
/// 手柄控制器（用于交互控制，但不具体实现）
/// </summary>
public class ZXTK_Controller : MonoBehaviour {

    /// <summary>
    /// 对应手柄控制器
    /// </summary>
    public SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    private ZXTK_Global global;
    private ZXTK_AutoPoint autoPoint;
    private ZXTK_Prompt prompt;
    private ZXTK_Shake shake;
    private void Awake()
    {
        
    }
    void Start () {
        global = GameObject.Find("[CLTOOLKIT]GLOBAL").GetComponent<ZXTK_Global>();
        autoPoint = gameObject.GetComponent<ZXTK_AutoPoint>();
        prompt = gameObject.GetComponent<ZXTK_Prompt>();
        shake = gameObject.GetComponent<ZXTK_Shake>();
    }
	
	// Update is called once per frame
	void Update () {

        if (trackedObject.index == SteamVR_TrackedObject.EIndex.None) return;
        if (device == null) device = SteamVR_Controller.Input((int)trackedObject.index);
        #region 测试函数功能
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            autoPoint.On();
        }
        //Debug.Log(autoPoint.Actice + "*****" + autoPoint.a);
        if (Input.GetKeyDown(KeyCode.W))
        {
            prompt.ButtonBreatheOn(SDK_BaseController.ControllerElements.Trigger, Global.Instance.HexToColor("FFA400FF"), .8f);
            prompt.ControllerSetTransparence(0.3f);
            prompt.TooltipOn(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "这是Trigger");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            prompt.ButtonBreatheOff(SDK_BaseController.ControllerElements.Trigger);
            prompt.ControllerSetTransparence(1f);
            prompt.TooltipOff(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
        }
        if (Input.GetKeyDown((KeyCode.R)))
        {
            Debug.Log("震动");
            shake.On(.05f, .1f, .2f);//一般的瞬移所有震动幅度
        }
        */
        #endregion
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            
        }
    }
}
