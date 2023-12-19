namespace NewsParser.Core.DTO
{
    public class HtmlDto
    {
        /// <summary>
        /// Элемент в виде строки
        /// </summary>
        public string InnerHtml { get; set; }

        public HtmlDto(string html)
        {
            InnerHtml = html;
        }

        public override string ToString()
        {
            return InnerHtml;
        }
    }
}