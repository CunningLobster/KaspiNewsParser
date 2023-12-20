using NewsParser.Core.DTO;
using NewsParser.Core.Entities;

namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostGetterService
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
        /// <summary>
        /// Получить список новостных постов в промежутке между датами
        /// </summary>
        /// <param name="newsPosts">Список новостных постов</param>
        /// <param name="from">Дата с</param>
        /// <param name="to">Дата по</param>
        /// <returns>Отфильтрованный список постов в виде ДТО</returns>
        public Task<List<NewsPostDto>>? GetNewsPostsByDates(List<NewsPostDto>? newsPosts, DateTime? from, DateTime? to);
        /// <summary>
        /// Получить список новостей по тексту поиска
        /// </summary>
        /// <param name="newsPosts">Список новостных постов</param>
        /// <param name="searchText">Текст поиска</param>
        /// <returns>Отфильтрованный список постов в виде ДТО</returns>
        public Task<List<NewsPostDto>>? GetNewsPostsByText(List<NewsPostDto>? newsPosts, string searchText);
    }
}