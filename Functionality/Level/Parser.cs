using System;
using UnityEngine;

namespace ExtendedStay.Functionality.Level
{
    public class Parser : BaseParser
    {
        public override string Identifier => "LEVEL";

        public override bool TryParse(string text, out ParseManager.FailureReason reason)
        {
            ParseLines(text);

            Storage.Factory factory = new();

            while (Advance())
            {
                switch (Method)
                {
                    case "Hash":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out factory.hash))
                        {
                            //
                        }

                        break;
                    case "Name":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out factory.name))
                        {
                            //
                        }

                        break;
                    case "Act":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out factory.act))
                        {
                            //
                        }

                        break;
                    case "Level":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out factory.level))
                        {
                            //
                        }

                        break;
                    case "Position":
                        if (ParameterCount == 2
                            && TryGetFloatParameter(out float x)
                            && TryGetFloatParameter(out float y))
                        {
                            factory.position = new Vector2(x, y);
                        }

                        break;
                    case "Character":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out string characterName)
                            && Enum.TryParse(characterName, out Character character))
                        {
                            factory.character = character;
                        }

                        break;
                    case "CustomCharacter":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out factory.customCharacter))
                        {
                            //
                        }

                        break;
                }
            }

            switch (factory.Register())
            {
                case Storage.Factory.Status.InvalidHash:
                    Plugin.LogError("The level has an invalid hash.");
                    reason = ParseManager.FailureReason.InvalidEvent;
                    return false;
            }

            reason = default;
            return true;
        }
    }
}
