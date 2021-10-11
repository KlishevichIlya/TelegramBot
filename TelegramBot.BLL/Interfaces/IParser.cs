
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IParser
    {
        Task<IEnumerable<NewsDTO>> MakeHtmlRequest(int offset, int count);
    }
}
