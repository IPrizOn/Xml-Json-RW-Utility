using System.Text.RegularExpressions;

namespace Xml_Json_RW_Utility.AppDataFile
{
    internal class StringCheck
    {
        // Является ли файл для работы XML форматом
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

        // Начинается ли строка на цифру и содержит ли знаки препинания
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

        // Существует ли такой тег в словаре
        public static bool IsTagExist(string tagName, Dictionary<string, List<string>> tags)
        {
            foreach(var tag in tags.Keys)
            {
                if(tagName == tag)
                {
                    return false;
                }
            }

            return true;
        }

        // Существует ли такой элемент в списке
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

        // Преобразование строки в имя элемента
        public static string ElementWithoutSymbols(bool isXml, string input)
        {
            if(isXml)
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
