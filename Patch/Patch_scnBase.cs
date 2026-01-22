using ExtendedStay.Functionality.Ward;
using HarmonyLib;

namespace ExtendedStay.Patch
{
    [HarmonyPatch]
    internal static class Patch_scnBase
    {
        [HarmonyPatch(typeof(scnBase), nameof(scnBase.GoToLevelSelect))]
        private static class GoToLevelSelect
        {
            private static bool Prefix()
            {
                return !PersistentData.TryReturnToWard();
            }
        }
    }
}
