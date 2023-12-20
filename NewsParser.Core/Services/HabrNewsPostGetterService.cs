using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NewsParser.Core.DTO;
using NewsParser.Core.Entities;
using NewsParser.Core.Extensions;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class HabrNewsPostGetterService : INewsPostGetterService
    {
        private readonly ILogger<HabrNewsPostGetterService> _logger;
        public HabrNewsPostGetterService(ILogger<HabrNewsPostGetterService> logger)
        {
            _logger = logger;
        }

        public List<NewsPostDto>? GetAllNewsPostsFromHtml(HtmlDto htmlElement)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetAllNewsPostsFromHtml), nameof(HabrNewsPostGetterService));

            List<HtmlDto>? articles = htmlElement.GetFirstElementByClassName("tm-articles-subpage")?.GetAllElementsByClassName("tm-articles-list__item");

            if (articles == null)
                return null;

            List<NewsPostDto> newsPosts = new List<NewsPostDto>();
            foreach (HtmlDto article in articles)
            {
                NewsPostDto? newsPost = GetNewsPostFromHtml(article);
                if (newsPost != null)
                    newsPosts.Add(newsPost);
            }

            return newsPosts;
        }

        public NewsPostDto? GetNewsPostFromHtml(HtmlDto htmlElement)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostFromHtml), nameof(HabrNewsPostGetterService));

            HtmlDto? titleHtml = htmlElement.GetFirstElementByClassName("tm-title__link")?.GetFirstElementByTagName("span");
            string title = titleHtml != null ? titleHtml.GetInnerText() : string.Empty;

            HtmlDto? textHtml = htmlElement.GetFirstElementByClassName("article-formatted-body");

            HtmlDto? postTimeHtml = htmlElement.GetFirstElementByClassName("tm-article-datetime-published")?.GetFirstElementByTagName("time");
            string postTime = postTimeHtml != null ? postTimeHtml.GetAttributeValue("datetime") : string.Empty;

            return new NewsPostDto
            {
                Id = Guid.NewGuid(),
                Title = title,
                Text = textHtml,
                PostDate = DateTime.Parse(postTime).ToUniversalTime()
            };
        }

        public async Task<List<NewsPostDto>>? GetNewsPostsByDates(List<NewsPostDto>? newsPosts, DateTime? from, DateTime? to)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostsByDates), nameof(HabrNewsPostGetterService));

            if (from == null)
                from = DateTime.MinValue;
            if (to == null)
                to = DateTime.MaxValue;

            return newsPosts.Where(n => n.PostDate >= from && n.PostDate <= to).ToList();
        }

        public async Task<List<NewsPostDto>>? GetNewsPostsByText(List<NewsPostDto>? newsPosts, string searchText)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostsByText), nameof(HabrNewsPostGetterService));

            Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);

            List<NewsPostDto>? resultPosts = newsPosts?
            .Where(p => regex.IsMatch(p.Title) || regex.IsMatch(p.Text.ToPlainText())).ToList();

            if (resultPosts == null)
                return null;

            return resultPosts;
        }
    }
}