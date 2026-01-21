using UnityEngine;

namespace ExtendedStay.Util
{
    internal static class PositionUtil
    {
        public static float AsPercentX(this float value)
        {
            return value * scrVfxControl.instance.RDWidth / 100f;
        }

        public static float AsPercentY(this float value)
        {
            return value * scrVfxControl.instance.RDHeight / 100f;
        }

        public static Vector2 AsPercent(this Vector2 value)
        {
            return new Vector2(value.x.AsPercentX(), value.y.AsPercentY());
        }
    }
}
