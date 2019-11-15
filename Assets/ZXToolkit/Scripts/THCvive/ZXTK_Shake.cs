using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
/// <summary>
/// 震动
/// </summary>
public class ZXTK_Shake : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    private float Strength;
    private bool active;
    private bool _switch;

    private void Awake()
    {
        trackedObject = gameObject.GetComponent<ZXTK_Controller>().trackedObject;
    }
    // Use this for initialization
    void Start () {
        _switch = false;
        active = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!_switch) return;
        if (active) VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(gameObject), Strength);
	}
    /// <summary>
    /// 开启震动
    /// </summary>
    /// <param name="strength">强度</param>
    /// <param name="duration">单次振动时长</param>
    /// <param name="pulseInterval">脉冲间隔</param>
    public void On(float strength,float duration, float pulseInterval)
    {
        if (_switch) return;
        _switch = true;
        active = true;
        Strength = strength;
        StartCoroutine(DelayChange(duration, pulseInterval));
    }

    /// <summary>
    /// 关闭震动
    /// </summary>
    public void Off()
    {
        _switch = false;
    }

    IEnumerator DelayChange(float t,float r)
    {
        while (true) 
        {
            yield return new WaitForSeconds(t);
            active = !active;
            yield return new WaitForSeconds(r);
            active = true;
        }
    }
}
