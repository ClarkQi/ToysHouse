using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;
using GameFramework;

namespace muzi
{
    public class PreloadProcedure : ProcedureBase
    {
        private bool _configLoadSucceed = false;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            EntryInstance.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            EntryInstance.Event.Subscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFail);
            LoadConfig();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_configLoadSucceed)
            {
                ChangeState<AssetsRequestProcedure>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            EntryInstance.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, OnLoadConfigSuccess);
            EntryInstance.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, OnLoadConfigFail);
        }

        private void LoadConfig()
        {
            EntryInstance.Config.LoadConfig("BuildSettings", "Assets/ToysHouse/Configs/BuildSettings.txt", this);
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs args = (LoadConfigSuccessEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            //if (args.ConfigName.Equals("DefaultConfig"))
                Debug.Log("DefaultConfig Load Success");
            _configLoadSucceed = true;
            //Debug.Log("domain=" + EntryInstance.Config.GetString("domain"));
        }

        private void OnLoadConfigFail(object sender, GameEventArgs e)
        {
            LoadConfigFailureEventArgs args = (LoadConfigFailureEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            //if (args.ConfigName.Equals("DefaultConfig"))
                Debug.Log("DefaultConfig Load Fail");
            Log.Error("Can not load config '{0}' from '{1}' with error message '{2}'.", args.ConfigName, args.ConfigAssetName, args.ErrorMessage);
        }
    }
}