using NewsParser.Core.RepositoryContracts;
using NewsParser.Core.ServiceContracts;
using NewsParser.Core.Services;
using NewsParser.Infrastructure.Repositories;

namespace NewsParser.API.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            // Сервисы
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<INewsPostGetterService, NewsPostGetterService>();
            services.AddScoped<ITextAnalizerService, TextAnalizerService>();
            services.AddScoped<INewsPostAdderService, NewsPostAdderService>();
            services.AddScoped<INewsPostHtmlGetterService, HabrNewsPostHtmlGetterService>();
            services.AddScoped<INewsPostDeleterService, NewsPostDeleterService>();

            // Репозитории
            services.AddScoped<INewsPostRepository, NewsPostRepository>();

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}