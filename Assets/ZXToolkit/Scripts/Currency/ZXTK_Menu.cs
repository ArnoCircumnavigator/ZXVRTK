using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using System.IO;
using Currency;
using VRTK;
using VRTK.SecondaryControllerGrabActions;
using VRTK.GrabAttachMechanics;
using UnityEngine.UI;
#if UNITY_EDITOR
public class BasicFolder {

    [MenuItem("ZxToolkit/创建基本文件夹 #&_b")]
    private static void CreateBasicFolder()
    {
        ZXTK_Implement.GenerateFolder();
    }
}
#endif
#if UNITY_EDITOR
public class CreatVRScene : EditorWindow
{
    private static Texture texture;
    [MenuItem("ZxToolkit/创建新的VR场景 #&_a")]
    private static void CreatWindow()
    {
        texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/ZXToolkit/Textrues/CL_log.png");
        Rect wh = new Rect(0, 0, 300, 245);
        CreatVRScene window = (CreatVRScene)EditorWindow.GetWindowWithRect(typeof(CreatVRScene), wh, true, "HTC便捷式开发助手v1.1-->张兴");
        window.Show();
    }
    private string ScenesName = "";
    public string Error = "";
    void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;//居中
        //GUIContent uIContent = new GUIContent("xxx", texture, "sss");
        //EditorGUILayout.LabelField(uIContent);
        //texture = EditorGUILayout.ObjectField("",texture, typeof(Texture), true) as Texture;
        GUILayout.Label(texture);
        GUILayout.Label("向 阳 的 黑 小 孩");
        GUILayout.Space(8);
        GUI.skin.textField.fontSize = 10;
        GUI.skin.textField.fontStyle = FontStyle.Normal;
        GUI.skin.textField.alignment = TextAnchor.MiddleCenter;//
        GUILayout.Space(10);
        ScenesName = EditorGUILayout.TextField("场景名称：", ScenesName);
        GUILayout.Space(10);
        //GUILayout.Space(90);
        if (GUILayout.Button("确认创建！"))
        {
            try
            {
                if (ScenesName == "")
                {
                    Debug.LogError("亲，给你的场景起个名字吧！");
                    this.ShowNotification(new GUIContent("失败 + 1"));
                    return;
                }
                if (!ZXTK_Implement.CreatSceneOfVR(ScenesName))
                {
                    this.ShowNotification(new GUIContent("失败 + 1"));
                    return;
                }
                this.ShowNotification(new GUIContent("创建《" + ScenesName + "》，成功 + 1"));
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                GUI.skin.label.fontSize = 23;
                GUI.skin.label.fontStyle = FontStyle.Normal;
            }
        }
        if(!Error.Equals("")) this.ShowNotification(new GUIContent(Error));
    }
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
    /*
    void OnFocus()
    {
        //Debug.Log("当窗口获得焦点时调用一次");
    }

    void OnLostFocus()
    {
        //Debug.Log("当窗口失去焦点时调用一次");
    }

    void OnHierarchyChange()
    {
        //Debug.Log("当Hierarchy视图中的任何对象发生改变时调用");
    }

    void OnInspectorUpdate()
    {
        //重新绘制
        this.Repaint();
    }
    

    void OnSelectionChange()
    {
        //当窗口出去开启壮体啊，并且在Hierarchy视图中选择某个游戏对象时调用
        foreach (Transform t in Selection.transforms)
        {
            this.ShowNotification(new GUIContent("当前配置物体，已更改为：" + Selection.activeObject.name));
        }
    }

    void OnDestroy()
    {
        //Debug.Log("当窗口关闭时调用");
    }
    */
}
#endif
#if UNITY_EDITOR
public class AddVRComponent
{
    [MenuItem("ZxToolkit/为当前场景添加VR环境 #&_a")]
    private static void AddVRcomponent()
    {
        ZXTK_Implement.CreatePrefabOfVR();
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("已为当前场景添加VR环境！");
    }
}
#endif
#if UNITY_EDITOR
public class ObjectSetUp : EditorWindow
{
    private static Texture texture;
    private enum PrimaryGrab
    {
        [Tooltip("交互物体当被抓取时会自动的成为手柄的子对象，使物体自然的跟随手柄移动和旋转。")]
        父子物体式,
        [Tooltip("抓取时，在手柄和抓取物体之间建立一个Fixed Joint来连接")]
        FixedJoint连接式,
        [Tooltip("使用这种触碰抓取方式的物体会被标记为一个可攀爬的交互对象")]
        攀爬式,
        [Tooltip("在可交互物体上自定义一个抓取关节，运行时会把这个关节作为交互物体的子对象")]
        关节式,
        [Tooltip("这种抓取方式是给物体施加一个控制器方向的力让它旋转，适用于门或者车轮等物体")]
        添加力,
        [Tooltip("在手柄和抓取物体之间创建一个Spring Joint来连接")]
        SpringJoint连接式,
        [Tooltip("这种抓取方式物体只是跟随手柄方向进行移动，当碰到其他刚体时，超过限制就会掉落。")]
        模拟碰撞式
    }
    private enum SecondaryGrab
    {
        换手,
        控制方向,
        缩放
    }
    private bool useGrab = true;
    private bool holdGrab = false;
    private bool useUse = false;
    private bool useIfGrabbed = false;
    private bool holdUse = false;
    private PrimaryGrab primGrab;
    private SecondaryGrab secGrab;
    private bool disableIdle = true;
    private bool addrb = true;
    private bool addHaptics = true;
    private Color touchColor = Color.clear;

    [MenuItem("ZxToolkit/配置VR交互物体 #&_v")]
    private static void Collocate()
    {
        if (!Selection.activeObject)
        {
            Debug.LogError("伙计，在Hierarchy面板上至少选择一个物体，不然我不知道配置那个（些）！");
            return;
        }
        texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/ZXToolkit/Textrues/CL_log.png");
        Rect wh = new Rect(0, 0, 600, 610);
        ObjectSetUp window = (ObjectSetUp)EditorWindow.GetWindowWithRect(typeof(ObjectSetUp), wh, true, "HTC便捷式开发助手v1.1-->张兴");
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;//居中
        GUILayout.Label(texture);
        GUILayout.Label("向 阳 的 黑 小 孩");
        GUILayout.Space(8);
        GUI.skin.label.fontStyle = FontStyle.Normal;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("【触碰设置】", EditorStyles.boldLabel);
        touchColor = EditorGUILayout.ColorField("触碰时高亮颜色", touchColor);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("【抓取设置】", EditorStyles.boldLabel);
        useGrab = EditorGUILayout.Toggle("是否可抓取？", useGrab);
        holdGrab = EditorGUILayout.Toggle("长按抓取？", holdGrab);
        EditorGUILayout.Space();
        primGrab = (PrimaryGrab)EditorGUILayout.EnumPopup("单手抓取方式", primGrab);
        string temp = "";
        switch (primGrab)
        {
            case PrimaryGrab.父子物体式:
                temp = "交互物体当被抓取时会自动的成为手柄的子对象，使物体自然的跟随手柄移动和旋转。";
                break;
            case PrimaryGrab.FixedJoint连接式:
                temp = "抓取时，在手柄和抓取物体之间建立一个Fixed Joint来连接。";
                break;
            case PrimaryGrab.攀爬式:
                temp = "使用这种触碰抓取方式的物体会被标记为一个可攀爬的交互对象";
                break;
            case PrimaryGrab.关节式:
                temp = "在可交互物体上自定义一个抓取关节，运行时会把这个关节作为交互物体的子对象。";
                break;
            case PrimaryGrab.SpringJoint连接式:
                temp = "在手柄和抓取物体之间创建一个Spring Joint来连接。";
                break;
            case PrimaryGrab.模拟碰撞式:
                temp = "这种抓取方式物体只是跟随手柄方向进行移动，当碰到其他刚体时，超过限制就会掉落。";
                break;
        }
        //GUILayout.BeginVertical();
        GUI.skin.label.fontSize = 10;
        GUI.skin.label.fontStyle = FontStyle.Normal;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("   解释：" + temp, GUILayout.MaxHeight(20));
        //temp = EditorGUILayout.TextArea(temp, GUILayout.MaxHeight(100));
        //GUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        string temp1 = "";
        switch (secGrab)
        {
            case SecondaryGrab.换手:
                temp1 = "一只手抓握，第二只手上来时，给第二只手（换手操作）。";
                break;
            case SecondaryGrab.控制方向:
                temp1 = "两只手同时握着一个东西，比如机枪，等很重的,需要两只手一起拿东西";
                break;
            case SecondaryGrab.缩放:
                temp1 = "通过两只手的距离，来控制缩放";
                break;
        }
        secGrab = (SecondaryGrab)EditorGUILayout.EnumPopup("双手同时抓取方式", secGrab);
        GUILayout.Label("   解释：" + temp1, GUILayout.MaxHeight(20));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("【使用设置】", EditorStyles.boldLabel);
        useUse = EditorGUILayout.Toggle("是否可以使用？", useUse);
        holdUse = EditorGUILayout.Toggle("长按使用？", holdUse);
        useIfGrabbed = EditorGUILayout.Toggle("仅当抓取时使用？", useIfGrabbed);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("【优化设置】", EditorStyles.boldLabel);
        disableIdle = EditorGUILayout.Toggle("空闲时，不启用对应组件？", disableIdle);
        addrb = EditorGUILayout.Toggle("添加刚体？", addrb);
        addHaptics = EditorGUILayout.Toggle("添加震动组件？", addHaptics);
        EditorGUILayout.Space();
        if (GUILayout.Button("设置当前Hierarchy面板选中物体！", GUILayout.Height(40))) SetupObject();
    }
    void OnInspectorUpdate()
    {
        this.Repaint();
    }
    public void SetupObject()
    {
        Transform[] transforms = Selection.transforms;
        foreach (Transform transform in transforms)
        {
            GameObject go = transform.gameObject;
            VRTK_InteractableObject intObj = go.GetComponent<VRTK_InteractableObject>();
            if (intObj == null)
            {
                intObj = go.AddComponent<VRTK_InteractableObject>();
            }
            intObj.touchHighlightColor = touchColor;
            intObj.isGrabbable = useGrab;
            intObj.holdButtonToGrab = holdGrab;
            intObj.isUsable = useUse;
            intObj.disableWhenIdle = disableIdle;
            intObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            intObj.useOverrideButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            VRTK_BaseGrabAttach grab = go.GetComponent<VRTK_BaseGrabAttach>();
            if (grab != null)
            {
                DestroyImmediate(grab);
            }
            switch (primGrab)
            {
                case PrimaryGrab.父子物体式:
                    grab = go.AddComponent<VRTK_ChildOfControllerGrabAttach>();
                    break;
                case PrimaryGrab.FixedJoint连接式:
                    grab = go.AddComponent<VRTK_FixedJointGrabAttach>();
                    break;
                case PrimaryGrab.攀爬式:
                    grab = go.AddComponent<VRTK_ClimbableGrabAttach>();
                    break;
                case PrimaryGrab.关节式:
                    grab = go.AddComponent<VRTK_CustomJointGrabAttach>();
                    break;
                case PrimaryGrab.添加力:
                    grab = go.AddComponent<VRTK_RotatorTrackGrabAttach>();
                    break;
                case PrimaryGrab.SpringJoint连接式:
                    grab = go.AddComponent<VRTK_SpringJointGrabAttach>();
                    break;
                case PrimaryGrab.模拟碰撞式:
                    grab = go.AddComponent<VRTK_TrackObjectGrabAttach>();
                    break;
                default:
                    grab = go.AddComponent<VRTK_ChildOfControllerGrabAttach>();
                    break;
            }
            intObj.grabAttachMechanicScript = grab;
            VRTK_BaseGrabAction grab2 = go.GetComponent<VRTK_BaseGrabAction>();
            if (grab2 != null)
            {
                DestroyImmediate(grab2);
            }
            switch (secGrab)
            {
                case SecondaryGrab.换手:
                    grab2 = go.AddComponent<VRTK_SwapControllerGrabAction>();
                    break;
                case SecondaryGrab.控制方向:
                    grab2 = go.AddComponent<VRTK_ControlDirectionGrabAction>();
                    break;
                case SecondaryGrab.缩放:
                    grab2 = go.AddComponent<VRTK_AxisScaleGrabAction>();
                    break;
                default:
                    grab2 = go.AddComponent<VRTK_SwapControllerGrabAction>();
                    break;
            }
            intObj.secondaryGrabActionScript = grab2;
            if (addrb)
            {
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    go.AddComponent<Rigidbody>();
                }
            }
            if (addHaptics)
            {
                VRTK_InteractHaptics haptics = go.GetComponent<VRTK_InteractHaptics>();
                if (haptics == null)
                {
                    go.AddComponent<VRTK_InteractHaptics>();
                }
            }
        }
    }
}
#endif
