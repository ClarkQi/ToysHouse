using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace muzi
{
    public static class AssetUtility
    {
        public static string GetUIFormPath(UIForm uiForm)
        {
            return string.Format("Assets/ToysHosue/UI/UIForms/{0}.prefab", uiForm.ToString());
        }

        public static string GetUIItemPath(string name)
        {
            return string.Format("Assets/ToysHouse/UI/UIItems/{0}.prefab", name);
        }
    }
}