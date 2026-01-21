using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedStay.Functionality.Locale
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
            text.Clear();
        }

        public void Register(string id, SystemLanguage language, string text)
        {
            if (!this.text.ContainsKey(id))
            {
                this.text.Add(id, new());
            }

            this.text[id][language] = text;
        }

        public string GetLocalised(string id, SystemLanguage language)
        {
            if (!this.text.TryGetValue(id, out Dictionary<SystemLanguage, string> dict))
            {
                return id;
            }

            if (!dict.TryGetValue(language, out string text))
            {
                return id;
            }

            return text;
        }

        public string GetLocalised(string id)
        {
            return GetLocalised(id, Persistence.GetLanguage());
        }

        private readonly Dictionary<string, Dictionary<SystemLanguage, string>> text = new();
    }
}
