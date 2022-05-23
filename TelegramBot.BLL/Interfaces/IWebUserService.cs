using ServiceResult;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.DTO.Web;

namespace TelegramBot.BLL.Interfaces
{
    public interface IWebUserService
    {
        Task<Result<LoginResponse>> Login(LoginModel user);
        Task<Result<Response>> Register(RegisterModel model);
    }
}
