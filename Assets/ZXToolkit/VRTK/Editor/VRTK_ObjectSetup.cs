namespace VRTK
{
    using UnityEngine;
    using UnityEditor;
    using VRTK.GrabAttachMechanics;
    using VRTK.SecondaryControllerGrabActions;
    public class VRTK_ObjectSetup : EditorWindow
    {
        private enum PrimaryGrab
        {
            ChildOfController,
            FixedJoint,
            Climbable,
            CustomJoint,
            RotatorTrack,
            SpringJoint,
            TrackObject
        }
        private enum SecondaryGrab
        {
            SwapControllers,
            ControlDirection,
            AxisScale
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

        //[MenuItem("Window/VRTK/Setup Interactable Object")]
        public static void Init()
        {
            Rect wh = new Rect(0, 0, 300, 420);
            VRTK_ObjectSetup window = (VRTK_ObjectSetup)EditorWindow.GetWindowWithRect(typeof(VRTK_ObjectSetup), wh, true, "策蓝科技便捷式开发助手->张兴");
            window.Show();
            //VRTK_ObjectSetup window = (VRTK_ObjectSetup)EditorWindow.GetWindow(typeof(VRTK_ObjectSetup));
            // window.minSize = new Vector2( 300f, 370f );
            //window.maxSize = new Vector2( 400f, 400f );
            //window.autoRepaintOnSceneChange = true;
            //window.titleContent.text = "配置VR物体ing";
            //window.Show();
        }
        private void OnGUI()
        {
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;//居中
            GUILayout.Label("C L K J");
            GUILayout.Space(8);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("【触碰设置】", EditorStyles.boldLabel);
            touchColor = EditorGUILayout.ColorField("触碰时高亮颜色", touchColor);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("【抓取设置】", EditorStyles.boldLabel);
            useGrab = EditorGUILayout.Toggle("是否可抓取？", useGrab);
            holdGrab = EditorGUILayout.Toggle("长按抓取？", holdGrab);
            EditorGUILayout.Space();
            primGrab = (PrimaryGrab)EditorGUILayout.EnumPopup("抓取方式", primGrab);
            secGrab = (SecondaryGrab)EditorGUILayout.EnumPopup("二级联动抓取方式", secGrab);
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
            if(GUILayout.Button("设置当前Hierarchy面板选中物体！", GUILayout.Height(40))) SetupObject();
        }

        public void SetupObject()
        {
            Transform[] transforms = Selection.transforms;
            foreach(Transform transform in transforms)
            {
                GameObject go = transform.gameObject;
                VRTK_InteractableObject intObj = go.GetComponent<VRTK_InteractableObject>();
                if(intObj == null)
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
                if(grab != null)
                {
                    DestroyImmediate(grab);
                }
                switch(primGrab)
                {
                    case PrimaryGrab.ChildOfController:
                        grab = go.AddComponent<VRTK_ChildOfControllerGrabAttach>();
                        break;
                    case PrimaryGrab.FixedJoint:
                        grab = go.AddComponent<VRTK_FixedJointGrabAttach>();
                        break;
                    case PrimaryGrab.Climbable:
                        grab = go.AddComponent<VRTK_ClimbableGrabAttach>();
                        break;
                    case PrimaryGrab.CustomJoint:
                        grab = go.AddComponent<VRTK_CustomJointGrabAttach>();
                        break;
                    case PrimaryGrab.RotatorTrack:
                        grab = go.AddComponent<VRTK_RotatorTrackGrabAttach>();
                        break;
                    case PrimaryGrab.SpringJoint:
                        grab = go.AddComponent<VRTK_SpringJointGrabAttach>();
                        break;
                    case PrimaryGrab.TrackObject:
                        grab = go.AddComponent<VRTK_TrackObjectGrabAttach>();
                        break;
                    default:
                        grab = go.AddComponent<VRTK_ChildOfControllerGrabAttach>();
                        break;
                }
                intObj.grabAttachMechanicScript = grab;
                VRTK_BaseGrabAction grab2 = go.GetComponent<VRTK_BaseGrabAction>();
                if(grab2 != null)
                {
                    DestroyImmediate(grab2);
                }
                switch(secGrab)
                {
                    case SecondaryGrab.SwapControllers:
                        grab2 = go.AddComponent<VRTK_SwapControllerGrabAction>();
                        break;
                    case SecondaryGrab.ControlDirection:
                        grab2 = go.AddComponent<VRTK_ControlDirectionGrabAction>();
                        break;
                    case SecondaryGrab.AxisScale:
                        grab2 = go.AddComponent<VRTK_AxisScaleGrabAction>();
                        break;
                    default:
                        grab2 = go.AddComponent<VRTK_SwapControllerGrabAction>();
                        break;
                }
                intObj.secondaryGrabActionScript = grab2;
                if(addrb)
                {
                    Rigidbody rb = go.GetComponent<Rigidbody>();
                    if(rb == null)
                    {
                        go.AddComponent<Rigidbody>();
                    }
                }
                if(addHaptics)
                {
                    VRTK_InteractHaptics haptics = go.GetComponent<VRTK_InteractHaptics>();
                    if(haptics == null)
                    {
                        go.AddComponent<VRTK_InteractHaptics>();
                    }
                }
            }
        }
    }
}
