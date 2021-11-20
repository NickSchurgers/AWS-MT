using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CommandLambda;
using CommandLambda.CommandResults;
using MainTrade.CommandLambda;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Types;

// This project specifies the serializer used to convert Lambda event into .NET classes in the project's main 
// main function. This assembly register a serializer for use when the project is being debugged using the
// AWS .NET Mock Lambda Test Tool.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MainTrade.TelegramBotLambda
{
    public class Function
    {

        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            try
            {
                Func<APIGatewayHttpApiV2ProxyRequest, ILambdaContext, Task<APIGatewayHttpApiV2ProxyResponse>> func = FunctionHandler;
                using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer()))
                using (var bootstrap = new LambdaBootstrap(handlerWrapper))
                {
                    await bootstrap.RunAsync();
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
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
        public static async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
        {
            try
            {
                var updateEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<Update>(input.Body);
                var command = updateEvent.Message.Text.TrimStart('/').Split();

                var client = new AmazonLambdaClient();
                var request = new InvokeRequest { FunctionName = "CommandProcessor", Payload = JsonSerializer.Serialize(command) };
                var response = await client.InvokeAsync(request);

                var snsClient = new AmazonSimpleNotificationServiceClient();
                var snsRequest = new PublishRequest
                {
                    TopicArn = "arn:aws:sns:us-east-1:890196580586:telegram",
                    Message = await new CommandResultParser().ParseAsync(response),
                    MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                        { "chat_id",
                            new MessageAttributeValue { DataType = "String", StringValue = updateEvent.Message.Chat.Id.ToString() }
                        }
                    }
                };
                await snsClient.PublishAsync(snsRequest);

                return new APIGatewayHttpApiV2ProxyResponse { StatusCode = (int)HttpStatusCode.NoContent };
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
                LambdaLogger.Log(e.StackTrace);

                return new APIGatewayHttpApiV2ProxyResponse { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
