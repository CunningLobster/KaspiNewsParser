using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NewsParser.Core.Entities;
using NewsParser.Core.RepositoryContracts;
using NewsParser.Infrastructure.Helpers;

namespace NewsParser.Infrastructure.Repositories
{
    public class NewsPostRepository : INewsPostRepository
    {
        private readonly IConfiguration _configuration;

        public NewsPostRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<NewsPost>?> AddNewsPosts(List<NewsPost> newsPosts)
        {
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                dbConnection.Open();

                DataTable dataTable = DataCoverter.ConvertToDataTable(newsPosts);
                var parameters = new { NewsPosts = dataTable.AsTableValuedParameter("dbo.NewsPostsType") };

                await dbConnection.ExecuteAsync(
                    "dbo.AddNewsPosts",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }

            return newsPosts;
        }

        public async Task<bool> DeleteAllNewsPosts()
        {
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                dbConnection.Open();

                await dbConnection.ExecuteAsync(
                    "dbo.DeleteAllNewsPosts",
                    commandType: CommandType.StoredProcedure
                );
            }

            return true;
        }

        public async Task<List<NewsPost>?> GetAllNewsPosts()
        {
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                dbConnection.Open();

                var newsPosts = await dbConnection.QueryAsync<NewsPost>(
                    "dbo.GetAllNewsPosts",
                    CommandType.StoredProcedure
                );

                return newsPosts.ToList();
            }
        }
    }
}