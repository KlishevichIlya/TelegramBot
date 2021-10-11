using System.Collections.Generic;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.NewsRepository
{
    public interface INewsRepository : IGenericRepository<News>
    {
        IEnumerable<News> GetLastFiveNews();
    }
}
