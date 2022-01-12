using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IArticleService 
    {
        Task<IEnumerable<NewsDTO>> GetLasFiveNewsAsync();
    }
}
