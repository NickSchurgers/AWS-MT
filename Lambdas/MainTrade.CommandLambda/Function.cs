using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using MainTrade.CommandLambda.Commands;
using MainTrade.CommandLambda.Options;
using CommandLine;
using System;
using System.Threading.Tasks;
using CommandLambda.Options;
using System.Collections.Generic;
using CommandLine.Text;
using System.Text;
using System.IO;

// This project specifies the serializer used to convert Lambda event into .NET classes in the project's main 
// main function. This assembly register a serializer for use when the project is being debugged using the
// AWS .NET Mock Lambda Test Tool.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MainTrade.CommandLambda
{
    public class Function
    {
        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Func<string[], ILambdaContext, Task<CommandResult>> func = FunctionHandler;
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer()))
            using (var bootstrap = new LambdaBootstrap(handlerWrapper))
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
        public static async Task<CommandResult> FunctionHandler(string[] input, ILambdaContext context)
        {
            try
            {
                return await Parser.Default.ParseArguments<ExchangeOptions, MetricsOptions, PortfolioOptions>(input)
                    .MapResult(
                        (ExchangeOptions opts) => new ExchangeCommand().ProcessAsync(opts),
                        (MetricsOptions opts) => new MetricsCommand().ProcessAsync(opts),
                        (PortfolioOptions opts) => new PortfolioCommand().ProcessAsync(opts),
                        errs => Task.FromResult<CommandResult>(new("Command not recognized.")));
            }
            catch (Exception ex)
            {
                return new CommandResult(ex.Message);
            }
        }
    }
}
