using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.WebUserRepository
{
    public class WebUserRepository: GenericRepository<WebUser>, IWebUserRepository
    {
        public WebUserRepository(ApplicationContext context): base(context)
        {
        }

        public async Task<WebUser> SingleOrDefaultAsync(Expression<Func<WebUser, bool>> expression)
        {
            await _context.WebUsers.Where(expression)
        }
    }
}
