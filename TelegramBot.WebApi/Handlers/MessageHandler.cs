using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Handlers
{
    public class MessageHandler
    {
        private readonly IParser _parser;
        private readonly IUserService _userService;
        private IServiceScopeFactory _scopeFactory;
        private readonly IArticleService _articleService;

        public MessageHandler(IParser parser, IUserService userService, IArticleService articleService, IServiceScopeFactory scopeFactory)
        {
            _parser = parser;
            _userService = userService;
            _articleService = articleService;
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Handler of each event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="_client"></param>
        /// <returns></returns>
        public async Task Handle(object sender, MessageEventArgs e, ITelegramBotClient _client)
        {
            if (e.Message is not { Type: Telegram.Bot.Types.Enums.MessageType.Text } || string.IsNullOrEmpty(e.Message.Text)) return;

            try
            {
                if (e.Message.Text.ToLower().Contains("/lastnews"))
                {
                    await SendArticleAsync(e.Message.Chat.Id, 0, 5, _client);
                }

                if (e.Message.Text.ToLower().Contains("/load"))
                {
                    await OnLoadMoreNewsAsync(e.Message.Chat.Id, 0, 5, _client);
                }

                if (e.Message.Text.ToLower().Contains("/subscribe"))
                {
                    await OnStartSubscribeAsync(e.Message.From.Username, e.Message.From.Id, _client, e.Message.Chat.Id);
                }

                if (e.Message.Text.ToLower().Contains("/unsubscribe"))
                {
                    await OnStopSubscibeAsync(e.Message.From.Username, e.Message.From.Id);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send articles to Telegram and show message for load more articles
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="_client"></param>
        /// <returns></returns>
        private async Task SendArticleAsync(long chatId, int offset, int count, ITelegramBotClient _client)
        {
            var articles = await _parser.MakeHtmlRequest(offset, count);
            foreach (var article in articles)
            {
                var linkButton = KeyboardGoOver("Перейти", (EncodeUrl(article.Href)));
                await _client.SendPhotoAsync(chatId: chatId, photo: article.Image,
                    caption: $"*{article.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: linkButton);

            }
            await OnLoadMoreNewsAsync(chatId, offset + count, count, _client);
        }

        /// <summary>
        /// Encode url without domain 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string EncodeUrl(string uri)
        {
            var objectUri = new Uri(uri);
            var staticPartOfUri = objectUri.Scheme + "://" + objectUri.Host + objectUri.Segments[0] + objectUri.Segments[1] + objectUri.Segments[2];
            var encodePartOfQuery = WebUtility.UrlEncode(objectUri.Segments[3]);
            return staticPartOfUri + encodePartOfQuery;
        }

        /// <summary>
        /// Inline keyboard for loading new articles
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="_client"></param>
        /// <returns></returns>
        private static async Task OnLoadMoreNewsAsync(long chatId, int offset, int count, ITelegramBotClient _client)
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

        /// <summary>
        /// Subscribe user to automatically update news
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task OnStartSubscribeAsync(string userName, long userId, ITelegramBotClient _client, long chatId)
        {
            var user = new UserDTO
            {
                UserId = userId.ToString(),
                UserName = userName
            };

            var temp = Task.Run(() =>
            {
                _userService.StartSubscribeAsync(user);
            }).ContinueWith(x => _client.SendTextMessageAsync(chatId, "Вы успешно подписались на рассылку!"));

            var articles = await ReturnNewArticles();
            foreach (var item in articles)
            {
                var linkButton = KeyboardGoOver("Перейти", (EncodeUrl(item.Href)));
                await _client.SendPhotoAsync(chatId: chatId, photo: item.Image,
                    caption: $"*{item.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: linkButton);
            }
        }

        /// <summary>
        /// Unsubscribe user from automatically update news
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task OnStopSubscibeAsync(string userName, long userId)
        {
            var user = new UserDTO()
            {
                UserId = userId.ToString(),
                UserName = userName
            };
            await _userService.StopSubscribeAsync(user);
        }

        /// <summary>
        /// Inline keyboard for show link behind each article
        /// </summary>
        /// <param name="text"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static InlineKeyboardMarkup KeyboardGoOver(string text, string url)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithUrl(text, url)
                },
            });
        }

        /// <summary>
        /// Handler for callback. Load a new five articles.
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="_client"></param>
        /// <returns></returns>
        public async Task CallBack(CallbackQueryEventArgs ev, ITelegramBotClient _client)
        {
            var (offset, count) = GetOffsetAndCountFromString(ev.CallbackQuery.Data);
            await _client.DeleteMessageAsync(ev.CallbackQuery.Message.Chat.Id, ev.CallbackQuery.Message.MessageId);
            await SendArticleAsync(ev.CallbackQuery.Message.Chat.Id, offset, count, _client);
        }

        /// <summary>
        /// Get offset to 5 from string 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static (int, int) GetOffsetAndCountFromString(string str)
        {
            var numbers = Regex.Matches(str, @"\d+")
                .Cast<Match>()
                .Select(m => Convert.ToInt32(m.Value))
                .ToList();
            return (numbers[0], numbers[1]);
        }

        private Task GetNewArticles()
        {
            var timer = new Timer(async (o) => await ReturnNewArticles(), null, TimeSpan.Zero, TimeSpan.FromSeconds(49));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Find new articles and return them 
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<NewsDTO>> ReturnNewArticles()
        {
            using var scope = _scopeFactory.CreateScope();
            var articlesRequest = await _parser.MakeRequestWithoutSaving();
            var articlesFromDb = await _articleService.GetLasFiveNewsAsync();
            return articlesRequest;
            //return articlesFromDb is not null ? articlesFromDb.Except(articlesRequest) : articlesRequest;
        }
    }
}