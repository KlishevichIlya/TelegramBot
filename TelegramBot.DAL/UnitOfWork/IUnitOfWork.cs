using System.Threading.Tasks;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UserRepository;
using TelegramBot.DAL.WebUserRepository;

namespace TelegramBot.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        INewsRepository News { get; }
        IUserRepository Users { get; }  
        IWebUserRepository WebUsers { get; }
        Task CompleteAsync();
    }
}
