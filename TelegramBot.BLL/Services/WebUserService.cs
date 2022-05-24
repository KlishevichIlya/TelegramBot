using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceResult;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.DTO.Web;
using TelegramBot.BLL.Interfaces;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.UnitOfWork;

namespace TelegramBot.BLL.Services
{
    public class WebUserService : IWebUserService
    {
        private readonly IUnitOfWork _context;
        public WebUserService(IUnitOfWork context) 
        {
            _context = context;
        }

        public async Task Login(LoginModel model)
        {
           
        }

        public async Task Register(RegisterModel model)
        {
           var targetUser = await _context.WebUsers
        }

    }
}
