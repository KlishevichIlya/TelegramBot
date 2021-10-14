using Microsoft.Extensions.DependencyInjection;
using TelegramBot.BLL.Interfaces;
using TelegramBot.BLL.Mapping;
using TelegramBot.BLL.Services;
using TelegramBot.DAL;

namespace TelegramBot.BLL
{
    public static class BLLInjection
    {
        public static void Injection(IServiceCollection services)
        {
            DALInjection.Injection(services);
            services.AddScoped<IParser, ParserService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddAutoMapper(typeof(CommonMappingProfile));
        }
    }
}
