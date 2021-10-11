using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.BLL.Interfaces;
using TelegramBot.BLL.Services;

namespace TelegramBot.WebApi.Handlers
{
    public class MessageHandler
    {
        
        private readonly IParser _parser;
        public MessageHandler(IParser parser)
        {
            _parser = parser;
        }
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
    }
}
