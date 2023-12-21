using NewsParser.Core.DTO;
using NewsParser.Core.ServiceContracts;

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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}