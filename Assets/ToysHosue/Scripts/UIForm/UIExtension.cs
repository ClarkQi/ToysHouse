using GameFramework;
using GameFramework.DataTable;
using GameFramework.Procedure;
using GameFramework.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace muzi
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            string assetName = AssetUtility.GetUIFormAsset(uiFormId);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(assetName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            string assetName = AssetUtility.GetUIFormAsset(uiFormId);
            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(assetName);
                if (uiForm == null)
                {
                    return null;
                }

                return (UGuiForm)uiForm.Logic;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm)uiGroup.GetUIForm(assetName);
            if (uiForm == null)
            {
                return null;
            }

            return (UGuiForm)uiForm.Logic;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId,UIGroup uiGroup,bool pause=true, object userData = null)
        {
            string assetName = AssetUtility.GetUIFormAsset(uiFormId);

            if (uiComponent.IsLoadingUIForm(assetName))
            {
                return null;
            }

            return uiComponent.OpenUIForm(assetName, uiGroup.ToString(), pause, userData);
        }

        public static void OpenDialog(this UIComponent uiComponent, DialogParams dialogParams)
        {
            if (((MainUIProcedure)EntryInstance.Procedure.GetProcedure<MainUIProcedure>()).UseNativeDialog)
            {
                OpenNativeDialog(dialogParams);
            }
            else
            {
                uiComponent.OpenUIForm(UIFormId.DialogForm,UIGroup.PopDialog,false, dialogParams);
            }
        }

        private static void OpenNativeDialog(DialogParams dialogParams)
        {
            throw new System.NotImplementedException("OpenNativeDialog");
        }
    }
}
