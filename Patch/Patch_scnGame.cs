using ExtendedStay.Functionality.Settings;
using ExtendedStay.Functionality.Ward;
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
                if (!Controller.Instance.areThereAnyModComments)
                {
                    return;
                }

                Storage.Instance.OnLevelLoad(__instance.currentLevel);

                Controller.Instance.Clear();
                if (Controller.Instance.TrySetupLevels())
                {

                }

                if (scnEditor.instance != null)
                {
                    scnEditor.instance.ipm.UpdateBlankPanel(onlyIfVisible: false);
                }
            }
        }
    }
}
