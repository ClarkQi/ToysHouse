using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Vuforia;

namespace muzi
{

    public class UIScanForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            formId = UIFormId.UIScanForm;

            EntryInstance.Scene.LoadScene(AssetUtility.GetSceneAsset("Scan"));
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Debug.Log("UIScanForm Open");
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            if (tracker != null && !tracker.IsActive)
                tracker.Start();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            Debug.Log("UIScanForm Close");
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            if (tracker != null && tracker.IsActive)
                tracker.Stop();
        }
    }
}