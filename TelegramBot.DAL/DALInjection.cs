using Microsoft.Extensions.DependencyInjection;
using TelegramBot.DAL.UnitOfWork;

namespace TelegramBot.DAL
{
    public static class DALInjection
    {
        public static void Injection(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
