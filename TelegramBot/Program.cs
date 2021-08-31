using System;
using ParserAutoRun.Controllers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public class Program
    {
        private static string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        private static TelegramBotClient _client;
        private static NewsController _novostiController;

        
        static void Main(string[] args)
        {
            _client = new TelegramBotClient(_token);
            _novostiController = new NewsController();            
            _client.StartReceiving();
          
            
            _client.OnMessage += OnMessageHandler;
            
            Console.ReadLine();
            _client.StopReceiving();
        }


        private static async void BotOnMessageRecieved(object sender, MessageEventArgs e)
        {
            var keyboard = new ReplyKeyboardMarkup();
            keyboard.Keyboard =
                        new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("История"),
                    new KeyboardButton("Подписка")
                },

                new KeyboardButton[]
                {
                    new KeyboardButton("Перейти на сайт")
                },

                new KeyboardButton[]
                {
                    new KeyboardButton("3-1"),
                    new KeyboardButton("3-2"),
                    new KeyboardButton("3-3")
                }
            };
                       
            await _client.SendTextMessageAsync(e.Message.Chat.Id,"Выберите неогбходимую функцию", replyMarkup: keyboard);
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text ||
                string.IsNullOrEmpty(e.Message.Text)) return;

            try
            {
                if (e.Message.Text.ToLower().Contains("история"))
                {
                    var countOfPage = await _novostiController.GetLastPage();
                    for (var currentPage = 1; currentPage <= countOfPage; currentPage++)
                    {
                        var articles = await _novostiController.GetHtmlRequest(currentPage);
                        foreach (var article in articles)
                        {
                            var keyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithUrl("Перейти", $"{article.Href}")
                                },
                               
                            });

                            await _client.SendPhotoAsync(chatId: e.Message.Chat.Id, photo: article.Image,
                                caption: $"*{article.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboard);

                        }
                    }

                }
                if (e.Message.Text.ToLower().Contains("/start") || e.Message.Text.Contains("/menu"))
                {
                    BotOnMessageRecieved(sender, e);
                }
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
