
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IParser
    {
        //Task<IEnumerable<NewsDTO>> MakeHtmlRequest(int offset, int count);
        IAsyncEnumerable<NewsDTO> MakeHtmlRequest(int offset, int count);
        Task<IEnumerable<NewsDTO>> MakeRequestWithoutSaving();
        Task SaveArticlesAsync(IEnumerable<NewsDTO> articlesDTO);
    }
}
