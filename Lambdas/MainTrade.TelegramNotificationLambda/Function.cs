using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SNSEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

// This project specifies the serializer used to convert Lambda event into .NET classes in the project's main 
// main function. This assembly register a serializer for use when the project is being debugged using the
// AWS .NET Mock Lambda Test Tool.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MainTrade.TelegramNotificationLambda
{
    public class Function
    {
        private static TelegramBotClient _botClient;

        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            _botClient = new TelegramBotClient("2086559527:AAFnxRQKqtNBrRX6hULsHGygMcWr8F2aurg");

            Func<SNSEvent, ILambdaContext, Task> func = FunctionHandler;
            using(var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer()))
            using(var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task FunctionHandler(SNSEvent input, ILambdaContext context)
        {
            try
            {
                var records = input.Records.Select(x => x.Sns);
                foreach (var record in records)
                {
                    record.MessageAttributes.TryGetValue("chat_id", out SNSEvent.MessageAttribute chatIdAttribute);
                    _ = long.TryParse(chatIdAttribute.Value, out long chatId);
                    await _botClient.SendTextMessageAsync(new ChatId(chatId), record.Message);
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(ex.Message);
            }
        }
    }
}
