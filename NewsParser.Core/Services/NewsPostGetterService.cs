using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NewsParser.Core.DTO;
using NewsParser.Core.Entities;
using NewsParser.Core.Extensions;
using NewsParser.Core.RepositoryContracts;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class NewsPostGetterService : INewsPostGetterService
    {
        private readonly ILogger<NewsPostGetterService> _logger;
        private readonly INewsPostRepository _newsPostRepository;

        public NewsPostGetterService(ILogger<NewsPostGetterService> logger, INewsPostRepository newsPostRepository)
        {
            _newsPostRepository = newsPostRepository;
            _logger = logger;
        }

        public async Task<List<NewsPostDto>?> GetNewsPostsByDates(DateTime? from, DateTime? to)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostsByDates), nameof(NewsPostGetterService));

            List<NewsPost>? newsPosts = await _newsPostRepository.GetAllNewsPosts();
            if (newsPosts == null)
                return null;

            if (from == null)
                from = DateTime.MinValue;
            if (to == null)
                to = DateTime.MaxValue;

            List<NewsPost>? newsPostsByDates = newsPosts.Where(n => n.PostDate >= from && n.PostDate <= to).ToList();

            return newsPostsByDates.Select(n => n.ToNewsPostDto()).ToList();
        }

        public async Task<List<NewsPostDto>?> GetNewsPostsByText(string searchText)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostsByText), nameof(NewsPostGetterService));

            List<NewsPost>? newsPosts = await _newsPostRepository.GetAllNewsPosts();
            if (newsPosts == null)
                return null;

            Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);

            List<NewsPostDto> newsPostDtos = newsPosts.Select(n => n.ToNewsPostDto()).ToList();
            List<NewsPostDto>? resultPosts = newsPostDtos.Where(p =>
            {
                bool matchInTitle = regex.IsMatch(p.Title);
                bool matchInText = p.Text != null ? regex.IsMatch(p.Text.ToPlainText()) : false;
                return matchInTitle || matchInText;
            }).ToList();

            if (resultPosts == null)
                return null;

            return resultPosts;
        }
    }
}