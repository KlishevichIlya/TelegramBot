
namespace TelegramBot.DAL.Entities
{
    public class Editor
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isAdminRule { get; set; }
    }
}
