using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotLambda
{
    public class CommandService
    {
        private readonly TelegramBotClient _botClient;

        public CommandService()
        {
            _botClient = new TelegramBotClient("2086559527:AAFnxRQKqtNBrRX6hULsHGygMcWr8F2aurg");
        }


        public Task SendAsync(Update update)
        {
            LambdaLogger.Log(update.Message.Text);
            return _botClient.SendTextMessageAsync(update.Message.Chat.Id, update.Message.Text);
        }
    }
}
