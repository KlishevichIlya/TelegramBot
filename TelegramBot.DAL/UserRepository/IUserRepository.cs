using System.Collections.Generic;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.UserRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IEnumerable<User> GetOldestUser(int count);
    }
}
