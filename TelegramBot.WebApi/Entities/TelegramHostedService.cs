using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.WebApi.Handlers;

namespace TelegramBot.WebApi.Entities
{
    public class TelegramHostedService : IHostedService
    {
        private IServiceScopeFactory _scopeFactory;
        private TelegramBotClient _client;
        private readonly string _token = "1973694233:AAHqgQSqs7lz-TE7n5HVCm5Z692ZRiivQcc";
        public TelegramHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new TelegramBotClient(_token);
            _client.StartReceiving();
            _client.OnMessage += OnMessageHandlerAsync;

            return Task.CompletedTask;
        }

        private async void OnMessageHandlerAsync(object sender, MessageEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<MessageHandler>();
            await handler.Handle(sender, e, _client);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
