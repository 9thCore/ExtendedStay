using ExtendedStay.Functionality;
using ExtendedStay.Functionality.Ward;
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
                Controller.Instance.areThereAnyModComments = false;

                Functionality.Settings.Storage.Instance.Clear();
                Functionality.Level.Storage.Instance.Clear();

                foreach (LevelEvent_Base levelEvent in __instance.levelEvents)
                {
                    if (levelEvent.active
                        && levelEvent is LevelEvent_Comment comment)
                    {
                        if (ParseManager.Instance.TryParse(comment.text, out ParseManager.FailureReason reason))
                        {
                            Controller.Instance.areThereAnyModComments = true;
                        }
                        else if (reason != ParseManager.FailureReason.NoMatch)
                        {
                            Plugin.LogError($"Failure ({reason}) reading comment at bar {comment.bar}, beat {comment.beat}. Full comment text is:\n{comment.text}");
                            Controller.Instance.areThereAnyModComments = true;
                        }
                    }
                }
            }
        }
    }
}
