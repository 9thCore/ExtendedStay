using System;

namespace ExtendedStay.Util
{
    internal static class EnumUtil
    {
        public static string ListValues<T>() where T : Enum
        {
            return (typeof(T).GetEnumValues() as T[])
                .ToArrayString();
        }
    }
}
