using System.Text.RegularExpressions;
using NewsParser.Core.DTO;

namespace HabrParcerConsole
{
    public static class HtmlParser
    {
        /// <summary>
        /// Получить первый элемент по имени класса
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором находится элемент с искомым классом</param>
        /// <param name="className">Имя класса</param>
        /// <returns>HTML элемент в виде ДТО объекта</returns>
        public static HtmlDto? GetFirstElementByClassName(this HtmlDto HtmlDto, string className)
        {
            return GetElementByClassNameFromIndex(HtmlDto, className, 0);
        }

        /// <summary>
        /// Получить первый элемент по тэгу
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором находится элемент с искомым тэгом</param>
        /// <param name="tagName">Имя тэга</param>
        /// <returns>HTML элемент в виде ДТО объекта</returns>
        public static HtmlDto? GetFirstElementByTagName(this HtmlDto HtmlDto, string tagName)
        {
            return GetElementByTagNameFromIndex(HtmlDto, tagName, 0);
        }

        /// <summary>
        /// Найти индекс элемента, где тэг закрывается
        /// </summary>
        /// <param name="tagName">Имя тэга</param>
        /// <param name="htmlText">HTML элемент</param>
        /// <returns>Индекс элемента, где тэг закрывается</returns>
        private static int FindElementCloseIndex(string tagName, string htmlText)
        {
            string openTag = $"<{tagName}";
            string closeTag = $"</{tagName}>";

            int openCount = 0;
            int closeCount = 0;
            int currentSearchIndex = 0;

            int closeIndex = htmlText.IndexOf(closeTag, currentSearchIndex);
            int openIndex = htmlText.IndexOf(openTag, currentSearchIndex);

            do
            {
                if (closeIndex > openIndex && openIndex != -1)
                {
                    openCount++;
                    currentSearchIndex = openIndex + openTag.Length;
                    openIndex = htmlText.IndexOf(openTag, currentSearchIndex);
                }
                else
                {
                    closeCount++;
                    currentSearchIndex = closeIndex + closeTag.Length;
                    closeIndex = htmlText.IndexOf(closeTag, currentSearchIndex);
                }
            }
            while (openCount > closeCount);

            return currentSearchIndex;
        }

        /// <summary>
        /// Получить текст внутри внешнего тэга, со всеми внутренними элементами
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, внутри которого находится искомый текст</param>
        /// <returns>Внутренний текст</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        /// <exception cref="ArgumentException">Не валидный HTML</exception>
        public static string GetInnerText(this HtmlDto HtmlDto)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            string innerHtml = HtmlDto.InnerHtml;

            if (innerHtml[0] != '<')
                throw new ArgumentException("Invalid html");

            int innerTextStart = innerHtml.IndexOf('>') + 1;

            int endTagStartIndex = innerHtml.LastIndexOf("</");

            int innerTextEnd = innerHtml.Length - (innerHtml.Length - endTagStartIndex);

            return innerHtml.Substring(innerTextStart, innerTextEnd - innerTextStart);
        }

        /// <summary>
        /// Получить значения аттрибута HTML элемента
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором содержится аттрибут</param>
        /// <param name="attribute">Имя аттрибута</param>
        /// <returns>Значение аттрибута</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        /// <exception cref="ArgumentException">Не валидный HTML</exception>
        public static string GetAttributeValue(this HtmlDto HtmlDto, string attribute)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            string innerHtml = HtmlDto.InnerHtml;

            if (innerHtml[0] != '<')
                throw new ArgumentException("Invalid html");

            int attributeIndex = innerHtml.IndexOf($"{attribute}=\"");
            if (attributeIndex == -1)
                throw new ArgumentException($"Attribute \"{attribute}\" is not found");

            int openTagEndIndex = innerHtml.IndexOf('>');
            if (attributeIndex >= openTagEndIndex)
                throw new ArgumentException($"This HTML element doesn't contain attribute \"{attribute}\"");

            int attributeValueStartIndex = attributeIndex + $"{attribute}=\"".Length;
            int attributeValueEndIndex = innerHtml.IndexOf("\"", attributeValueStartIndex);

            return innerHtml.Substring(attributeValueStartIndex, attributeValueEndIndex - attributeValueStartIndex);
        }

        /// <summary>
        /// Получить элемент по тэгу, начиная с индекса строки
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором находится элемент с искомым тэгом</param>
        /// <param name="tagName">Имя тэга</param>
        /// <param name="startIndex">Стартовый индекс</param>
        /// <returns>HTML элемент в виде ДТО объекта</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        private static HtmlDto? GetElementByTagNameFromIndex(this HtmlDto HtmlDto, string tagName, int startIndex)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            string innerHtml = HtmlDto.InnerHtml;

            int tagIndex = innerHtml.IndexOf($"<{tagName}", startIndex);

            if (tagIndex == -1)
                return null;

            int elCloseIndex = FindElementCloseIndex(tagName, innerHtml);

            int elStringLength = elCloseIndex - tagIndex;
            string element = innerHtml.Substring(tagIndex, elStringLength);

            return new HtmlDto(element);
        }

        /// <summary>
        /// Получить элемент по классу, начиная с индекса строки
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором находится элемент с искомым классом</param>
        /// <param name="className">Имя класса</param>
        /// <param name="startIndex">Индекс строки, с которого начинается поиск</param>
        /// <returns>HTML элемент в виде ДТО объекта</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        private static HtmlDto? GetElementByClassNameFromIndex(this HtmlDto HtmlDto, string className, int startIndex)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            string innerHtml = HtmlDto.InnerHtml;

            //Получить подстроку, которая начинается с тэга
            int classAttributeIndex = innerHtml.IndexOf($"class=\"{className}", startIndex);

            if (classAttributeIndex == -1)
                return null;

            //Получить подстроку до начала аттрибута класса
            string toClassAttributeIndex = innerHtml.Substring(0, classAttributeIndex);

            int tagIndex = toClassAttributeIndex.LastIndexOf('<');

            //Подстрока, начиная с тэга, в котором содержится искомый класс
            string textFromTag = innerHtml.Substring(tagIndex);

            //Получить имя тэга, в котором содержится искомый класс
            string tagName = textFromTag.Substring(1, textFromTag.IndexOf((char)32) - 1);

            //Имея подстроку и имя тэга, получим искомый элемент по тэгу
            return GetElementByTagNameFromIndex(new HtmlDto(textFromTag), tagName, 0);
        }

        /// <summary>
        /// Получить список элементов по классу
        /// </summary>
        /// <param name="HtmlDto">HTML элемент, в котором находятся элементы с искомым классом</param>
        /// <param name="className">Имя класса</param>
        /// <returns>Список HTML элементов в виде ДТО объекта</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        public static List<HtmlDto> GetAllElementsByClassName(this HtmlDto HtmlDto, string className)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            int startIndex = 0;

            List<HtmlDto> htmlElements = new List<HtmlDto>();
            HtmlDto? element = GetElementByClassNameFromIndex(HtmlDto, className, startIndex);
            while (element != null)
            {
                htmlElements.Add(element);
                startIndex = HtmlDto.InnerHtml.IndexOf(element.InnerHtml) + element.InnerHtml.Length;
                element = GetElementByClassNameFromIndex(HtmlDto, className, startIndex);
            }

            return htmlElements;
        }

        /// <summary>
        /// Преобразовать HTML элемент в чистый текст
        /// </summary>
        /// <param name="HtmlDto">HTML элемент для преобразования</param>
        /// <returns>Чистый текст без HTML форматирования</returns>
        /// <exception cref="ArgumentNullException">HtmlDto = null</exception>
        public static string ToPlainText(this HtmlDto HtmlDto)
        {
            if (HtmlDto == null)
                throw new ArgumentNullException(nameof(HtmlDto));

            string innerHtml = HtmlDto.InnerHtml;
            //matches one or more (white space or line breaks) between '>' and '<'
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";
            //match any character between '<' and '>', even when end tag is missing
            const string stripFormatting = @"<[^>]*(>|$)";
            //matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = innerHtml;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}