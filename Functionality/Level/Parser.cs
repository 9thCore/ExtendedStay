using UnityEngine;

namespace ExtendedStay.Functionality.Level
{
    public class Parser : BaseParser
    {
        public override string Identifier => "LEVEL";

        [CommentMethod]
        public void ID(string id) => factory.id = id;

        [CommentMethod]
        public void Hash(string hash) => factory.hash = hash;

        [CommentMethod]
        public void Name(string name) => factory.name = name;

        [CommentMethod]
        public void Act(string act) => factory.act = act;

        [CommentMethod]
        public void Level(string level) => factory.level = level;

        [CommentMethod]
        public void Position(float x, float y) => factory.position = new Vector2(x, y);

        [CommentMethod]
        public void Character(Character character) => factory.character = character;

        [CommentMethod]
        public void CustomCharacter(string customCharacter) => factory.customCharacter = customCharacter;

        [CommentMethod]
        public void LevelType(Storage.LevelType levelType) => factory.levelType = levelType;

        [CommentMethod]
        public void DontUseRank() => factory.dontUseRank = true;

        [CommentMethod]
        public void DescriptionOffset(float x, float y) => factory.descriptionOffset = new Vector2(x, y);

        protected override void OnStartParse()
        {
            factory = new();
        }

        protected override bool TryFinishParse(out ParseManager.FailureReason reason)
        {
            switch (factory.Register())
            {
                case Storage.Factory.Status.InvalidHash:
                    Plugin.LogError("The level has an invalid hash.");
                    reason = ParseManager.FailureReason.InvalidEvent;
                    return false;
                case Storage.Factory.Status.InvalidID:
                    Plugin.LogError("The level has an invalid ID.");
                    reason = ParseManager.FailureReason.InvalidEvent;
                    return false;
            }

            reason = ParseManager.FailureReason.NoFailure;
            return true;
        }

        private Storage.Factory factory;
    }
}
