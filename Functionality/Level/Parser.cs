using System;
using System.Text;
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
                if (!ValidLine)
                {
                    continue;
                }

                switch (Method)
                {
                    case "hash":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Hash() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out factory.hash))
                        {
                            Plugin.LogError("First parameter of Hash() must be a string");
                            break;
                        }

                        break;
                    case "name":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Name() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out factory.name))
                        {
                            Plugin.LogError("First parameter of Name() must be a string");
                            break;
                        }

                        break;
                    case "act":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Act() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out factory.act))
                        {
                            Plugin.LogError("First parameter of Act() must be a string");
                            break;
                        }

                        break;
                    case "level":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Level() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out factory.level))
                        {
                            Plugin.LogError("First parameter of Level() must be a string");
                            break;
                        }

                        break;
                    case "position":
                        if (ParameterCount != 2)
                        {
                            Plugin.LogError("Method Position() requires two parameters");
                            break;
                        }

                        if (!TryGetFloatParameter(out float x))
                        {
                            Plugin.LogError("First parameter of Position() must be a float");
                            break;
                        }

                        if (!TryGetFloatParameter(out float y))
                        {
                            Plugin.LogError("Second parameter of Position() must be a float");
                            break;
                        }

                        factory.position = new(x, y);
                        break;
                    case "character":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Character() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out string characterName))
                        {
                            Plugin.LogError("First parameter of Character() must be a string");
                            break;
                        }

                        if (!Enum.TryParse(characterName, true, out Character character))
                        {
                            Plugin.LogError($"{characterName} is not a valid character");
                            break;
                        }

                        factory.character = character;
                        break;
                    case "customcharacter":
                        if (ParameterCount != 1)
                        {
                            Plugin.LogError("Method Character() requires one parameter");
                            break;
                        }

                        if (!TryGetStringParameter(out string customCharacter))
                        {
                            Plugin.LogError("First parameter of Character() must be a string");
                            break;
                        }

                        factory.customCharacter = customCharacter;
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
