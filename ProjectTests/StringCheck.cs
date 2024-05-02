using System.Text.RegularExpressions;

namespace ProjectTests
{
    internal class StringCheck
    {
        public static bool IsXmlFile(string fileType)
        {
            if (fileType.Equals(".xml"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidName(string input)
        {
            if (Char.IsDigit(input[0]))
            {
                return false;
            }
            if (Regex.IsMatch(input, @"[.,\/#!$%\^&\*;:{}=\-_`~()]"))
            {
                return false;
            }

            return true;
        }

        public static bool IsTagExist(string tagName, Dictionary<string, List<string>> tags)
        {
            foreach (var tag in tags.Keys)
            {
                if (tagName == tag)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsElementExist(bool isXml, string elementName, List<string> elements)
        {
            foreach (var element in elements)
            {
                if (elementName == ElementWithoutSymbols(isXml, element))
                {
                    return false;
                }
            }

            return true;
        }

        public static string ElementWithoutSymbols(bool isXml, string input)
        {
            if (isXml)
            {
                int startIndex = input.IndexOf("<") + 1;
                int endIndex = input.IndexOf(">", startIndex);

                return input.Substring(startIndex, endIndex - startIndex);
            }
            else
            {
                int startIndex = input.IndexOf("\"") + 1;
                int endIndex = input.IndexOf("\"", startIndex);

                return input.Substring(startIndex, endIndex - startIndex);
            }
        }
    }
}
