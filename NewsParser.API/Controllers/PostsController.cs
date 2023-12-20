using System.Text;
using Microsoft.AspNetCore.Mvc;
using NewsParser.Core.DTO;
using NewsParser.Core.Extensions;
using NewsParser.Core.ServiceContracts;

namespace NewsParser.API.Controllers
{
    public class PostsController : CustomControllerBase
    {
        private readonly IHttpService _httpService;
        private readonly INewsPostGetterService _newsPostGetterService;
        private readonly ITextAnalizerService _textAnalizerService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IHttpService httpService, INewsPostGetterService newsPostGetterService, ITextAnalizerService textAnalizerService, ILogger<PostsController> logger)
        {
            _logger = logger;
            _textAnalizerService = textAnalizerService;
            _httpService = httpService;
            _newsPostGetterService = newsPostGetterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewsPostDto>>> GetNewsPosts(DateTime? from, DateTime? to)
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetNewsPosts), nameof(PostsController));

            string httpResponseString = await _httpService.GetHttpResponse("https://habr.com/ru/articles/top/alltime/");

            HtmlDto html = new HtmlDto(httpResponseString);

            var newsPosts = _newsPostGetterService.GetAllNewsPostsFromHtml(html);
            var newsPostsByDates = await _newsPostGetterService.GetNewsPostsByDates(newsPosts, from, to);

            if (newsPostsByDates == null)
                return NotFound();

            return newsPostsByDates;
        }

        [HttpGet]
        [Route("topten")]
        public async Task<ActionResult<List<string>>> GetTopTenWords()
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetTopTenWords), nameof(PostsController));

            string httpResponseString = await _httpService.GetHttpResponse("https://habr.com/ru/articles/top/alltime/");

            HtmlDto html = new HtmlDto(httpResponseString);

            var newsPosts = _newsPostGetterService.GetAllNewsPostsFromHtml(html);

            if (newsPosts == null)
            {
                return NotFound();
            }
            //Получить текст всех новостей
            StringBuilder allTextBuilder = new StringBuilder();
            foreach (var post in newsPosts)
            {
                allTextBuilder.Append(post.Text?.ToPlainText());
            }
            string allText = allTextBuilder.ToString();

            Dictionary<string, int>? topTenWordsDict = _textAnalizerService.GetTopWords(allText, 10);

            List<string>? topTenWords = topTenWordsDict?.Select(d => d.Key).ToList();

            if (topTenWords == null)
            {
                return NotFound();
            }
            return topTenWords;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<NewsPostDto>>> GetNewsPostsByText(string text)
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetNewsPostsByText), nameof(PostsController));

            string httpResponseString = await _httpService.GetHttpResponse("https://habr.com/ru/articles/top/alltime/");

            HtmlDto html = new HtmlDto(httpResponseString);

            var newsPosts = _newsPostGetterService.GetAllNewsPostsFromHtml(html);

            List<NewsPostDto>? newsPostByText = await _newsPostGetterService.GetNewsPostsByText(newsPosts, text);
            if (newsPostByText == null)
                return NotFound();

            return newsPostByText;
        }
    }
}