using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Service.Handlers;

namespace TelegramBot.Service.Entities
{
    public class TelegramHostedService : IHostedService
    {
        private IServiceScopeFactory _scopeFactory;
        private TelegramBotClient _client;
        private readonly IConfiguration _config;
        public TelegramHostedService(IServiceScopeFactory scopeFactory, IConfiguration config)
        {
            _scopeFactory = scopeFactory;
            _config = config;
        }

        /// <summary>
        /// Starting Hosted Service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new TelegramBotClient(_config.GetValue<string>("Token"));
            _client.StartReceiving();
            _client.OnMessage += OnMessageHandlerAsync;
            _client.OnCallbackQuery += OnLoadCallBackAsync;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handler for button "Load more news"
        /// </summary>
        /// <param name="src"></param>
        /// <param name="ev"></param>
        private async void OnLoadCallBackAsync(object src, CallbackQueryEventArgs ev)
        {
            if (ev.CallbackQuery.Data.StartsWith("load_"))
            {
                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<MessageHandler>();
                await handler.CallBack(ev, _client);
            }
        }

        /// <summary>
        /// Handle action from Telegram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnMessageHandlerAsync(object sender, MessageEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<MessageHandler>();
            await handler.Handle(sender, e, _client);
        }

        /// <summary>
        /// Stop Hosted service and stop receiving message from Telegram
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.StopReceiving();
            return Task.CompletedTask;
        }
    }
}
