using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.NewsRepository
{
    public interface INewsRepository : IGenericRepository<News>
    {
        Task<IEnumerable<News>> GetLastFiveNewsAsync();
    }
}
