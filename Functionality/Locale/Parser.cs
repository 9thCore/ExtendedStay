using ExtendedStay.Util;
using System;
using System.Text;
using UnityEngine;

namespace ExtendedStay.Functionality.Locale
{
    public class Parser : BaseParser
    {
        public override string Identifier => "TEXT";

        public override bool TryParse(string text, out ParseManager.FailureReason reason)
        {
            ParseLines(text);

            string id = null;
            SystemLanguage language = SystemLanguage.English;

            StringBuilder builder = null;
            bool readingText = false;
            while (Advance())
            {
                if (readingText)
                {
                    builder.AppendLine(CurrentLine);
                    continue;
                }

                if (!ValidLine)
                {
                    continue;
                }

                switch (Method)
                {
                    case "id":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method ID() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out id))
                        {
                            Plugin.LogError("First parameter of ID() must be a string");
                            break;
                        }

                        break;
                    case "language":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Language() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out string languageString))
                        {
                            Plugin.LogError("First parameter of Language() must be a string");
                            break;
                        }

                        if (!Enum.TryParse(languageString, true, out language))
                        {
                            Plugin.LogError($"{languageString} is not a valid language, must be one of {EnumUtil.ListValues<SystemLanguage>()}");
                            break;
                        }

                        break;
                    case "starttext":
                        if (ParameterCount != 0)
                        {
                            Plugin.LogError("Method StartText() requires no parameters");
                            break;
                        }

                        if (string.IsNullOrEmpty(id))
                        {
                            Plugin.LogError("The text has an invalid ID.");
                            reason = ParseManager.FailureReason.InvalidEvent;
                            return false;
                        }

                        readingText = true;
                        builder = new();
                        break;
                }
            }

            Storage.Instance.Register(id, language, builder.ToString());
            reason = ParseManager.FailureReason.NoFailure;
            return true;
        }
    }
}
