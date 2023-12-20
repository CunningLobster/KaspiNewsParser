using NewsParser.Core.Entities;
using NewsParser.Core.RepositoryContracts;

namespace NewsParser.Infrastructure.Repositories
{
    public class NewsPostRepository : INewsPostRepository
    {


        public Task<List<NewsPost>?> AddNewsPosts(List<NewsPost> newsPosts)
        {
            //AddStoreProcedure
            throw new NotImplementedException();
        }

        public Task<List<NewsPost>?> GetAllNewsPosts()
        {
            //GetAllStoreProcedure
            throw new NotImplementedException();
        }

        public Task<NewsPost?> GetNewsPostById(Guid id)
        {
            //GetByIdStoreProcedure
            throw new NotImplementedException();
        }
    }
}