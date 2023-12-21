using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsParser.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string t_createNewsPostsType = @"
                CREATE TYPE dbo.NewsPostsType AS TABLE
                (
                    Id UNIQUEIDENTIFIER,
                    Title NVARCHAR(MAX),
                    Text NVARCHAR(MAX),
                    PostDate DATETIME
                );
            ";
            migrationBuilder.Sql(t_createNewsPostsType);

            string sp_addNewsPosts = @"
                CREATE PROCEDURE dbo.AddNewsPosts
                    @NewsPosts dbo.NewsPostsType READONLY
                AS
                BEGIN
                    INSERT INTO dbo.NewsPosts (Id, Title, Text, PostDate)
                    SELECT Id, Title, Text, PostDate
                    FROM @NewsPosts;
                END;
            ";
            migrationBuilder.Sql(sp_addNewsPosts);

            string sp_deleteAllNewsPosts = @"
                CREATE PROCEDURE dbo.DeleteAllNewsPosts
                AS
                BEGIN
                    DELETE FROM dbo.NewsPosts
                END;
            ";
            migrationBuilder.Sql(sp_deleteAllNewsPosts);

            string sp_getAllNewsPosts = @"
                CREATE PROCEDURE dbo.GetAllNewsPosts
                AS
                BEGIN
                    SELECT * FROM dbo.NewsPosts
                END;
            ";
            migrationBuilder.Sql(sp_getAllNewsPosts);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string t_createNewsPostsType = @"
                DROP TYPE dbo.NewsPostsType
            ";
            migrationBuilder.Sql(t_createNewsPostsType);

            string sp_addNewsPosts = @"
                DROP PROCEDURE dbo.AddNewsPosts;
            ";
            migrationBuilder.Sql(sp_addNewsPosts);

            string sp_deleteAllNewsPosts = @"
                DROP PROCEDURE dbo.DeleteAllNewsPosts;
            ";
            migrationBuilder.Sql(sp_deleteAllNewsPosts);

            string sp_getAllNewsPosts = @"
                DROP PROCEDURE dbo.GetAllNewsPosts;
            ";
            migrationBuilder.Sql(sp_getAllNewsPosts);
        }
    }
}
