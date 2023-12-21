using System.Reflection;
using NewsParser.Core.RepositoryContracts;
using NewsParser.Core.ServiceContracts;
using NewsParser.Core.Services;
using NewsParser.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

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
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Habr News Parser",
                    Description = "ASP.NET Core Web API приложение для парсинга новостных статей с портала Хабр.\n Проект выполнен в рамках тестового задания от компании Kaspi.kz. \n Исполнитель - Черепанский Ян Артурович.",
                    Contact = new OpenApiContact
                    {
                        Name = "LinkedIn: Yan Cherepansky",
                        Url = new Uri("https://www.linkedin.com/in/yan-cherepansky-422703263/")
                    }
                });


                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }
    }
}