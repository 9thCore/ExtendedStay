using RDLevelEditor;

namespace ExtendedStay.Functionality.Ward
{
    public static class PersistentData
    {
        public static void Clear()
        {
            wardLevelPath = null;
            wardWasInEditor = false;
        }

        public static void Collect(string hash)
        {
            if (scnEditor.instance != null)
            {
                wardWasInEditor = true;
                wardLevelPath = scnEditor.instance.openedFilePath;
            }
            else
            {
                wardWasInEditor = false;
                wardLevelPath = scnGame.currentLevelPath;
                selectedHash = hash;
            }
        }

        public static bool TryReturnToWard()
        {
            if (string.IsNullOrEmpty(wardLevelPath)
                || !RDFile.Exists(wardLevelPath))
            {
                wardLevelPath = null;
                selectedHash = null;
                return false;
            }

            if (wardWasInEditor)
            {
                scrVfxControl.FlushCustomLevelData();
                scnEditor.customLevelPath = wardLevelPath;
                scnBase.GoToLevelEditor();
            }
            else
            {
                scrVfxControl.FlushCustomLevelData();
                scnBase.GoToLevelWithExternalPath(wardLevelPath);
            }

            Clear();
            return true;
        }

        public static string SelectedHash => selectedHash;

        private static string selectedHash = null;
        private static string wardLevelPath = null;
        private static bool wardWasInEditor = false;
    }
}
