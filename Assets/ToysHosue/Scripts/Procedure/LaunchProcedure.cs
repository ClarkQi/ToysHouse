using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;
using GameFramework.Resource;
using GameFramework;
using UnityGameFramework.Runtime;

namespace muzi
{
    public class LaunchProcedure : ProcedureBase
    {
        bool m_InitResourcesComplete = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            //EntryInstance.UI.OpenUIForm(AssetUtlity.GetUIFormPath(UIForm.UIShopForm), UIGroup.Content.ToString());//Assets/ToysHosue/UI/UIForms/UIShopForm.prefab

            EntryInstance.Resource.InitResources(OnInitResourcesComplete);
            //EntryInstance.Resource.LoadAsset("Assets/ToysHosue/UI/UI/UIForms/UIShopForm.prefab", new LoadAssetCallbacks(LoadAssetSucceed, LoadAssetFailed));
            //EntryInstance.Resource.LoadAsset("Assets/ToysHosue/UI/UIItems/HeadImage.prefab", new LoadAssetCallbacks(LoadAssetSucceed, LoadAssetFailed));
            //EntryInstance.Resource.LoadAsset("Assets/ToysHosue/UI/UIItems/HeadImage.prefab", new LoadAssetCallbacks(LoadAssetSucceed, LoadAssetFailed));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_InitResourcesComplete)
            {
                return;
            }
            EntryInstance.UI.OpenUIForm(AssetUtility.GetUIFormPath(UIForm.UIShopForm), UIGroup.Content.ToString());
            ChangeState<AssetsRequestProcedure>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

        }

        private void OnInitResourcesComplete()
        {
            m_InitResourcesComplete = true;

            Log.Info("Init resources complete.");
            
        }

        //private void LoadAssetSucceed(string assetName, object asset, float duration, object userData)
        //{
        //}

        //private void LoadAssetFailed(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        //{

        //}
    }
}