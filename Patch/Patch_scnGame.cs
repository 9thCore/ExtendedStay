using ExtendedStay.Functionality.Settings;
using HarmonyLib;
using RDLevelEditor;

namespace ExtendedStay.Patch
{
    [HarmonyPatch]
    internal static class Patch_scnGame
    {
        [HarmonyPatch(typeof(scnGame), nameof(scnGame.StartTheGame))]
        private static class StartTheGame
        {
            private static void Prefix(scnGame __instance)
            {
                Storage.Instance.OnLevelLoad(__instance.currentLevel);

                if (scnEditor.instance != null)
                {
                    scnEditor.instance.ipm.UpdateBlankPanel(onlyIfVisible: false);
                }
            }
        }
    }
}
