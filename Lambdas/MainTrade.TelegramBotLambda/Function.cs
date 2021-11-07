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
using System.Net;
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
                var request = new InvokeRequest { FunctionName = "CommandProcessor", Payload = System.Text.Json.JsonSerializer.Serialize(command) };
                var response = await client.InvokeAsync(request);

                var snsClient = new AmazonSimpleNotificationServiceClient();
                var snsRequest = new PublishRequest
                {
                    TopicArn = "arn:aws:sns:us-east-1:890196580586:telegram",
                    Message = await ParseResult(response.Payload),
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

        private static async Task<string> ParseResult(MemoryStream json)
        {
            var jsonRoot = (await JsonDocument.ParseAsync(json)).RootElement;
            var type = (CommandResultType)jsonRoot.GetProperty("Type").GetInt32();
            var data = jsonRoot.GetProperty("Data").GetRawText();

            return type switch
            {
                CommandResultType.METRICS => ParseMetrics(data),
                CommandResultType.PORTFOLIO => ParsePortfolio(data),
                CommandResultType.LIST => ParseList(data),
                CommandResultType.TEXT => ParseText(data),
                CommandResultType.ERROR => ParseError(data),
                _ => throw new ArgumentOutOfRangeException($"Invalid command result type: {type}"),
            };
        }

        private static string ParseMetrics(string data)
        {
            var metrics = JsonSerializer.Deserialize<CommandResultMetrics>(data);
            return metrics.Text;
        }

        private static string ParsePortfolio(string data)
        {
            var pf = JsonSerializer.Deserialize<CommandResultPortfolio>(data);
            return pf.Text;
        }

        private static string ParseList(string data)
        {
            var list = JsonSerializer.Deserialize<CommandResultList>(data);
            return "";
        }

        private static string ParseText(string data)
        {
            var text = JsonSerializer.Deserialize<CommandResultText>(data);
            return text.Text;
        }

        private static string ParseError(string data)
        {
            var error = JsonSerializer.Deserialize<CommandResultError>(data);
            return error.Exception.Message;
        }
    }
}
