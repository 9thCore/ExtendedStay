using System.Collections.Generic;
using UnityEngine;

namespace ExtendedStay.Functionality.Level
{
    public class Storage
    {
        private static Storage instance;
        public static Storage Instance
        {
            get
            {
                instance ??= new Storage();
                return instance;
            }
        }

        public void Clear()
        {
            levelData.Clear();
        }

        public IEnumerable<Data> LevelData => levelData;

        public class Factory
        {
            public string hash = null;
            public string name = "Unnamed";
            public string act = "1";
            public string level = "1";
            public Vector2 position = new(50, 50);
            public Character character = Character.Samurai;
            public string customCharacter = string.Empty;
            public LevelType levelType = LevelType.Normal;
            public bool dontUseRank = false;
            public Vector2 descriptionOffset = Vector2.zero;

            public Status Register()
            {
                if (!Valid())
                {
                    return Status.InvalidHash;
                }

                Instance.Register(Build());
                return Status.Ok;
            }

            private bool Valid()
            {
                return hash != null;
            }

            private Data Build()
            {
                return new Data(hash, name, act, level, position, character, customCharacter,
                    levelType, dontUseRank, descriptionOffset);
            }

            public enum Status
            {
                Ok,
                InvalidHash
            }
        }

        public readonly record struct Data(string hash, string name, string act, string level, Vector2 position,
            Character character, string customCharacter, LevelType levelType, bool dontUseRank, Vector2 descriptionOffset)
        {
            public readonly string hash = hash;
            public readonly string name = name;
            public readonly string act = act;
            public readonly string level = level;
            public readonly Vector2 position = position;

            public readonly Character character = character;
            public readonly string customCharacter = customCharacter;

            public readonly LevelType levelType = levelType;
            public readonly bool dontUseRank = dontUseRank;

            public readonly Vector2 descriptionOffset = descriptionOffset;
        }

        public enum LevelType
        {
            Normal,
            Boss,
            Intermission,
            Bonus
        }

        private void Register(Data data)
        {
            levelData.Add(data);
        }

        private readonly List<Data> levelData = new();
    }
}
