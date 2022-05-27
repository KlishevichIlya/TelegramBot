using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Controllers
{
    
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _userService.GetAllUsers();
            return Ok(customers);
        }


        [HttpPatch]
        public async Task<IActionResult> UpdateCustromer(UserDTO newUser)
        {
            var response = await _userService.UpdateUserAsync(newUser);
            return Ok(response);
        }

    }
}
