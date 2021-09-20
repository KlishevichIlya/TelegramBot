using Microsoft.Extensions.DependencyInjection;
using TelegramBot.DAL.GenericRepository;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UnitOfWork;
using TelegramBot.DAL.UserRepository;

namespace TelegramBot.DAL
{
    public static class DALInjection
    {
        public static void Injection(IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository.UserRepository>();
            services.AddTransient<INewsRepository, NewsRepository.NewsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
