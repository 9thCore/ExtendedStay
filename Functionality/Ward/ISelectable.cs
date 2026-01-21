using UnityEngine;

namespace ExtendedStay.Functionality.Ward
{
    internal interface ISelectable
    {
        public Vector2 Position { get; }

        public void OnHover();
        public void OnInteract();
    }
}
