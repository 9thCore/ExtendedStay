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

        public bool TryParse(string text, out FailureReason reason)
        {
            Match match = ModuleMatcher.Match(text);
            if (!match.Success)
            {
                reason = FailureReason.NoMatch;
                return false;
            }

            string identifier = match.Groups[1].Value;
            
            if (registeredManagers.TryGetValue(identifier, out BaseParser manager))
            {
                string restOfTheOwl = match.Groups[2].Value;
                return manager.TryParse(restOfTheOwl, out reason);
            }

            reason = FailureReason.InvalidManager;
            return false;
        }

        public enum FailureReason
        {
            NoFailure,
            NoMatch,
            InvalidManager,
            InvalidEvent
        }

        private ParseManager()
        {
            Register(new Settings.Parser());
            Register(new Level.Parser());
            Register(new Locale.Parser());
        }

        private void Register(BaseParser manager)
        {
            registeredManagers.Add(manager.Identifier, manager);
        }

        private readonly Dictionary<string, BaseParser> registeredManagers = new();

        private static readonly Regex ModuleMatcher = new("^EXSTAY\\.(\\w+)(.*)$", RegexOptions.Singleline);
    }
}
