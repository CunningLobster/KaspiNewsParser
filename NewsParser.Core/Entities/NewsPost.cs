using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsParser.Core.Entities
{
    /// <summary>
    /// Новостной пост
    /// </summary>
    public class NewsPost
    {
        [Key]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime PostDate { get; set; }
    }
}