using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Resource;
using System;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.SceneManagement;
using GameFramework.Download;

namespace muzi
{
    public class TestLoadScene : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {

        }

        private void OnGUI()
        {
            GUILayout.Space(300);
            if (GUILayout.Button("打开场景", GUILayout.Width(300), GUILayout.Width(80)))
            {
                //StartCoroutine(LoadSceneAsset());
                EntryInstance.Resource.ResourceHelper.LoadAssetByWWW("file://" + Application.persistentDataPath + "/product1/scene/modelshow.dat", OnLoadAssetCompete);
            }
        }

        private void OnLoadAssetCompete(AssetBundle assetBundle, string errorMessage)
        {
            if (assetBundle!=null)
            {
                //EntryInstance.Scene.UnloadScene(AssetUtility.GetSceneAsset("Scan"));
                SceneManager.LoadScene("ModelShow", LoadSceneMode.Additive);
                EntryInstance.UI.CloseAllLoadedUIForms();
                EntryInstance.UI.OpenUIForm(AssetUtility.GetUIFormAsset(UIFormId.UIARForm), UIGroup.Content.ToString(), true, UIFormId.UIARForm);
                assetBundle.Unload(false);
            }
            if(errorMessage!=null)
            {
                Debug.Log(errorMessage);
            }
        }

        IEnumerator LoadSceneAsset()
        {
            using (WWW www=new WWW("file://"+Application.persistentDataPath+ "/product1/scene/modelshow.dat"))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    string[] scenes = www.assetBundle.GetAllScenePaths();
                    AssetBundle ab = www.assetBundle;

                    foreach (var scene in EntryInstance.Scene.GetLoadedSceneAssetNames())
                    {
                        EntryInstance.Scene.UnloadScene(scene);
                    }
                    Debug.Log(scenes[0]);
                    //SceneManager.LoadScene(scenes[0], LoadSceneMode.Additive);

                    FindObjectOfType<DefaultLoadResourceAgentHelper>().LoadAsset(www.assetBundle, "ModelShow.unity", null,true);
                    EntryInstance.UI.CloseAllLoadedUIForms();
                    EntryInstance.UI.OpenUIForm(AssetUtility.GetUIFormAsset(UIFormId.UIARForm), UIGroup.Content.ToString(), true, UIFormId.UIARForm);
                }
                www.assetBundle.Unload(false);
            }
        }

        private void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            if (userData!=this)
            {
                return;
            }
            Debug.Log("OnLoadAssetFail  " + assetName+"  "+errorMessage);
        }

        private void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            if (userData != this)
            {
                return;
            }
            foreach (var scene in EntryInstance.Scene.GetLoadedSceneAssetNames())
            {
                Debug.Log(scene.ToString());
                EntryInstance.Scene.UnloadScene(scene);
            }
            Debug.Log("OnLoadAssetSuccess  "+ assetName);
            EntryInstance.UI.OpenUIForm(AssetUtility.GetUIFormAsset(UIFormId.UIARForm), UIGroup.Content.ToString(), true, UIFormId.UIARForm);
            EntryInstance.Scene.LoadScene(assetName);
        }
    }
}