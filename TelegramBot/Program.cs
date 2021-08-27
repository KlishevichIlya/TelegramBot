using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    public class Program
    {
        private static string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        private static TelegramBotClient _client;

        
        static void Main(string[] args)
        {
            _client = new TelegramBotClient(_token);
            _client.StartReceiving();
            _client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            _client.StopReceiving();
        }

        private static void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if(e.Message != null && e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text && !string.IsNullOrEmpty(e.Message.Text))
            {
                try
                {
                    if(e.Message.Text.ToLower().Contains("hi"))
                    {
                        _client.SendTextMessageAsync(e.Message.Chat.Id, "Proverka");
                    }
                    
                }
                catch(Exception ex)
                {

                }
            }            
        }
    }
}
