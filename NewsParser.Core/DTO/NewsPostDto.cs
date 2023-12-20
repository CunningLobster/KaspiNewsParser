using NewsParser.Core.Entities;

namespace NewsParser.Core.DTO
{
    public class NewsPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public HtmlDto? Text { get; set; }
        public DateTime PostDate { get; set; }

        public NewsPost ToNewsPost()
        {
            return new NewsPost
            {
                Id = Id,
                Title = Title,
                Text = Text.InnerHtml,
                PostDate = PostDate
            };
        }
    }
}