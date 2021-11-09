using System.Threading.Tasks;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UserRepository;

namespace TelegramBot.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        INewsRepository News { get; }
        IUserRepository Users { get; }
        Task CompleteAsync();
    }
}
