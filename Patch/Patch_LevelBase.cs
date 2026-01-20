using ExtendedStay.Functionality;
using ExtendedStay.Functionality.Settings;
using HarmonyLib;
using RDLevelEditor;

namespace ExtendedStay.Patch
{
    [HarmonyPatch]
    internal static class Patch_LevelBase
    {
        [HarmonyPatch(typeof(LevelBase), nameof(LevelBase.LoadCustomAssets))]
        private static class LoadCustomAssets
        {
            private static void Prefix(LevelBase __instance)
            {
                Storage.Instance.Clear();

                foreach (LevelEvent_Base levelEvent in __instance.levelEvents)
                {
                    if (levelEvent is LevelEvent_Comment comment)
                    {
                        ParseManager.Instance.Parse(comment.text);
                    }
                }
            }
        }
    }
}
