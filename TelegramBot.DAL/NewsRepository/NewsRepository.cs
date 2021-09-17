using System.Collections.Generic;
using System.Linq;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.NewsRepository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationContext context): base(context)
        {           
        }

        public IEnumerable<News> GetLastFiveNews() => _context.News.OrderBy(x => x.Date).Take(5);
    }
}
