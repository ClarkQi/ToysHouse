using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;
using GameFramework.Resource;
using GameFramework;

namespace muzi
{
    public class LaunchProcedure : ProcedureBase
    {
        //bool m_InitResourcesComplete = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            EntryInstance.UI.OpenUIForm(AssetUtlity.GetUIFormPath(UIForm.UIShopForm), UIGroup.Content.ToString());//Assets/ToysHosue/UI/UIForms/UIShopForm.prefab

            ChangeState<AssetsRequestProcedure>(procedureOwner);

            //EntryInstance.Resource.InitResources(OnInitResourcesComplete);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

        }

        //private void OnInitResourcesComplete()
        //{
        //    m_InitResourcesComplete = true;

        //    Log.Info("Init resources complete.");

        //}
    }
}