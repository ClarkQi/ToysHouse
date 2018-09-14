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

            EntryInstance.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = "Hello",
                Message = "恭喜打开消息弹框",
                OnClickConfirm = delegate(object data) { Debug.Log("Hello World"); },
                OnClickCancel=delegate(object data) { Debug.Log("OnClickCancel"); },
            });
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            Debug.Log("UICollectForm Close");
        }
    }
}