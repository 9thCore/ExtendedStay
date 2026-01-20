using System.Reflection;

namespace ExtendedStay.Functionality.Settings
{
    public class Parser : BaseParser
    {
        public override string Identifier => "SETTINGS";

        public override void Parse(string text)
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
        }
    }
}
