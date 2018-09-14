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
    public class MainUIProcedure : ProcedureBase
    {
        //private UIFormId _uiForm = UIFormId.UIBottomBarForm;
        private UGuiForm _currentForm=null;
        public UGuiForm CurrentForm { get { return _currentForm; } }

        private UGuiForm _dialogForm = null;

        public bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            EntryInstance.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSucceed);
            EntryInstance.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);

            EntryInstance.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnUIFormOpenSuccess);
            //EntryInstance.UI.OpenUIForm(UIFormId.UIScanForm, UIGroup.Content, true,UIFormId.UIScanForm);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //switch (_uiForm)
            //{
            //    case UIFormId.UIBottomBarForm:
            //        break;
            //    case UIFormId.UICollectForm:
            //        break;
            //    case UIFormId.UIScanForm:
            //        break;
            //    case UIFormId.UIShopForm:
            //        break;
            //    default:
            //        break;
            //}
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            EntryInstance.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSucceed);
            EntryInstance.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFail);

            EntryInstance.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnUIFormOpenSuccess);
            if (_currentForm != null)
            {
                _currentForm.Close();
            }
        }

        private void OnUIFormOpenSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs args = (OpenUIFormSuccessEventArgs)e;
            if (args.UserData == null)
                return;
            
            string typeName= args.UserData.GetType().Name;
            if (typeName.Equals("UIFormId"))
            {
                switch ((UIFormId)args.UserData)
                {
                    case UIFormId.UIBottomBarForm:
                        break;
                    case UIFormId.UICollectForm:
                        RefashCurrentForm();
                        break;
                    case UIFormId.UIScanForm:
                        RefashCurrentForm();
                        break;
                    case UIFormId.UIShopForm:
                        RefashCurrentForm();
                        break;
                    case UIFormId.DialogForm:
                        break;
                    default:
                        break;
                }
            }
            else if (typeName.Equals("DialogParams"))
            {
                if (_dialogForm!=null&&_dialogForm.IsAvailable)
                    _dialogForm.Close();
                _dialogForm = (UGuiForm)args.UIForm.Logic;
            }

            //_uiForm = (UIFormId)args.UserData;
            if (!typeName.Equals("DialogParams"))
            {
                _currentForm = (UGuiForm)args.UIForm.Logic;
                Debug.Log(_currentForm.Name + "页面打开");
            }
        }

        void RefashCurrentForm()
        {
            if (_currentForm != null)
            {
                _currentForm.Close();
            }
        }

        #region ShopForm下载
        public static readonly string Domain = "http://192.168.5.100/appadmin/public";//http://192.168.5.100/appadmin/public     http://appadmin.com
        private string _assetsRequestUrl = Domain + "/api/arproduct/getArproductPage";//?page=1&listRows=12
        public int Page { get; private set; }

        public void RequestAsset()
        {
            EntryInstance.WebRequest.AddWebRequest(_assetsRequestUrl + "?page=" + Page, this);
        }

        private void OnWebRequestSucceed(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs arg = (WebRequestSuccessEventArgs)e;
            if (arg.UserData != this)
            {
                return;
            }

            string data = System.Text.Encoding.UTF8.GetString(arg.GetWebResponseBytes());
            if (data != null)
            {
                Debug.Log(data);
                RequestProductModel requestProduct = Utility.Json.ToObject<RequestProductModel>(data);

                if (requestProduct != null)
                {
                    Debug.Log(requestProduct.msg);
                    if (requestProduct.code == 0)
                    {

                    }
                    else if (requestProduct.code == 1)
                    {
                        EntryInstance.Event.FireNow((int)EventId.ReceiveProductData, new ReceiveProductDataEventArgs() { ProductModels = requestProduct.data });
                        Page++;
                    }
                    else if (requestProduct.code == 2)
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
        #endregion
    }
}