using System;
using ParserAutoRun.Controllers;
using Telegram.Bot;
using Telegram.Bot.Args;


namespace TelegramBot
{
    public class Program
    {
        private static string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        private static TelegramBotClient _client;
        private static NovostiController _novostiController;

        
        static void Main(string[] args)
        {
            _client = new TelegramBotClient(_token);
            _novostiController = new NovostiController();
            _client.StartReceiving();
            _client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            _client.StopReceiving();
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text ||
                string.IsNullOrEmpty(e.Message.Text)) return;
            try
            {
                if (!e.Message.Text.ToLower().Contains("/work")) return;
                
                var countOfPage = await _novostiController.GetLastPage();
                for (var currentPage = 1; currentPage <= countOfPage; currentPage++)
                {
                    var articles = await _novostiController.GetHtmlRequest(currentPage);
                    foreach (var article in articles)
                    {
                        await _client.SendTextMessageAsync(e.Message.Chat.Id, article.Href +
                                                                              "\n" + article.Title);
                    }
                }
                


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
