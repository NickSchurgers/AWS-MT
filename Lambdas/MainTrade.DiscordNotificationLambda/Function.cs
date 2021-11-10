using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SNSEvents;
using Discord.Webhook;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

// This project specifies the serializer used to convert Lambda event into .NET classes in the project's main 
// main function. This assembly register a serializer for use when the project is being debugged using the
// AWS .NET Mock Lambda Test Tool.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MainTrade.DiscordNotificationLambda
{
    public class Function
    {
        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
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
                using var client = new DiscordWebhookClient("https://discord.com/api/webhooks/907607836501090374/EQjzHTBQ2bx2wXDYL1gt17Xj3MstED1F_axa0Q6XWh1q45sND2JdB7lyNdE5v6Om24h_");

                var records = input.Records.Select(x => x.Sns);
                foreach (var record in records)
                {
                    //record.MessageAttributes.TryGetValue("chat_id", out SNSEvent.MessageAttribute chatIdAttribute);
                    // _ = long.TryParse(chatIdAttribute.Value, out long chatId);
                    await client.SendMessageAsync(record.Message);
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(ex.Message);
            }
        }
    }
}
