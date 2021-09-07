using System;
using ParserAutoRun.Controllers;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Diagnostics;
using TelegramBot.Entities;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace TelegramBot
{
    public class Program
    {
        private static string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        private static TelegramBotClient _client;
        private static NewsController _novostiController;
        private static readonly int offset = 0;
        private static readonly int count = 5;
        
        static void Main(string[] args)
        {
            _client = new TelegramBotClient(_token);
            _novostiController = new NewsController();            
            _client.StartReceiving();
          
            
            _client.OnMessage += OnMessageHandler;
            _client.OnCallbackQuery += OnLoadCallBackAsync;
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

        private static async Task OnLoadMoreNewsAsync(long chatId, int offset, int count)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Загрузить", $"load_{offset}_{count}")
                    },

                });           
           
            await _client.SendTextMessageAsync(chatId, "Хотите ли загрузить еще 5 новостей?", replyMarkup: keyboard);
        }

        private static async void OnLoadCallBackAsync(object src, CallbackQueryEventArgs ev)
        {            
            
             if (ev.CallbackQuery.Data.StartsWith("load_"))
            {               
                var (offset, count) = GetOffsetAndCountFromString(ev.CallbackQuery.Data);
                await SentArticle(ev.CallbackQuery.Message.Chat.Id, offset, count);
                await OnLoadMoreNewsAsync(ev.CallbackQuery.Message.Chat.Id, offset, count);
            }            
        }
        private static (int, int) GetOffsetAndCountFromString(string str)
        {
            var numbers = Regex.Matches(str, @"\d+")
                         .Cast<Match>()
                         .Select(m => Convert.ToInt32(m.Value))
                         .ToList();
            return (numbers[0], numbers[1]);
        
        }

        private static async Task SentArticle(long chatId, int offset, int count)
        {
            var articles = await _novostiController.GetHtmlRequest(offset, count);
            foreach (var article in articles)
            {
                var linkButton = KeyboardGoOver("Перейти", article.Href);
                await _client.SendPhotoAsync(chatId: chatId, photo: article.Image,
                        caption: $"*{article.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: linkButton);
            }
        }

        public static InlineKeyboardMarkup KeyboardGoOver(string text, string url)
        {
            return new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl(text, url)
                    },

                });
        }


        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text ||
                string.IsNullOrEmpty(e.Message.Text)) return;

            try
            {
                #region
                if (e.Message.Text.ToLower().Contains("/lastnews"))
                {
                    await SentArticle(e.Message.Chat.Id, 0, 5);
                    await OnLoadMoreNewsAsync(e.Message.Chat.Id, 0, 5);

                }
                #endregion

                if (e.Message.Text.ToLower().Contains("/start") || e.Message.Text.Contains("/menu"))                                        
                {
                    BotOnMessageRecieved(sender, e);
                }
                if (e.Message.Text.ToLower().Contains("/load"))
                {
                    OnLoadMoreNewsAsync(e.Message.Chat.Id,0, 5);
                }
               

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
