using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace muzi
{

    public class UICollectForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            formId = UIFormId.UICollectForm;
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Debug.Log("UICollectForm Open");
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            Debug.Log("UICollectForm Close");
        }
    }
}