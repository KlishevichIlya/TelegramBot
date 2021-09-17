using System.Collections.Generic;
using System.Linq;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {}

        public IEnumerable<User> GetOldestUser(int count) => _context.Users.OrderByDescending(x => x.DateOfStartSubscription).Take(count).ToList();
    }
}
