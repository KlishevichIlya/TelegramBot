using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceResult.ApiExtensions;
using System;
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
        private readonly IConfiguration _configuration;
        public AuthController(IWebUserService webService, IConfiguration configuration)
        {
            _webService = webService;
            _configuration = configuration;
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
            if(result.Errors.Count == 0)
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
            _ = double.TryParse(_configuration["JWT:RefreshokenValidityInDays"], out double expiresDays);
            var expiresDate = DateTime.Now.AddDays(expiresDays);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = new DateTimeOffset(expiresDate),
            };
            Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
        }
    }
}
