
using System.Collections.Generic;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IArticleService
    {
        IEnumerable<NewsDTO> GetLasFiveNews();
    }
}
