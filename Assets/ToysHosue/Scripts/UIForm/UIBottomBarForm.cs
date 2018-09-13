using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace muzi
{
    public class UIBottomBarForm : UGuiForm
    {
        [SerializeField]
        private Toggle TogCollect;
        [SerializeField]
        private Toggle TogScan;
        [SerializeField]
        private Toggle TogShop;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            formId = UIFormId.UIBottomBarForm;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            TogCollect.onValueChanged.AddListener(OnCollectClick);
            TogScan.onValueChanged.AddListener(OnScanClick);
            TogShop.onValueChanged.AddListener(OnShopClick);

            EntryInstance.UI.OpenUIForm(UIFormId.UIScanForm, UIGroup.Content, true, UIFormId.UIScanForm);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            TogCollect.onValueChanged.RemoveListener(OnCollectClick);
            TogScan.onValueChanged.RemoveListener(OnScanClick);
            TogShop.onValueChanged.RemoveListener(OnShopClick);
        }

        private void OnCollectClick(bool isOn)
        {
            if (isOn)
            {
                if (((MainUIProcedure)EntryInstance.Procedure.GetProcedure<MainUIProcedure>()).CurrentForm.formId != UIFormId.UICollectForm)
                    EntryInstance.UI.OpenUIForm(UIFormId.UICollectForm, UIGroup.Content, true, UIFormId.UICollectForm);
            }
        }

        private void OnScanClick(bool isOn)
        {
            if (isOn)
            {

                if (((MainUIProcedure)EntryInstance.Procedure.GetProcedure<MainUIProcedure>()).CurrentForm.formId != UIFormId.UIScanForm)
                    EntryInstance.UI.OpenUIForm(UIFormId.UIScanForm, UIGroup.Content, true, UIFormId.UIScanForm);
            }
        }

        private void OnShopClick(bool isOn)
        {
            if (isOn)
            {
                if (((MainUIProcedure)EntryInstance.Procedure.GetProcedure<MainUIProcedure>()).CurrentForm.formId != UIFormId.UIShopForm)
                    EntryInstance.UI.OpenUIForm(UIFormId.UIShopForm, UIGroup.Content, true, UIFormId.UIShopForm);
            }
        }
    }
}