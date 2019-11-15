using UnityEngine;
using VRTK;
public class ZXTK_FeedBack : MonoBehaviour {

    private ZXTK_Controller controller;
    private ZXTK_Prompt prompt;
    private ZXTK_Shake shake;
    void Awake () {
        prompt = gameObject.GetComponentInParent<ZXTK_Prompt>();
        shake = gameObject.GetComponentInParent<ZXTK_Shake>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "text")
        {
            prompt.TooltipOn(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip, "拾起");
            prompt.ButtonBreatheOn(SDK_BaseController.ControllerElements.Trigger, ZXTK_Global.Instance.HexToColor("FFA400FF"), 0.6f);
            prompt.ControllerSetTransparence(0.3f);
            //shake.On(.05f, .1f, .2f);//瞬移频率
            shake.On(0.1f, 0.1f, 1f);//触碰频率
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "text")
        {
            prompt.TooltipOff(VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
            prompt.ButtonBreatheOff(SDK_BaseController.ControllerElements.Trigger);
            prompt.ControllerSetTransparence(1);
            shake.Off();
        }
    }
}
