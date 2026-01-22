using RDLevelEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    using LevelStorage = Functionality.Level.Storage;

    internal class Controller : MonoBehaviour
    {
        private static Controller instance;
        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject holder = new($"Mod_{MyPluginInfo.PLUGIN_GUID}_WardController");
                    instance = holder.AddComponent<Controller>();
                }

                return instance;
            }
        }

        public void Clear()
        {
            foreach (IObject obj in objects)
            {
                obj.Destroy();
            }

            selectedIndex = 0;
            levels.Clear();
            objects.Clear();
            selectables.Clear();
        }

        public bool TrySetupLevels()
        {
            foreach (LevelStorage.Data data in LevelStorage.Instance.LevelData)
            {
                Level level = new(data);

                levels.Add(level);
                objects.Add(level);
                selectables.Add(level);
            }

            if (levels.Count == 0)
            {
                Plugin.LogError("No levels found. Cannot proceed with loading.");
                enabled = false;
                return false;
            }

            Select(0);
            return true;
        }

        public void SetupSelectors()
        {
            levelDescription.Setup();
        }

        public void Update()
        {
            if (!InteractionEnabled())
            {
                return;
            }

            if (RDInput.anyPlayerPress)
            {
                Interact();
            }
            else
            {
                if (RDInput.leftPress)
                {
                    SelectNext(Direction.Left);
                }
                else if (RDInput.rightPress)
                {
                    SelectNext(Direction.Right);
                }
            }
        }

        public ISelectable Selected => selectables[selectedIndex];

        public bool areThereAnyModComments = false;

        private bool InteractionEnabled()
        {
            if (!areThereAnyModComments
                || levelLoading
                || scnGame.instance.paused)
            {
                return false;
            }

            if (scnEditor.instance != null
                && scnEditor.instance.selectedControls.Count > 0)
            {
                return false;
            }

            return true;
        }

        private void SelectNext(Direction direction)
        {
            int sign = (int) direction;

            int bestIndex = -1;
            int worstIndex = -1;
            float bestDistance = float.MaxValue;
            float worstDistance = float.MinValue;
            bool shouldWrapAround = true;

            for (int i = 0; i < selectables.Count; i++)
            {
                ISelectable selectable = selectables[i];

                if (i != selectedIndex)
                {
                    float delta = selectable.Position.x - Selected.Position.x;

                    if (delta * sign > 0)
                    {
                        shouldWrapAround = false;

                        if (Mathf.Abs(delta) < bestDistance)
                        {
                            bestDistance = Mathf.Abs(delta);
                            bestIndex = i;
                        }
                    }
                    else if (shouldWrapAround)
                    {
                        if (Mathf.Abs(delta) > worstDistance)
                        {
                            worstDistance = Mathf.Abs(delta);
                            worstIndex = i;
                        }
                    }
                }
            }

            if (bestIndex != -1
                || worstIndex != -1)
            {
                Select(shouldWrapAround ? worstIndex : bestIndex);
            }
        }

        private void Select(int index)
        {
            selectedIndex = index;
            levelDescription.Update(Selected);
        }

        private void Interact()
        {
            if (Selected is not Level level)
            {
                return;
            }

            TryStartLevel(level);
        }

        private void TryStartLevel(Level level)
        {
            levelLoading = true;

            if (DesktopLevelLoader.cacheLevelsDict == null)
            {
                DesktopLevelLoader.InitializeCacheLevelsDict();
            }

            foreach (KeyValuePair<string, RDLevelSettings> keyValuePair in DesktopLevelLoader.cacheLevelsDict)
            {
                CustomLevelData data = new()
                {
                    settings = keyValuePair.Value
                };

                if (data.Hash == level.Data.hash)
                {
                    string relative = keyValuePair.Key.Substring("Local/".Length);
                    string directory = Path.Combine(LevelValidation.CustomLevelsPath, relative);

                    if (Directory.Exists(directory))
                    {
                        string file = Path.Combine(directory, data.settings.mainRDLevelRelativePath);

                        if (!string.IsNullOrEmpty(file) && RDFile.Exists(file))
                        {
                            StartCoroutine(DoLevelLoadTransitionAndGotoLevel(file));
                            return;
                        }
                    }
                }
            }

            Plugin.LogError($"Could not find level with hash {level.Data.hash}");
            levelLoading = false;
        }

        private IEnumerator DoLevelLoadTransitionAndGotoLevel(string file)
        {
            scrVfxControl.instance.Flash(-1, 1f, 1f);

            yield return new WaitForSecondsRealtime(1f);

            scrQuad scrQuad = scrVfxControl.instance.GetRoomOverlay(-1);
            scrQuad.TweenColorFromTo(Color.black.WithAlpha(0f), Color.black, 1f);

            yield return new WaitForSecondsRealtime(1f);

            scrVfxControl.instance.FlashTextUIInstant("LOADING...", flash: false);

            yield return null;

            if (!RDFile.Exists(file))
            {
                Plugin.LogError($"level at {file} was removed while loading lol");
                yield break;
            }

            scnBase.GoToLevelWithExternalPath(file);
            yield break;
        }

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        private readonly Description levelDescription = new();

        private readonly List<Level> levels = new();
        private readonly List<ISelectable> selectables = new();
        private readonly List<IObject> objects = new();

        private int selectedIndex = 0;

        private bool currentlyDayShift = true;

        private bool levelLoading = false;
    }
}
