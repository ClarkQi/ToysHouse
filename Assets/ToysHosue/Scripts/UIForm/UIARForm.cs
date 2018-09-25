using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using Vuforia;
using UnityEngine.SceneManagement;

namespace muzi
{
    public class UIARForm : UGuiForm
    {
        public Button BtnBack;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            formId = UIFormId.UIARForm;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            if (tracker != null && !tracker.IsActive)
                tracker.Start();
            BtnBack.onClick.AddListener(OnBackClick);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            if (tracker != null && tracker.IsActive)
                tracker.Stop();
            BtnBack.onClick.RemoveListener(OnBackClick);
        }

        private void OnBackClick()
        {

            SceneManager.UnloadSceneAsync("ModelShow");
            //EntryInstance.Scene.LoadScene("Assets/ToysHouse/Scenes/Scan.unity");
            EntryInstance.Procedure.GetProcedure<MainUIProcedure>().OpenBottomBarForm();
        }
    }
}