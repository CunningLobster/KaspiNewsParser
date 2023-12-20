using NewsParser.Core.DTO;

namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostAdderService
    {
        /// <summary>
        /// Добавить новостные посты из HTML элемента
        /// </summary>
        /// <param name="htmlElement">HTML элемент</param>
        /// <returns>Список добавленных постов</returns>
        Task<List<NewsPostDto>?> AddNewsPostsFromHtml(HtmlDto htmlElement);
    }
}