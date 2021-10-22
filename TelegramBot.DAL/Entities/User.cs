using System;

namespace TelegramBot.DAL.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfStartSubscription { get; set; }
        public bool isUnsubscribe { get; set; }
    }
}
