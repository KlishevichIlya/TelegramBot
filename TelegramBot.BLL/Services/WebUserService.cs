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

namespace TelegramBot.BLL.Services
{
    public class WebUserService : IWebUserService
    {
        private readonly UserManager<WebUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public WebUserService(UserManager<WebUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            try
            {
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authCalims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    foreach (var role in userRoles)
                    {
                        authCalims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var token = CreateToken(authCalims);
                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                    await _userManager.UpdateAsync(user);
                    return new SuccessResult<LoginResponse>(new LoginResponse()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    });
                }               
                return new InvalidResult<LoginResponse>("Invalid UserName");                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new UnexpectedResult<LoginResponse>();
            }           
        }

        public async Task<Result<Response>> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);

            if (userExists != null)
                return new InvalidResult<Response>("User already exists!");

            var user = new WebUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new UnexpectedResult<Response>();

            return new SuccessResult<Response>(new Response()
            {
                Status = "Success",
                Message = "User was created successfully!"
            });
        }



        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidInMinutes"], out int tokenValidInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT: ValidIssuer"],
                audience: _configuration["JWT: ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
