using System.Text;
using UnityEngine;

namespace ExtendedStay.Functionality.Locale
{
    public class Parser : BaseParser
    {
        public override string Identifier => "TEXT";

        [CommentMethod]
        public void Id(string id)
        {
            this.id = id;
        }

        [CommentMethod]
        public void Language(SystemLanguage language)
        {
            this.language = language;
        }

        [CommentMethod]
        public void AddLine(string line)
        {
            builder.AppendLine(line);
        }

        [CommentMethod]
        public void NewLine()
        {
            builder.AppendLine();
        }

        protected override void OnStartParse()
        {
            id = null;
            language = SystemLanguage.English;
            builder = new();
        }

        protected override bool TryFinishParse(out ParseManager.FailureReason reason)
        {
            if (id == null)
            {
                Plugin.LogError($"The text has an invalid ID.");
                reason = ParseManager.FailureReason.InvalidEvent;
                return false;
            }

            Storage.Instance.Register(id, language, builder.ToString());

            reason = ParseManager.FailureReason.NoFailure;
            return true;
        }

        private string id;
        private SystemLanguage language;
        private StringBuilder builder;
    }
}
