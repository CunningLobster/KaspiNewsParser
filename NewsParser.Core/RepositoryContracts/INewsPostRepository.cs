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
        /// Получить новостной пост по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор новостного поста</param>
        /// <returns>Новостной пост с данным идентификатором</returns>
        Task<NewsPost?> GetNewsPostById(Guid id);
        /// <summary>
        /// Добавить новостные посты в БД
        /// </summary>
        /// <param name="newsPosts">Посты для добавления</param>
        /// <returns>Добавленные посты</returns>
        Task<List<NewsPost>?> AddNewsPosts(List<NewsPost> newsPosts);
    }
}