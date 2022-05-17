using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        { }

        public IEnumerable<User> GetOldestUser(int count) => _context.Users.OrderByDescending(x => x.DateOfStartSubscription).Take(count).ToList();

        public async Task<IEnumerable<User>> UpdateUser(User user)
        {
            var targetUset = _context.Users.FirstOrDefault(x => x.UserId == user.UserId);
            if(targetUset != null)
            {
                targetUset.UserName = user.UserName;
                targetUset.DateOfStartSubscription = user.DateOfStartSubscription;
                targetUset.isUnsubscribe = user.isUnsubscribe;
            }            
            await _context.SaveChangesAsync();
            return _context.Users;
        }
    }
}
