using Microsoft.AspNetCore.Identity;
using System;

namespace TelegramBot.DAL.Entities
{
    public class WebUser 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } 
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
