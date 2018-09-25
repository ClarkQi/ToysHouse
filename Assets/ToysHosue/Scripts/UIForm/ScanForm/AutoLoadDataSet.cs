using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using System.IO;
using System.Net;
using UnityGameFramework.Runtime;
using GameFramework;
using System;
using GameFramework.Event;

namespace muzi
{
    public class AutoLoadDataSet : MonoBehaviour
    {

        ObjectTracker tracker;
        DataSet TotalDatas;
        static bool isLoaded = false;
        bool isUnzip = false;
        string dataPath;
        string zipPath;
        string versionPath;
        RequestTotalDatasetModel requestDataset;
        string versionUrl = MainUIProcedure.Domain + "/api/arproduct/checkDatasetVersion";
        void Start()
        {
            dataPath = Application.persistentDataPath + "/Vuforia/TotalDatas.xml";
            zipPath = Application.persistentDataPath + "/Vuforia.zip";
            versionPath = Application.persistentDataPath + "/Vuforia/DataVersion.txt";
            GetFile();
        }

        private void OnEnable()
        {
            EntryInstance.Event.Subscribe(DownloadSuccessEventArgs.EventId, OnDownLoadDatasetSuccess);
            EntryInstance.Event.Subscribe(DownloadFailureEventArgs.EventId, OnDownLoadDatasetFail);
        }

        private void OnDisable()
        {
            EntryInstance.Event.Unsubscribe(DownloadSuccessEventArgs.EventId, OnDownLoadDatasetSuccess);
            EntryInstance.Event.Unsubscribe(DownloadFailureEventArgs.EventId, OnDownLoadDatasetFail);
        }

        private void OnDownLoadDatasetSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs args = (DownloadSuccessEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            Debug.Log("OnDownLoadDatasetSuccess");

            StartCoroutine(StartUnzip());
        }

        IEnumerator StartUnzip()
        {
            yield return 0;
            isUnzip = true;
            ZipUtility.AsyncUnzipFile(zipPath, Application.persistentDataPath, null, new UnzipCallback(zipPath, OnUnzipFileSucceed));
            yield return new WaitWhile(()=>isUnzip);
            StartCoroutine(LoadUpdate());
            EntryInstance.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = "提示",
                Message = "识别图更新成功！",
                ConfirmText = "确定"
            });
        }

        private void OnDownLoadDatasetFail(object sender, GameEventArgs e)
        {
            DownloadFailureEventArgs args = (DownloadFailureEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            EntryInstance.UI.OpenDialog(new DialogParams() {
                Mode=1,
                Title = "提示",
                Message="更新识别图失败,扫描识别图不可用！",
                ConfirmText="确定"
            });
        }

        void OnUnzipFileSucceed(bool isSucceed)
        {
            Debug.Log("OnUnzipFileSucceed");
            isUnzip = true;
        }

        void GetFile()
        {
            if (File.Exists(dataPath))
            {
                if (UpdateVersion())
                {
                    EntryInstance.UI.OpenDialog(new DialogParams()
                    {
                        Mode = 0,
                        Title = "提示",
                        Message = "识别图更新中，请稍候！",
                    });
                    EntryInstance.Download.AddDownload(zipPath, MainUIProcedure.Domain + requestDataset.data.datasetfile, this);
                }
                else
                    StartCoroutine(LoadUpdate());
            }
            else
            {
                //当网络不可用时              
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    EntryInstance.UI.OpenDialog(new DialogParams()
                    {
                        Mode = 1,
                        Title = "提示",
                        Message = "需要更新资源，请连接网络后点击确定！",
                        ConfirmText = "确定",
                        OnClickConfirm = (data) => { GetFile(); }
                    });
                    return;
                }
                RequestVersion();
                if (requestDataset == null)
                {
                    EntryInstance.UI.OpenDialog(new DialogParams()
                    {
                        Mode = 1,
                        Title = "提示",
                        Message = "需要更新资源，请连接网络后重试！",
                        ConfirmText = "确定",
                        OnClickConfirm = (data) => { GetFile(); }
                    });
                }
                else
                {
                    EntryInstance.UI.OpenDialog(new DialogParams()
                    {
                        Mode = 0,
                        Title = "提示",
                        Message = "识别图更新中，请稍候！",
                    });
                    EntryInstance.Download.AddDownload(zipPath, MainUIProcedure.Domain + requestDataset.data.datasetfile, this);
                }
            }
        }

        bool UpdateVersion()
        {
            try
            {
                RequestVersion();
                if (File.Exists(versionPath)&&requestDataset!=null)
                {
                    StreamReader sr = new StreamReader(versionPath);
                    string version = sr.ReadLine();
                    if (Convert.ToSingle(requestDataset.data.version) > Convert.ToSingle(version))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        void RequestVersion()
        {
            WebClient wc = new WebClient();
            string json = wc.DownloadString(versionUrl);
            if (json != null)
                requestDataset = Utility.Json.ToObject<RequestTotalDatasetModel>(json);
        }

        IEnumerator LoadUpdate()
        {
            while (!isLoaded)
            {
                yield return new WaitForSeconds(.1f);
                if (VuforiaRuntimeUtilities.IsVuforiaEnabled() && !isLoaded)
                {
                    if (tracker == null)
                    {
                        tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
                    }
                    if (TotalDatas == null && tracker != null)
                    {
                        TotalDatas = tracker.CreateDataSet();
                    }

                    if (File.Exists(dataPath) && TotalDatas.Load(dataPath, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
                    {
                        isLoaded = true;
                        tracker.ActivateDataSet(TotalDatas);
                        ReadDataSet();
                        Debug.Log("Load DataSet Succeed!");
                    }
                    else
                    {
                        Debug.Log("Load DataSet fail!");
                    }
                }
            }
        }

        void ReadDataSet()
        {
            foreach (ImageTargetBehaviour target in FindObjectsOfType(typeof(ImageTargetBehaviour)))
            {
                target.transform.parent = transform;
                target.name = "ImageTarget";
                target.gameObject.AddComponent<TurnOffBehaviour>();
                target.gameObject.AddComponent<ScanTrackableEventHandler>();
            }
        }

        //private void OnDestroy()
        //{
        //    TotalDatas.DestroyAllTrackables(false);
        //}
    }
}