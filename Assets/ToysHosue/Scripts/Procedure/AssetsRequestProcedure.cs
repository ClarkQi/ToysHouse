using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace muzi
{
    public class AssetsRequestProcedure : ProcedureBase
    {
        public static readonly string Domain= "http://192.168.5.100/appadmin/public";//http://192.168.5.100/appadmin/public     http://appadmin.com
        private string _assetsRequestUrl = Domain+"/api/arproduct/getArproductPage";//?page=1&listRows=12
        public int Page { get; private set; }
        
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            Page = 1;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            EntryInstance.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSucceed);
            EntryInstance.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
            RequestAsset();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            EntryInstance.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSucceed);
            EntryInstance.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);
        }

        public void RequestAsset()
        {
            EntryInstance.WebRequest.AddWebRequest(_assetsRequestUrl+"?page="+Page, this);
        }

        private void OnWebRequestSucceed(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs arg = (WebRequestSuccessEventArgs)e;
            if (arg.UserData!=this)
            {
                return;
            }

            string data = System.Text.Encoding.UTF8.GetString(arg.GetWebResponseBytes());
            if (data != null)
            {
                Debug.Log(data);
                RequestProductModel requestProduct = Utility.Json.ToObject<RequestProductModel>(data);

                if (requestProduct!=null)
                {
                    Debug.Log(requestProduct.msg);
                    if (requestProduct.code==0)
                    {
                        
                    }
                    else if (requestProduct.code == 1)
                    {
                        EntryInstance.Event.FireNow((int)EventId.ReceiveProductData, new ReceiveProductDataEventArgs() { ProductModels = requestProduct.data });
                        Page++;
                    }
                    else if (requestProduct.code==2)
                    {
                        
                    }
                }
            }
            
        }

        private void OnWebRequestFail(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs arg = (WebRequestFailureEventArgs)e;
            if (arg.UserData != this)
            {
                return;
            }
            Debug.Log(arg.ErrorMessage);
        }
    }
}