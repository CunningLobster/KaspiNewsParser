namespace NewsParser.Core.Entities
{
    public class NewsPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime PostDate { get; set; }
    }
}