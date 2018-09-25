using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;
using GameFramework.Resource;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace muzi
{
    public class LaunchProcedure : ProcedureBase
    {
        bool m_InitResourcesComplete = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            EntryInstance.Resource.InitResources(OnInitResourcesComplete);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_InitResourcesComplete)
            {
                return;
            }
            EntryInstance.UI.OpenUIForm(UIFormId.UIBottomBarForm, UIGroup.BottomBar, true, UIFormId.UIBottomBarForm);

            ChangeState<MainUIProcedure>(procedureOwner);
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
    }
}