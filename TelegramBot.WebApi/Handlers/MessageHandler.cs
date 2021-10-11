using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Handlers
{
    public class MessageHandler
    {
        private readonly IParser _parser;
        public MessageHandler(IParser parser)
        {
            _parser = parser;
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
                    await OnStartSubscibeAsync(e.Message.From.Username, e.Message.From.Id);
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
                var linkButton = KeyboardGoOver("Перейти", article.Href);
                await _client.SendPhotoAsync(chatId: chatId, photo: article.Image,
                    caption: $"*{article.Title}*", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: linkButton);

            }
            await OnLoadMoreNewsAsync(chatId, offset + count, count, _client);
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

        private static async Task OnStartSubscibeAsync(string userName, long userId)
        {

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
    }
}
