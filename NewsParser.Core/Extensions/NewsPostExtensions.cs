using NewsParser.Core.DTO;
using NewsParser.Core.Entities;

namespace NewsParser.Core.Extensions
{
    public static class NewsPostExtensions
    {
        public static NewsPostDto ToNewsPostDto(this NewsPost newsPost)
        {
            return new NewsPostDto
            {
                Id = newsPost.Id,
                Title = newsPost.Title,
                Text = new HtmlDto(newsPost.Text),
                PostDate = newsPost.PostDate
            };
        }
    }
}