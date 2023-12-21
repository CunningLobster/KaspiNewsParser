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

        /// <summary>
        /// Получить все новостные посты между датами. Если даты не введены, возвращает все имеющиеся посты
        /// </summary>
        /// <param name="from">Дата с MM-dd-yyyy</param>
        /// <param name="to">Дата по MM-dd-yyyy</param>
        /// <returns>Новостные посты в промежутке между указанными датами включительно</returns>
        [HttpGet]
        [Route("posts")]
        public async Task<ActionResult<List<NewsPostDto>>> GetNewsPosts(DateTime? from, DateTime? to)
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetNewsPosts), nameof(PostsController));

            if (from > to)
            {
                return Problem(
                    title: "Invalid search dates",
                    detail: "To date must be more or equal than From date",
                    statusCode: 400
                );
            }
            var newsPostsByDates = await _newsPostGetterService.GetNewsPostsByDates(from, to);

            if (newsPostsByDates == null)
                return NotFound();

            return newsPostsByDates;
        }

        /// <summary>
        /// Вернуть топ 10 самых популярных слов в тексте новостей
        /// </summary>
        /// <returns>Список топ 10 самых популярных слов</returns>
        [HttpGet]
        [Route("topten")]
        public async Task<ActionResult<List<string>>> GetTopTenWords()
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetTopTenWords), nameof(PostsController));

            var newsPosts = await _newsPostGetterService.GetNewsPostsByDates(null, null);

            if (newsPosts == null)
                return NotFound();

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
                return NotFound();

            return topTenWords;
        }

        /// <summary>
        /// Вернуть новостные посты, в названиях или тексте которых встречается указанный текст
        /// </summary>
        /// <param name="text">Текст для поиска</param>
        /// <returns>Новостные посты, содержащие искомый текст</returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<NewsPostDto>>> GetNewsPostsByText(string text)
        {
            _logger.LogInformation("Run GET Method {0} from {1}", nameof(GetNewsPostsByText), nameof(PostsController));

            var newsPostByText = await _newsPostGetterService.GetNewsPostsByText(text);

            if (newsPostByText == null)
                return NotFound();

            return newsPostByText;
        }
    }
}