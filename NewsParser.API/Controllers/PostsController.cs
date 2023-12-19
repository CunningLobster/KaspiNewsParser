using Microsoft.AspNetCore.Mvc;
using NewsParser.Core.DTO;
using NewsParser.Core.Entities;
using NewsParser.Core.ServiceContracts;
using NewsParser.Core.Extensions;

namespace NewsParser.API.Controllers
{
    public class PostsController : CustomControllerBase
    {
        private readonly IHttpService _httpService;
        private readonly INewsPostGetterService _newsPostGetterService;

        public PostsController(IHttpService httpService, INewsPostGetterService newsPostGetterService)
        {
            _httpService = httpService;
            _newsPostGetterService = newsPostGetterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewsPostDto>>> GetNewsPosts()
        {
            string httpResponseString = await _httpService.GetHttpResponse("https://habr.com/ru/articles/top/alltime/");

            HtmlDto html = new HtmlDto(httpResponseString);

            var newsPosts = _newsPostGetterService.GetAllNewsPostsFromHtml(html);

            if (newsPosts == null)
                return NotFound();

            return newsPosts;
        }
    }
}