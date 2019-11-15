using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// 当手柄指向某种的物体时，不需要操作，自动出现射线
/// </summary>
public class ZXTK_AutoPoint : MonoBehaviour {

    /// <summary>
    /// 对应手柄
    /// </summary>
    private SteamVR_TrackedObject trackedObject;
    /// <summary>
    /// 射线开始的位置
    /// </summary>
    [Tooltip("射线开始的位置")]
    public Transform RayStart;
    /// <summary>
    /// 需要触发的Layer层号
    /// </summary>
    [Tooltip("需要触发的Layer层号")]
    public int Target_Layer = 8;
    /// <summary>
    /// VRTK射线
    /// </summary>
    private VRTK_Pointer _Pointer;
    private LineRenderer line;
    private RaycastHit hit;
    /// <summary>
    /// 当射线出现时，为True
    /// </summary>
    [HideInInspector]
    public bool Actice;
    [HideInInspector]
    public bool a;
    void Start()
    {
        trackedObject = gameObject.GetComponent<ZXTK_Controller>().trackedObject;
        _Pointer = gameObject.GetComponent<VRTK_Pointer>();
        line = gameObject.GetComponent<LineRenderer>();
        a = false;
        Actice = false;
    }
    void Update()
    {
        if (trackedObject.index == SteamVR_TrackedObject.EIndex.None) return;
        if (!a) return;
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
        if (_Pointer.IsActivationButtonPressed())
        {
            line.positionCount = 0;
            Actice = false;
            return;
        }
        if (Physics.Raycast(RayStart.position, transform.forward, out hit, 10))
        {
            if (hit.collider.gameObject.layer == Target_Layer)//自定义UI层
            {
                line.positionCount = 2;
                line.SetPosition(0, RayStart.position);
                line.SetPosition(1, hit.point);
                Actice = true;
                return;
            }
            else line.positionCount = 0;
        }
        else line.positionCount = 0;
        Actice = false;
    }
    /// <summary>
    /// 开启自动射线
    /// </summary>
    public void On()
    {
        a = true;
    }
    /// <summary>
    /// 关闭自动射线
    /// </summary>
    public void Off()
    {
        a = false;
    }
}
