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
                            && TryGetStringParameter(out string hash))
                        {
                            factory.SetHash(hash);
                        }

                        break;
                    case "Name":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out string name))
                        {
                            factory.SetName(name);
                        }

                        break;
                    case "Act":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out string act))
                        {
                            factory.SetAct(act);
                        }

                        break;
                    case "Level":
                        if (ParameterCount == 1
                            && TryGetStringParameter(out string level))
                        {
                            factory.SetLevel(level);
                        }

                        break;
                    case "Position":
                        if (ParameterCount == 2
                            && TryGetFloatParameter(out float x)
                            && TryGetFloatParameter(out float y))
                        {
                            factory.SetPosition(new Vector2(x, y));
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
