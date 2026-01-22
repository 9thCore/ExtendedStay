using ExtendedStay.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ExtendedStay.Functionality
{
    using LevelStorage = Level.Storage;

    public abstract class BaseParser
    {
        public BaseParser()
        {
            CacheCommentMethods();
        }

        public abstract string Identifier { get; }

        public virtual bool TryParse(string text, out ParseManager.FailureReason reason)
        {
            ParseLines(text);

            OnStartParse();
            while (Advance())
            {
                if (!ValidLine)
                {
                    continue;
                }

                if (cachedMethods.TryGetValue(Method, out CommentMethod method))
                {
                    if (!method.TryInvoke(currentLineTokens, out CommentMethod.FailureReason methodReason))
                    {
                        Plugin.LogError($"Invalid method call of {method.name} ({methodReason}), skipping");
                    }
                }
            }

            return TryFinishParse(out reason);
        }

        protected abstract void OnStartParse();
        protected abstract bool TryFinishParse(out ParseManager.FailureReason reason);

        protected void ParseLines(string text)
        {
            currentLines = text.Split('\n', '\r');
            lineIndex = -1;

            for (int i = 0; i < currentLines.Length; i++)
            {
                currentLines[i] = currentLines[i].Trim(' ', '\t');
            }
        }

        protected bool Advance()
        {
            lineIndex++;

            if (lineIndex >= currentLines.Length)
            {
                return false;
            }

            currentLineTokens = currentLines[lineIndex].Tokenise();

            if (ValidLine)
            {
                currentLineTokens[0] = currentLineTokens[0].ToLower();
            }

            return true;
        }

        protected bool ValidLine => currentLineTokens.Length > 0;
        protected string Method => currentLineTokens[0];

        [AttributeUsage(AttributeTargets.Method)]
        protected class CommentMethodAttribute : Attribute
        {

        }

        private void CacheCommentMethods()
        {
            IEnumerable<MethodInfo> methods = GetType().GetMethods()
                .Where(method => method.GetCustomAttribute<CommentMethodAttribute>() != null);

            foreach (MethodInfo method in methods)
            {
                cachedMethods.Add(method.Name.ToLower(), new CommentMethod(this, method.Name, method));
            }
        }

        private string[] currentLineTokens = null;
        private string[] currentLines = null;
        private int lineIndex = 0;

        private readonly Dictionary<string, CommentMethod> cachedMethods = new();

        private readonly record struct CommentMethod
        {
            public readonly BaseParser instance;
            public readonly string name;
            public readonly MethodInfo method;
            public readonly List<Parameter> parameters = new();
            public readonly int requiredParameterCount;
            public readonly int totalParameterCount;
            public readonly int optionalParameterCount;

            public CommentMethod(BaseParser instance, string name, MethodInfo method)
            {
                this.instance = instance;
                this.name = name;
                this.method = method;

                requiredParameterCount = 0;
                optionalParameterCount = 0;

                ParameterInfo[] methodParameters = method.GetParameters();
                foreach (ParameterInfo parameter in methodParameters)
                {
                    Parameter.Data data = new(
                        Optional: parameter.IsOptional);

                    if (!data.Optional)
                    {
                        requiredParameterCount++;
                    }
                    else
                    {
                        optionalParameterCount++;
                    }

                    Type type = parameter.ParameterType;
                    if (type == typeof(string))
                    {
                        parameters.Add(new StringParameter(data));
                    }
                    else if (type == typeof(SystemLanguage))
                    {
                        parameters.Add(new SystemLanguageParameter(data));
                    }
                    else if (type == typeof(Character))
                    {
                        parameters.Add(new CharacterParameter(data));
                    }
                    else if (type == typeof(float))
                    {
                        parameters.Add(new FloatParameter(data));
                    }
                    else if (type == typeof(LevelStorage.LevelType))
                    {
                        parameters.Add(new LevelTypeParameter(data));
                    }
                    else
                    {
                        Plugin.LogError($"DEV: {parameter.Name} has invalid parameter type {type}");
                        parameters.Add(new InvalidParameter(data));
                    }
                }

                totalParameterCount = requiredParameterCount + optionalParameterCount;
            }

            public bool TryInvoke(string[] tokens, out FailureReason reason)
            {
                int suppliedParameterCount = tokens.Length - 1;

                if (suppliedParameterCount < requiredParameterCount
                    || suppliedParameterCount > totalParameterCount)
                {
                    string supplyText = $"{suppliedParameterCount} {(suppliedParameterCount == 1 ? "was" : "were")}";

                    if (optionalParameterCount == 0)
                    {
                        Plugin.LogError($"Method {name}() requires {totalParameterCount} parameters, but {supplyText} supplied instead");
                        reason = FailureReason.InvalidParameterCount;
                        return false;
                    }
                    else
                    {
                        Plugin.LogError($"Method {name}() requires between {requiredParameterCount} and {totalParameterCount} parameters, but {supplyText} supplied instead");
                        reason = FailureReason.InvalidParameterCount;
                        return false;
                    }
                }

                object[] invokingParameters = new object[totalParameterCount];

                int index = 0;

                foreach (Parameter parameter in parameters)
                {
                    if (index >= suppliedParameterCount)
                    {
                        if (!parameter.data.Optional)
                        {
                            Plugin.LogError($"Parameter #{index + 1} {parameter.ErrorMessage}");
                            reason = FailureReason.InvalidParameter;
                            return false;
                        }

                        invokingParameters[index] = Type.Missing;
                        index++;
                        continue;
                    }

                    if (!parameter.TryParse(tokens[index + 1], out object result))
                    {
                        Plugin.LogError($"Parameter #{index + 1} {parameter.ErrorMessage}");
                        reason = FailureReason.InvalidParameter;
                        return false;
                    }

                    invokingParameters[index] = result;
                    index++;
                }

                reason = FailureReason.NoFailure;
                method.Invoke(instance, invokingParameters);
                return true;
            }

            public abstract record Parameter(Parameter.Data data)
            {
                public readonly Data data = data;

                public abstract string ErrorMessage { get; }

                public abstract bool TryParse(string token, out object result);

                public readonly record struct Data(bool Optional);
            }

            public record InvalidParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => "must be some type... that this parser currently does not support!";

                public override bool TryParse(string token, out object result)
                {
                    result = default;
                    return false;
                }
            }

            public record StringParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => "must be a string";

                public override bool TryParse(string token, out object result)
                {
                    result = token;
                    return true;
                }
            }

            public record SystemLanguageParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => $"must be a language, one of {EnumUtil.ListValues<SystemLanguage>()}";

                public override bool TryParse(string token, out object result)
                {
                    if (!Enum.TryParse(token, out SystemLanguage value))
                    {
                        result = default;
                        return false;
                    }

                    result = value;
                    return true;
                }
            }

            public record CharacterParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => $"must be a character, one of {EnumUtil.ListValues<Character>()}";

                public override bool TryParse(string token, out object result)
                {
                    if (!Enum.TryParse(token, out Character value))
                    {
                        result = default;
                        return false;
                    }

                    result = value;
                    return true;
                }
            }

            public record FloatParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => "must be a float";

                public override bool TryParse(string token, out object result)
                {
                    if (!float.TryParse(token, out float value))
                    {
                        result = default;
                        return false;
                    }

                    result = value;
                    return true;
                }
            }

            public record LevelTypeParameter(Parameter.Data data) : Parameter(data)
            {
                public override string ErrorMessage => $"must be a level type, one of {EnumUtil.ListValues<LevelStorage.LevelType>()}";

                public override bool TryParse(string token, out object result)
                {
                    if (!Enum.TryParse(token, out LevelStorage.LevelType value))
                    {
                        result = default;
                        return false;
                    }

                    result = value;
                    return true;
                }
            }

            public enum FailureReason
            {
                NoFailure,
                InvalidParameter,
                InvalidParameterCount
            }
        }
    }
}
