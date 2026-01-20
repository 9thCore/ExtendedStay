namespace ExtendedStay.Functionality
{
    public abstract class BaseParser
    {
        public abstract string Identifier { get; }

        public abstract void Parse(string text);

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

            currentLineTokens = currentLines[lineIndex].Split(' ', '\t');
            tokenIndex = 1;
            return true;
        }

        protected bool TryGetStringParameter(out string parameter)
        {
            if (tokenIndex >= currentLines.Length)
            {
                parameter = default;
                return false;
            }

            parameter = currentLineTokens[tokenIndex++];
            return true;
        }

        protected string Method => currentLineTokens[0];
        protected int ParameterCount => currentLineTokens.Length - 1;

        private string[] currentLineTokens = null;
        private string[] currentLines = null;
        private int lineIndex = 0;
        private int tokenIndex = 0;
    }
}
