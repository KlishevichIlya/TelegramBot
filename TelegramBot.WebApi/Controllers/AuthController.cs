using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.DTO.Web;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IWebUserService _webService;
        public AuthController(IWebUserService webService)
        {
            _webService = webService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel registerModel)
        {
            var result = await _webService.RegisterAsync(registerModel);      
            SetRefreshTokenInCookie(result.Data);
            return this.FromResult(result);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            var result = await _webService.LoginAsync(loginModel);
            SetRefreshTokenInCookie(result.Data);
            return this.FromResult(result);
        }  

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _webService.RefreshAsync(refreshToken);
            SetRefreshTokenInCookie(response.Data);
            return this.FromResult(response);
        }


        private void SetRefreshTokenInCookie(Response response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = response.Expiration
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
        }
    }
}
