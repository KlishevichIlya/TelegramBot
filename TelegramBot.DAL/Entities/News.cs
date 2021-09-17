using System;

namespace TelegramBot.DAL.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
    }
}
