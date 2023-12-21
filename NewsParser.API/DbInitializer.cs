using Microsoft.EntityFrameworkCore;
using NewsParser.Core.DTO;
using NewsParser.Core.ServiceContracts;
using NewsParser.Infrastructure.Data;

namespace NewsParser.API
{
    public static class DbInitializer
    {
        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        /// <param name="serviceProvider">Сервисы</param>
        /// <param name="newsUrl">URL адрес, с которого нужно спарсить новости</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Новостные посты не найдены</exception>
        public async static Task InitDb(IServiceProvider serviceProvider, string newsUrl)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                await MigrateDb(services);
                await FillNewsPostTable(newsUrl, services);
            }
        }

        private static async Task FillNewsPostTable(string newsUrl, IServiceProvider services)
        {
            var newsSpostDeleterService = services.GetRequiredService<INewsPostDeleterService>();
            //Очистить таблицу при каждом запуске приложения
            await newsSpostDeleterService.DeleteAllNewsPosts();

            try
            {
                //Получить ответ от сервера с новостями
                var httpService = services.GetRequiredService<IHttpService>();
                string? url = newsUrl;
                string response = await httpService.GetHttpResponse(url);

                //Парсинг новостных статей
                var html = new HtmlDto(response);
                var newsPostAdderService = services.GetRequiredService<INewsPostAdderService>();
                await newsPostAdderService.AddNewsPostsFromHtml(html);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred filling NewsPostTable.");
            }
        }

        private static async Task MigrateDb(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

    }
}