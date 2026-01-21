using ExtendedStay.Util;
using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    using LevelStorage = Functionality.Level.Storage;

    public class Level : SpriteHolder, ISelectable, IObject
    {
        public LevelStorage.Data Data
        {
            get
            {
                return data;
            }
        }

        public Vector2 Position => transform.position;
        public Vector2 DescriptionPosition => new(Position.x + data.descriptionOffset.x - scrVfxControl.instance.RDWidth / 2f, data.descriptionOffset.y);

        public Level(LevelStorage.Data data) : base(data.character, data.customCharacter)
        {
            this.data = data;
            transform.position = data.position.AsPercent();
        }

        public void OnHover()
        {

        }

        public void OnInteract()
        {

        }

        public void Destroy()
        {
            CleanUp();
        }

        private readonly LevelStorage.Data data;
    }
}
