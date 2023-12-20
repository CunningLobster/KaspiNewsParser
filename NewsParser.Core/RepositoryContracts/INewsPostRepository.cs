using NewsParser.Core.Entities;

namespace NewsParser.Core.RepositoryContracts
{
    public interface INewsPostRepository
    {
        /// <summary>
        /// Получить все новостные посты
        /// </summary>
        /// <returns>Список новостных постов</returns>
        Task<List<NewsPost>?> GetAllNewsPosts();
        /// <summary>
        /// Добавить новостные посты в БД
        /// </summary>
        /// <param name="newsPosts">Посты для добавления</param>
        /// <returns>Количество добавленных постов</returns>
        Task<List<NewsPost>?> AddNewsPosts(List<NewsPost> newsPosts);
        /// <summary>
        /// Удалить все новостные посты из таблицы
        /// </summary>
        /// <returns>true, если удаление прошло успешно</returns>
        Task<bool> ClearNewsPostTable();
    }
}