using System.Collections.Generic;

namespace ExtendedStay.Util
{
    internal static class StringUtil
    {
        public static string[] Tokenise(this string text)
        {
            List<string> list = new();

            bool inQuotes = false;
            int startOfWord = -1;
            for (int i = 0; i < text.Length; i++)
            {
                switch (text[i])
                {
                    case '"':
                        inQuotes = !inQuotes;
                        break;
                    case ' ':
                    case '\t':
                        if (!inQuotes)
                        {
                            list.Add(text.Substring(startOfWord, i - startOfWord).TrimEnd('"'));
                            startOfWord = -1;
                        }

                        break;
                    default:
                        if (startOfWord == -1)
                        {
                            startOfWord = i;
                        }
                        
                        break;
                }
            }

            if (!inQuotes && startOfWord != -1)
            {
                list.Add(text.Substring(startOfWord, text.Length - startOfWord).TrimEnd('"'));
            }

            return list.ToArray();
        }
    }
}
