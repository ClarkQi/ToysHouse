using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace muzi
{

    public class UIScanForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            formId = UIFormId.UIScanForm;
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Debug.Log("UIScanForm Open");
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            Debug.Log("UIScanForm Close");
        }
    }
}