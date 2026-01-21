using System.Reflection;

namespace ExtendedStay.Functionality.Settings
{
    public class Parser : BaseParser
    {
        public override string Identifier => "SETTINGS";

        public override bool TryParse(string text, out ParseManager.FailureReason reason)
        {
            ParseLines(text);

            while (Advance())
            {
                switch (Method)
                {
                    case "Variable":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out string variableName))
                        {
                            FieldInfo variableField = typeof(LevelBase).GetField(variableName);
                            Storage.Instance.fieldToSetToDetectTheModIsLoaded = variableField;
                        }

                        break;
                }
            }

            reason = default;
            return true;
        }
    }
}
