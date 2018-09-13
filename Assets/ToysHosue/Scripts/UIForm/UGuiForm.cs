using GameFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace muzi
{
    public abstract class UGuiForm : UIFormLogic
    {
        public UIFormId formId;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            RectTransform transform = GetComponent<RectTransform>();
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.one;
            //transform.anchoredPosition = Vector2.zero;
            //transform.sizeDelta = Vector2.zero;
        }

        public void Close()
        {
            if (UIForm.UIFormAssetName == AssetUtility.GetUIFormAsset(UIFormId.UIBottomBarForm))
                return;
            EntryInstance.UI.CloseUIForm(this);
        }
    }
}
