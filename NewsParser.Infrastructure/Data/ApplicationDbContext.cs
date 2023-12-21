using Microsoft.EntityFrameworkCore;
using NewsParser.Core.Entities;

namespace NewsParser.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<NewsPost> NewsPosts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}