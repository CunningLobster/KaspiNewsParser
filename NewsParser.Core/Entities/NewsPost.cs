using System.ComponentModel.DataAnnotations;

namespace NewsParser.Core.Entities
{
    /// <summary>
    /// Новостной пост
    /// </summary>
    public class NewsPost
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime PostDate { get; set; }
    }
}