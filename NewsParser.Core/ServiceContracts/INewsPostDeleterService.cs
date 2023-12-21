namespace NewsParser.Core.ServiceContracts
{
    public interface INewsPostDeleterService
    {
        /// <summary>
        /// Удалить все новостные посты из таблицы
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAllNewsPosts();
    }
}