using System;
using System.Text.Json.Serialization;

namespace TelegramBot.BLL.DTO.Web
{
    public class Response
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
