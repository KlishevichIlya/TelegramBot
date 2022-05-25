using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.WebUserRepository
{
    public interface IWebUserRepository: IGenericRepository<WebUser>
    {
        Task<WebUser> SingleOrDefaultAsync(Expression<Func<WebUser, bool>> expression);

        Task<WebUser> FIrstOrDefaultAsync(Expression<Func<WebUser, bool>> expression);
    }
}
