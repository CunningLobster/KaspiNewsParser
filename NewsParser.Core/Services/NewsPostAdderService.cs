using Microsoft.Extensions.Logging;
using NewsParser.Core.DTO;
using NewsParser.Core.Entities;
using NewsParser.Core.Extensions;
using NewsParser.Core.RepositoryContracts;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class NewsPostAdderService : INewsPostAdderService
    {
        private readonly ILogger<NewsPostAdderService> _logger;
        private readonly INewsPostRepository _newsPostRepository;
        private readonly INewsPostHtmlGetterService _newsPostHtmlGetterService;

        public NewsPostAdderService(ILogger<NewsPostAdderService> logger, INewsPostRepository newsPostRepository, INewsPostHtmlGetterService newsPostHtmlGetterService)
        {
            _newsPostHtmlGetterService = newsPostHtmlGetterService;
            _newsPostRepository = newsPostRepository;
            _logger = logger;
        }

        public async Task<List<NewsPostDto>?> AddNewsPostsFromHtml(HtmlDto htmlElement)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(AddNewsPostsFromHtml), nameof(NewsPostAdderService));

            List<NewsPostDto>? newsPostsFromHtml = _newsPostHtmlGetterService.GetAllNewsPostsFromHtml(htmlElement);

            if (newsPostsFromHtml == null)
                return null;

            List<NewsPost> newsPostsToAdd = newsPostsFromHtml.Select(n => n.ToNewsPost()).ToList();

            List<NewsPost>? newsPostsAdded = await _newsPostRepository.AddNewsPosts(newsPostsToAdd);
            return newsPostsAdded?.Select(n => n.ToNewsPostDto()).ToList();
        }
    }
}