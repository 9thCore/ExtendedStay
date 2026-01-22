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
            public string id = null;
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
            public string perfectExpression = "Happy";
            public string failExpression = "Barely";
            public string unplayedExpression = null;

            public Status Register()
            {
                if (id == null)
                {
                    return Status.InvalidID;
                }
                
                if (hash == null)
                {
                    return Status.InvalidHash;
                }

                Instance.Register(Build());
                return Status.Ok;
            }

            private Data Build()
            {
                return new Data(id, hash, name, act, level, position, character, customCharacter,
                    levelType, dontUseRank, descriptionOffset, perfectExpression, failExpression,
                    unplayedExpression);
            }

            public enum Status
            {
                Ok,
                InvalidID,
                InvalidHash
            }
        }

        public readonly record struct Data(string Id, string Hash, string Name, string Act, string Level, Vector2 Position,
            Character Character, string CustomCharacter, LevelType LevelType, bool DontUseRank, Vector2 DescriptionOffset,
            string PerfectExpression, string FailExpression, string UnplayedExpression);

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
