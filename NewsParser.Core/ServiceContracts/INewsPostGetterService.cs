using NewsParser.Core.DTO;
using NewsParser.Core.Entities;

namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostGetterService
    {
        /// <summary>
        /// Получить новостной пост из HTML элемента
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        public NewsPost? GetNewsPostFromHtml(HtmlDto htmlElement);
        /// <summary>
        /// Получить список новостных постов в промежутке между датами
        /// </summary>
        /// <param name="newsPosts">Список новостных постов</param>
        /// <param name="from">Дата с</param>
        /// <param name="to">Дата по</param>
        /// <returns>Отфильтрованный список постов</returns>
        public Task<List<NewsPost>>? GetNewsPostsByDates(List<NewsPost> newsPosts, DateTime? from, DateTime? to);
    }
}