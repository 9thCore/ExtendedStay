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
            private string hash = null;
            private string name = "Unnamed";
            private string act = "1";
            private string level = "1";
            private Vector2 position = new(50, 50);

            public void SetHash(string hash)
            {
                this.hash = hash;
            }

            public void SetName(string name)
            {
                this.name = name;
            }

            public void SetAct(string act)
            {
                this.act = act;
            }

            public void SetLevel(string level)
            {
                this.level = level;
            }

            public void SetPosition(Vector2 position)
            {
                this.position = position;
            }

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
                return new Data(hash, name, act, level, position);
            }

            public enum Status
            {
                Ok,
                InvalidHash
            }
        }

        public readonly record struct Data(string hash, string name,
            string act, string level, Vector2 position)
        {
            public readonly string hash = hash;
            public readonly string name = name;
            public readonly string act = act;
            public readonly string level = level;
            public readonly Vector2 position = position;

            public readonly Character character = Character.LuckyBaseball;
        }

        private void Register(Data data)
        {
            levelData.Add(data);
        }

        private readonly List<Data> levelData = new();
    }
}
