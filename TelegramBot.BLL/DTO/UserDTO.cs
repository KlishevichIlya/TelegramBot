using System;

namespace TelegramBot.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfStartSubscription { get; set; }
        public bool isUnsubscribe { get; set; }
    }
}
