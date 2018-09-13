using GameFramework;
using UnityGameFramework.Runtime;

namespace muzi
{
    public static class ConfigExtension
    {

        public static void LoadConfig(this ConfigComponent configComponent, string configName, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName+".xml", AssetUtility.GetConfigPath(configName), AssetPriority.ConfigAsset, userData);
        }
    }
}