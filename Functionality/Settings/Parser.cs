using System.Reflection;

namespace ExtendedStay.Functionality.Settings
{
    public class Parser : BaseParser
    {
        public override string Identifier => "SETTINGS";

        protected override void OnStartParse()
        {
            variableName = "b0";
        }

        [CommentMethod]
        public void Variable(string variableName) => this.variableName = variableName;

        protected override bool TryFinishParse(out ParseManager.FailureReason reason)
        {
            FieldInfo variableField = typeof(LevelBase).GetField(variableName);
            Storage.Instance.fieldToSetToDetectTheModIsLoaded = variableField;

            reason = ParseManager.FailureReason.NoFailure;
            return true;
        }

        private string variableName;
    }
}
