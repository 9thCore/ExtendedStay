using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExtendedStay.Functionality
{
    public class ParseManager
    {
        private static ParseManager instance;
        public static ParseManager Instance
        {
            get
            {
                instance ??= new ParseManager();
                return instance;
            }
        }

        public void Parse(string text)
        {
            Match match = ModuleMatcher.Match(text);
            if (!match.Success)
            {
                return;
            }

            string identifier = match.Groups[1].Value;
            
            if (registeredManagers.TryGetValue(identifier, out BaseParser manager))
            {
                string restOfTheOwl = match.Groups[2].Value;
                manager.Parse(restOfTheOwl);
            }
        }

        private ParseManager()
        {
            Register(new Settings.Parser());
        }

        private void Register(BaseParser manager)
        {
            registeredManagers.Add(manager.Identifier, manager);
        }

        private readonly Dictionary<string, BaseParser> registeredManagers = new();

        private static readonly Regex ModuleMatcher = new("^EXSTAY\\.(\\w+)(.*)$", RegexOptions.Singleline);
    }
}
