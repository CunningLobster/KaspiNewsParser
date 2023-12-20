using Microsoft.Extensions.Logging;
using NewsParser.Core.DTO;
using NewsParser.Core.Extensions;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.Core.Services
{
    public class HabrNewsPostHtmlGetterService : INewsPostHtmlGetterService
    {
        private readonly ILogger<HabrNewsPostHtmlGetterService> _logger;

        public HabrNewsPostHtmlGetterService(ILogger<HabrNewsPostHtmlGetterService> logger)
        {
            _logger = logger;
        }

        public List<NewsPostDto>? GetAllNewsPostsFromHtml(HtmlDto htmlElement)
        {
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetAllNewsPostsFromHtml), nameof(HabrNewsPostHtmlGetterService));

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
            _logger.LogInformation("Run Method {0} from {1}", nameof(GetNewsPostFromHtml), nameof(HabrNewsPostHtmlGetterService));

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
    }
}