using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IUserService
    {
        Task StartSubscribeAsync(UserDTO userDTO);
        Task StopSubscribeAsync(UserDTO userDTO);
    }
}
