using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    using LevelStorage = Functionality.Level.Storage;

    public class Level : ISelectable
    {
        public LevelStorage.Data Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public Level()
        {
            
        }

        public Vector2 Position => data.position;

        public void OnHover()
        {

        }

        public void OnInteract()
        {

        }

        private LevelStorage.Data data;
    }
}
