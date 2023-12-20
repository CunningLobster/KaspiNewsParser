using NewsParser.Core.DTO;

namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostGetterService
    {
        /// <summary>
        /// Получить список новостных постов в промежутке между датами
        /// </summary>
        /// <param name="from">Дата с</param>
        /// <param name="to">Дата по</param>
        /// <returns>Отфильтрованный список постов в виде ДТО</returns>
        public Task<List<NewsPostDto>?> GetNewsPostsByDates(DateTime? from, DateTime? to);
        /// <summary>
        /// Получить список новостей по тексту поиска
        /// </summary>
        /// <param name="searchText">Текст поиска</param>
        /// <returns>Отфильтрованный список постов в виде ДТО</returns>
        public Task<List<NewsPostDto>?> GetNewsPostsByText(string searchText);
    }
}