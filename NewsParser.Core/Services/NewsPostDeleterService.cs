using NewsParser.Core.RepositoryContracts;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class NewsPostDeleterService : INewsPostDeleterService
    {
        private readonly INewsPostRepository _newsPostRepository;

        public NewsPostDeleterService(INewsPostRepository newsPostRepository)
        {
            _newsPostRepository = newsPostRepository;
        }

        public async Task<bool> DeleteAllNewsPosts()
        {
            return await _newsPostRepository.DeleteAllNewsPosts();
        }
    }
}