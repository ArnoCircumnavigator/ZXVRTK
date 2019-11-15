using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;
using UnityEngine;
namespace Currency{
#if UNITY_EDITOR
    public class ZXTK_Implement
    {
        /// <summary>
        /// 创建常用文件夹
        /// </summary>
        public static void GenerateFolder()
        {
            // 文件路径
            string prjPath = Application.dataPath + "/";
            Directory.CreateDirectory(prjPath + "Audio");
            Directory.CreateDirectory(prjPath + "Prefabs");
            Directory.CreateDirectory(prjPath + "Materials");
            Directory.CreateDirectory(prjPath + "Resources");
            Directory.CreateDirectory(prjPath + "Scripts");
            Directory.CreateDirectory(prjPath + "Textures");
            Directory.CreateDirectory(prjPath + "Scenes");
            AssetDatabase.Refresh();
            Debug.Log("文件夹创建成功！");
        }
        public static bool CreatSceneOfVR(string SceneName)
        {
            //是否存在某文件
            if( File.Exists(Application.dataPath + "/Scenes/" + SceneName + ".unity"))
            {
                Debug.LogError("路径" + Application.dataPath + "/Scenes" + "下，已存在" + SceneName + "场景");
                return false;
            }
            Scene scene = new Scene();
            if (SceneName == SceneManager.GetActiveScene().name) scene = SceneManager.GetActiveScene();
            else scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreatePrefabOfVR();
            try
            {
                if (Application.dataPath + "/Scenes" == null)
                {
                    if (EditorSceneManager.SaveScene(scene, Application.dataPath))
                    {
                        AssetDatabase.Refresh();
                        return true;
                    }
                    else return false;
                }//没有Scenes文件夹
                else if (EditorSceneManager.SaveScene(scene, Application.dataPath + "/Scenes/" + SceneName + ".unity"))
                {
                    AssetDatabase.Refresh();
                    return true;
                }
                return false;
            }
            catch (System.Exception e)
            {
                throw;
            }
            finally
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            }
        }
        public static void CreatePrefabOfVR()
        {
            try
            {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ZXToolkit/Prefab/[CLTOOLKIT].prefab");
                Object.Instantiate(go, Vector3.zero, Quaternion.identity).name = "[CLTOOLKIT]";
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
#endif
}
