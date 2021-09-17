using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelegramController : Controller
    {
        private static string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        private static TelegramBotClient _client;        
        private static IParser _parser;

        public TelegramController(IParser parser)
        {
             _client = new TelegramBotClient(_token);           
            _parser = parser;
            _client.StartReceiving();
            _client.OnMessage += OnMessageHandler;
            _client.OnCallbackQuery += OnLoadCallBackAsync;

        }

        [HttpGet("/start")]
        public void Start()
        {
           // _client.StartReceiving();
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

        private static async Task OnStartSubscibeAsync(string userName, long userId)
        {

        }


        private static async void OnLoadCallBackAsync(object src, CallbackQueryEventArgs ev)
        {

            if (ev.CallbackQuery.Data.StartsWith("load_"))
            {
                var (offset, count) = GetOffsetAndCountFromString(ev.CallbackQuery.Data);
                await _client.DeleteMessageAsync(ev.CallbackQuery.Message.Chat.Id, ev.CallbackQuery.Message.MessageId);
                await SendArticleAsync(ev.CallbackQuery.Message.Chat.Id, offset, count);
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

        private static async Task SendArticleAsync(long chatId, int offset, int count)
        {
            var articles = await _parser.MakeHtmlRequest(offset, count);
            foreach (var article in articles)
            {
                var linkButton = KeyboardGoOver("Перейти", article.Href);
                await _client.SendPhotoAsync(chatId: chatId, photo: article.Image,
                        caption: $"*{article.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: linkButton);

            }
            await OnLoadMoreNewsAsync(chatId, offset + count, count);
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

        [HttpGet]
        public  static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text ||
                string.IsNullOrEmpty(e.Message.Text)) return;

            try
            {
                if (e.Message.Text.ToLower().Contains("/lastnews"))
                {
                    await SendArticleAsync(e.Message.Chat.Id, 0, 5);
                }

                if (e.Message.Text.ToLower().Contains("/load"))
                {
                    await OnLoadMoreNewsAsync(e.Message.Chat.Id, 0, 5);
                }

                if (e.Message.Text.ToLower().Contains("/subscribe"))
                {
                    await OnStartSubscibeAsync(e.Message.From.Username, e.Message.From.Id);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
