using System;
using System.Reflection;

namespace ExtendedStay.Functionality.Settings
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
            fieldToSetToDetectTheModIsLoaded = null;
        }

        public void OnLevelLoad(LevelBase level)
        {
            if (fieldToSetToDetectTheModIsLoaded != null)
            {
                object value = GetValueToSetFieldTo();

                if (value != null)
                {
                    try
                    {
                        fieldToSetToDetectTheModIsLoaded.SetValue(level, GetValueToSetFieldTo());
                    }
                    catch
                    {
                        Plugin.LogError($"Could not set {fieldToSetToDetectTheModIsLoaded.Name}");
                    }
                }
            }
        }

        private object GetValueToSetFieldTo()
        {
            Type type = fieldToSetToDetectTheModIsLoaded.FieldType;
            if (type == typeof(int))
            {
                return 1;
            }
            else if (type == typeof(float))
            {
                return 1.0f;
            }
            else if (type == typeof(bool))
            {
                return true;
            }

            return null;
        }

        public FieldInfo fieldToSetToDetectTheModIsLoaded = null;
    }
}
