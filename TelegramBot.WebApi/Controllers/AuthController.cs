﻿using Microsoft.AspNetCore.Mvc;
using ServiceResult;
using ServiceResult.ApiExtensions;
using System;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
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


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel user)
        {
            var result = await _webService.Login(user);
            return this.FromResult(result);
        }  
    }
}