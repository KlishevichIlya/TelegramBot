using Microsoft.AspNetCore.Http;
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
        private readonly IConfiguration _configuration;
        public WebUserService(IUnitOfWork context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<Result<Response>> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _context.WebUsers.SingleOrDefaultAsync(x => x.UserName == model.UserName);               
                if (user == null)
                    return new InvalidResult<Response>($"We don't have a user with {model.UserName} UserName yet!");
                if(!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    return new InvalidResult<Response>($"Wrong password!");
                var token = GenerateToken(user);
                var refreshToken = GenerateRefreshToken();
                _ = double.TryParse(_configuration["JWT:RefreshokenValidityInDays"], out double expiresDays);
                user.ExpirationDate =DateTime.Now.AddDays(expiresDays);
                user.RefreshToken = refreshToken;
                await _context.CompleteAsync();
                return new SuccessResult<Response>(new Response()
                {
                    Email = user.Email,
                    Expiration = user.ExpirationDate,
                    RefreshToken = user.RefreshToken,
                    Token = token,
                    UserName = user.UserName,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<Response>();
            }
        }   

        public async Task<Result<Response>> RegisterAsync(RegisterModel model)
        {
            try
            {
                var targetUser = await _context.WebUsers.SingleOrDefaultAsync(x => x.Email == model.Email || x.UserName == model.UserName);
                if (targetUser != null)
                    return new InvalidResult<Response>("Invalid data! Users with the same data already exsist!");
                var newUser = new WebUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    UserName = model.UserName,
                    IsAdmin = model.IsAdmin,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),                   
                };
                var token = GenerateToken(newUser);
                var refreshToken = GenerateRefreshToken();
                newUser.RefreshToken = refreshToken;                
                _ = double.TryParse(_configuration["JWT:RefreshokenValidityInDays"], out double expiresDays);
                newUser.ExpirationDate = DateTime.Now.AddDays(expiresDays);
                await _context.WebUsers.AddAsync(newUser);
                await _context.CompleteAsync();
                return new SuccessResult<Response>(new Response()
                {
                    Email = newUser.Email,
                    Expiration = newUser.ExpirationDate,
                    RefreshToken = newUser.RefreshToken,
                    Token = token,
                    UserName = newUser.UserName,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<Response>();
            }
        }

        public async Task<Result<Response>> RefreshAsync(string refreshToken)
        {
            var user = await _context.WebUsers.FIrstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if(user == null)
                return new InvalidResult<Response>("Invalid refresh token!");
            
            if(user.ExpirationDate <= DateTime.Now)
                return new InvalidResult<Response>("RefreshToken already expires!");
            var newToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _ = double.TryParse(_configuration["JWT:RefreshokenValidityInDays"], out double expiresDays);
            user.ExpirationDate = DateTime.Now.AddDays(expiresDays);
            await _context.CompleteAsync();
           
            return new SuccessResult<Response>(new Response()
            {
                Email = user.Email,
                Expiration = user.ExpirationDate,
                RefreshToken = user.RefreshToken,
                Token = newToken,
                UserName = user.UserName,
            });
        }

        private string GenerateToken(WebUser webUser)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var issuer = _configuration["JWT:ValidIssuer"];
            var audience = _configuration["JWT:ValidAudience"];
            _ = double.TryParse(_configuration["JWT:TokenValidInMinutes"], out double expiresMinutes);
            var payload = new JwtPayload(issuer, audience, new List<Claim>
            {
                new Claim(ClaimTypes.Name,webUser.UserName),
                new Claim("Id", webUser.Id),
                new Claim(ClaimTypes.Role, webUser.IsAdmin ? "Admin" : "User")
            }, null, DateTime.Now.AddMinutes(expiresMinutes));
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private string GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
} 
