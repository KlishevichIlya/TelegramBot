using System.Threading.Tasks;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.NewsRepository;
using TelegramBot.DAL.UserRepository;


namespace TelegramBot.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository Users { get; }
        public INewsRepository News { get; }
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Users = new UserRepository.UserRepository(_context);
            News = new NewsRepository.NewsRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    }
}
