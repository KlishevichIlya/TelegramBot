using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IUserService
    {
        Task StartSubscribeAsync(UserDTO userDTO);
        Task StopSubscribeAsync(UserDTO userDTO);
        Task<IEnumerable<UserDTO>> GetAllUsers();
         
        Task<IEnumerable<UserDTO>> UpdateUserAsync(UserDTO updatedUser);
    }
}
