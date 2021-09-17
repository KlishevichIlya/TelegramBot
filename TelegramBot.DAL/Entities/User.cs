using System;

namespace TelegramBot.DAL.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfStartSubscription { get; set; }
    }
}
