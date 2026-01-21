using ExtendedStay.Util;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedStay.Functionality.Ward
{
    using LevelStorage = Functionality.Level.Storage;

    public class Description
    {
        public void Setup()
        {
            if (levelDescription != null)
            {
                return;
            }

            GameObject levelDescriptionHolder = new($"Mod_{MyPluginInfo.PLUGIN_GUID}_LevelDescription");
            levelDescriptionHolder.SetActive(false);

            levelDescription = levelDescriptionHolder.AddComponent<Text>();
            levelDescription.supportRichText = true;
            levelDescription.fontSize = 8;
            levelDescription.alignment = TextAnchor.UpperCenter;

            levelDescriptionHolder.AddComponent<RDStringToUIText>();

            levelDescriptionHolder.AddComponent<EightSidedOutline>().effectColor = new Color(0.12f, 0.12f, 0.12f, 1f);

            Transform transform = levelDescriptionHolder.transform;
            transform.SetParent(scnGame.instance.canvas.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            levelDescriptionHolder.SetActive(true);
            levelDescriptionTransform = transform as RectTransform;
        }

        public void Update(ISelectable selectable)
        {
            if (selectable is Level level)
            {
                UpdateText(level);
                levelDescription.gameObject.SetActive(true);
                levelDescriptionTransform.anchoredPosition = level.DescriptionPosition;
            }
            else
            {
                levelDescription.gameObject.SetActive(false);
            }
        }

        private void UpdateText(Level level)
        {
            StringBuilder description = new();
            StringBuilder narration = new();

            switch (level.Data.levelType)
            {
                case LevelStorage.LevelType.Boss:
                    description.Append($"<color={scnLevelSelect.highlightColor}>{RDString.Get("levelSelect.boss").Replace("[tier]", "").Trim()}</color>\n\n");
                    narration.Append(RDString.Get("narration.boss"));
                    break;
                case LevelStorage.LevelType.Bonus:
                    description.Append($"<color={scnLevelSelect.highlightColor}>{RDString.Get("levelSelect.bonus")}</color>\n\n");
                    narration.Append(RDString.Get("narration.bonus"));
                    break;
                case LevelStorage.LevelType.Intermission:
                    description.Append($"<color={scnLevelSelect.highlightColor}>{RDString.Get("levelSelect.intermission")}</color>\n\n");
                    narration.Append(RDString.Get("narration.intermission"));
                    break;
                case LevelStorage.LevelType.Normal:
                    narration.Append(RDString.Get("narration.level"));
                    break;
            }

            description.Append($"<color={scnLevelSelect.idColor}>{level.Data.act}-{level.Data.level}</color> ");
            description.Append($"<color={scnLevelSelect.levelNameColor}>{level.Data.name}</color>\n");

            Rank rank = RankUtil.GetHighestRank(level.Data.hash);
            if (rank == Rank.NotFinished)
            {
                description.Append($"{(level.Data.dontUseRank ? RDString.Get("levelSelect.incomplete") : RDString.Get("levelSelect.unplayed"))}");
            }
            else
            {
                if (level.Data.dontUseRank)
                {
                    if (rank.perfected)
                    {
                        description.Append($"<color={scnLevelSelect.perfectColor}>{RDString.Get("levelSelect.perfect")}</color>");
                    }
                    else
                    {
                        description.Append(RDString.Get("levelSelect.completed"));
                    }
                }
                else
                {
                    if (rank.perfected)
                    {
                        description.Append(RDString.Get("levelSelect.rank")
                            .Replace("[rank]", $"<color={scnLevelSelect.perfectColor}>{rank.ToString()}</color>"));
                    }
                    else if (rank.passed)
                    {
                        description.Append(RDString.Get("levelSelect.rank")
                            .Replace("[rank]", $"<color=white>{rank.ToString()}</color>"));
                    }
                    else
                    {
                        description.Append(RDString.Get("levelSelect.rank")
                            .Replace("[rank]", $"<color={scnLevelSelect.failedColor}>{rank.ToString()}</color>"));

                        description.Append($"\n<color={scnLevelSelect.failedColor}>{RDString.Get("levelSelect.passCondition")}</color>");
                    }
                }
            }

            levelDescription.text = description.ToString();
        }

        private Text levelDescription = null;
        private RectTransform levelDescriptionTransform = null;
    }
}
