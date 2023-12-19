using NewsParser.Core.ServiceContracts;
using NewsParser.Core.Services;

namespace NewsParser.API.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            // Add services to the container.
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<INewsPostGetterService, HabrNewsPostGetterService>();
            services.AddScoped<ITextAnalizerService, TextAnalizerService>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}