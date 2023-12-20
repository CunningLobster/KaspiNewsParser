using NewsParser.Core.DTO;

namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostHtmlGetterService
    {
        /// <summary>
        /// Получить новостной пост из HTML элемента
        /// </summary>
        /// <param name="htmlElement">HTML элемент</param>
        /// <returns>Новостной пост в виде ДТО</returns>
        public NewsPostDto? GetNewsPostFromHtml(HtmlDto htmlElement);
        /// <summary>
        /// Получить все новостные посты из HTML элемента
        /// </summary>
        /// <param name="htmlElement">HTML элемент</param>
        /// <returns>Новостной пост в виде ДТО</returns>
        public List<NewsPostDto>? GetAllNewsPostsFromHtml(HtmlDto htmlElement);
    }
}