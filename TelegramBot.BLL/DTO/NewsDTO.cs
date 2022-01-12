
using System;

namespace TelegramBot.BLL.DTO
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string Image { get; set; }
        public DateTime DateOfCreating { get; set; }
    }
}
