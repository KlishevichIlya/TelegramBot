using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _userService.GetAllUsers();
            return Ok(customers);
        }
    }
}
