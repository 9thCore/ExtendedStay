using System.Collections.Generic;
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
                successfulLoad = false;
                return false;
            }

            Select(0);
            successfulLoad = true;
            return true;
        }

        public void Update()
        {
            if (!successfulLoad)
            {
                return;
            }

            if (RDInput.anyPlayerPress)
            {

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
        }

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        private readonly List<Level> levels = new();
        private readonly List<ISelectable> selectables = new();
        private readonly List<IObject> objects = new();
        private int selectedIndex = 0;
        private bool successfulLoad = false;
    }
}
