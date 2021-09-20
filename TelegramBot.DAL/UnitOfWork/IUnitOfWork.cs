using System;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UserRepository;

namespace TelegramBot.DAL.UnitOfWork
{
    public interface IUnitOfWork //: IDisposable
    {
        INewsRepository News { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}
 