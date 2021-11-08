using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.NewsRepository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<News>> GetLastFiveNewsAsync() =>
            await _context.News.OrderBy(x => x.Date).Take(5).ToListAsync();

    }
}
