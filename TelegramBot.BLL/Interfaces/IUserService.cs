using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.DAL.Entities;

namespace TelegramBot.BLL.Interfaces
{
    public interface IUserService
    {
        Task StartSubscribeAsync(UserDTO userDTO);
        Task StopSubscribeAsync(UserDTO userDTO);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
