using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace muzi
{
    public static class AssetUtility
    {
        public static string GetUIFormAsset(UIFormId uiForm)
        {
            return string.Format("Assets/ToysHosue/UI/UIForms/{0}.prefab", uiForm.ToString());
        }

        public static string GetUIItemPath(string name)
        {
            return string.Format("Assets/ToysHouse/UI/UIItems/{0}.prefab", name);
        }

        public static string GetConfigPath(string name)
        {
            return string.Format("Assets/ToysHosue/Configs/{0}.xml", name);
        }

        public static UIGroup GetFormGroup(UIFormId uiForm)
        {
            switch (uiForm)
            {
                case UIFormId.UIBottomBarForm:
                    return UIGroup.BottomBar;
                case UIFormId.UICollectForm:
                case UIFormId.UIScanForm:
                case UIFormId.UIShopForm:
                    return UIGroup.Content;
                case UIFormId.DialogForm:
                    return UIGroup.PopDialog;
                default:
                    return UIGroup.TopBar;
            }
        }
    }
}