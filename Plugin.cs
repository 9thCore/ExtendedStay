using BepInEx;
using BepInEx.Logging;
using ExtendedStay.Patch;
using HarmonyLib;
using System.Reflection;

namespace ExtendedStay
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        internal static Plugin Instance;

        public static void LogWarn(object message) => Logger.LogWarning(message);
        public static void LogError(object message) => Logger.LogError(message);
        public static void LogInfo(object message) => Logger.LogInfo(message);

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Instance = this;

            Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} successfully loaded.");
        }
    }
}
