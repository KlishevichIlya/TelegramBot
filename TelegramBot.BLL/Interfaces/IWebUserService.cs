using ServiceResult;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.DTO.Web;

namespace TelegramBot.BLL.Interfaces
{
    public interface IWebUserService
    {
        Task<Result<Response>> LoginAsync(LoginModel user);
        Task<Result<Response>> RegisterAsync(RegisterModel model);
        Task<Result<Response>> RefreshAsync(string refreshToken);
    }
}
