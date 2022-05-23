using Microsoft.AspNetCore.Identity;
using System;

namespace TelegramBot.DAL.Entities
{
    public class WebUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
