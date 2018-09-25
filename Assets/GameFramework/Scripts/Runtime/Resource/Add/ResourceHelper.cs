using GameFramework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;
using GameFramework;

namespace muzi
{
    public class ResourceHelper : ResourceHelperBase
    {
        #region 默认方法
        /// <summary>
        /// 直接从指定文件路径读取数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallback">读取数据流回调函数。</param>
        public override void LoadBytes(string fileUri, LoadBytesCallback loadBytesCallback)
        {
            StartCoroutine(LoadBytesCo(fileUri, loadBytesCallback));
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
#if UNITY_5_5_OR_NEWER
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
            }
            else
            {
                SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            }
#else
            if (SceneManager.UnloadScene(SceneComponent.GetSceneName(sceneAssetName)))
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
#endif
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public override void Release(object objectToRelease)
        {
            AssetBundle assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                return;
            }

            /* Unity 当前 Resources.UnloadAsset 在 iOS 设备上会导致一些诡异问题，先不用这部分
            DummySceneObject dummySceneObject = objectToRelease as DummySceneObject;
            if (dummySceneObject != null)
            {
                return;
            }

            Object unityObject = objectToRelease as Object;
            if (unityObject == null)
            {
                Log.Warning("Asset is invalid.");
                return;
            }

            if (unityObject is GameObject || unityObject is MonoBehaviour)
            {
                // UnloadAsset may only be used on individual assets and can not be used on GameObject's / Components or AssetBundles.
                return;
            }

            Resources.UnloadAsset(unityObject);
            */
        }

        private void Start()
        {

        }

        private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallback loadBytesCallback)
        {
            WWW www = new WWW(fileUri);
            yield return www;

            byte[] bytes = www.bytes;
            string errorMessage = www.error;
            www.Dispose();

            if (loadBytesCallback != null)
            {
                loadBytesCallback(fileUri, bytes, errorMessage);
            }
        }

#if UNITY_5_5_OR_NEWER
        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
        }
#endif
        #endregion

        private AssetBundleCreateRequest _assetBundleCreateRequest = null;

        private EventHandler<LoadResourceUpdateEventArgs> _loadResourceUpdateEventHandler = null;

        private EventHandler<LoadResourceCompentEventArgs> _loadResourceCompentEventHandler = null;

        private EventHandler<LoadResourceErrorEventArgs> _loadResourceErrorEventHandler = null;

        public event EventHandler<LoadResourceUpdateEventArgs> LoadResourceUpdateEventHandler
        {
            add { _loadResourceUpdateEventHandler += value; }
            remove { _loadResourceUpdateEventHandler -= value; }
        }

        public event EventHandler<LoadResourceCompentEventArgs> LoadResourceCompentEventHandler
        {
            add { _loadResourceCompentEventHandler += value; }
            remove { _loadResourceCompentEventHandler -= value; }
        }

        public event EventHandler<LoadResourceErrorEventArgs> LoadResourceErrorEventHandler
        {
            add { _loadResourceErrorEventHandler += value; }
            remove { _loadResourceErrorEventHandler -= value; }
        }

        /// <summary>
        /// 异步加载资源AssetBundle.LoadFromFileAsync
        /// </summary>
        /// <param name="path">不带file://</param>
        public void AsyncLoadAsset(string path)
        {
            _assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(path);
        }

        /// <summary>
        /// WWW加载资源,监听事件
        /// </summary>
        /// <param name="path">带file://</param>
        public void LoadAssetByWWW(string path)
        {
            StartCoroutine(LoadAsset(path));
        }

        /// <summary>
        /// WWW加载,回调
        /// </summary>
        /// <param name="path"></param>
        /// <param name="LoadAssetCallBack"></param>
        public void LoadAssetByWWW(string path, GameFrameworkAction<AssetBundle, string> LoadAssetCallBack)
        {
            StartCoroutine(LoadAsset(path,LoadAssetCallBack));
        }

        private void Update()
        {
            UpdateAssetBundleCreateRequest();
        }

        IEnumerator LoadAsset(string path)
        {
            using (WWW www = new WWW(path))
            {
                yield return www;
                while (!www.isDone)
                {
                    if (_loadResourceUpdateEventHandler != null)
                        _loadResourceUpdateEventHandler(this, new LoadResourceUpdateEventArgs(LoadResourceProgress.LoadResource, www.progress));
                    yield return 0;
                }
                if (string.IsNullOrEmpty(www.error))
                {
                    if (_loadResourceCompentEventHandler != null)
                        _loadResourceCompentEventHandler(this, new LoadResourceCompentEventArgs(www.assetBundle));
                }
                else
                {
                    if (_loadResourceErrorEventHandler != null)
                        _loadResourceErrorEventHandler(this, new LoadResourceErrorEventArgs(LoadResourceStatus.NotExist, www.error));
                }
            }
        }

        IEnumerator LoadAsset(string path, GameFrameworkAction<AssetBundle, string> LoadAssetCallBack)
        {
            using (WWW www=new WWW(path))
            {
                yield return www;
                if (LoadAssetCallBack != null)
                    LoadAssetCallBack(www.assetBundle,www.error);
            }
        }

        private void UpdateAssetBundleCreateRequest()
        {
            if (_assetBundleCreateRequest != null)
            {
                if (_assetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = _assetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldFileAssetBundleCreateRequest = _assetBundleCreateRequest;
                        if (_loadResourceCompentEventHandler != null)
                            _loadResourceCompentEventHandler(this, new LoadResourceCompentEventArgs(assetBundle));
                        if (_assetBundleCreateRequest == oldFileAssetBundleCreateRequest)
                        {
                            _assetBundleCreateRequest = null;
                        }
                    }
                    else
                    {
                        if (_loadResourceErrorEventHandler != null)
                            _loadResourceErrorEventHandler(this, new LoadResourceErrorEventArgs(LoadResourceStatus.NotExist, string.Format("Can not load asset bundle from file which is not a valid asset bundle.")));
                    }
                }
                else
                {
                    if (_loadResourceUpdateEventHandler != null)
                        _loadResourceUpdateEventHandler(this, new LoadResourceUpdateEventArgs(LoadResourceProgress.LoadResource, _assetBundleCreateRequest.progress));
                }
            }
        }
    }
}