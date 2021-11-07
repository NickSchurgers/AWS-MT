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
using System.Linq;
using CommandLambda.CommandResults;
using CommandLambda;

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
            Func<string[], ILambdaContext, Task<ICommandResult>> func = FunctionHandler;
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
        public static async Task<ICommandResult> FunctionHandler(string[] input, ILambdaContext context)
        {
            try
            {
                var result = new Parser(c => c.HelpWriter = null).ParseArguments<ExchangeOptions, MetricsOptions, PortfolioOptions>(input);
                return await result.MapResult(
                        (ExchangeOptions opts) => new ExchangeCommand().ProcessAsync(opts),
                        (MetricsOptions opts) => new MetricsCommand().ProcessAsync(opts),
                        (PortfolioOptions opts) => new PortfolioCommand().ProcessAsync(opts),
                        errs => Task.FromResult(ParseErrors(result, errs)));
            }
            catch (Exception ex)
            {
                return new CommandResult<CommandResultError>(CommandResultType.ERROR, new CommandResultError(ex));
            }
        }

        private static ICommandResult ParseErrors(ParserResult<object> result, IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                switch (error.Tag)
                {

                    case ErrorType.HelpRequestedError:
                    case ErrorType.HelpVerbRequestedError:
                        return new CommandResult<CommandResultText>(CommandResultType.TEXT, new CommandResultText(HelpText.AutoBuild(result).ToString()));

                    case ErrorType.BadFormatTokenError:
                    case ErrorType.MissingValueOptionError:
                    case ErrorType.UnknownOptionError:
                    case ErrorType.MissingRequiredOptionError:
                    case ErrorType.MutuallyExclusiveSetError:
                    case ErrorType.BadFormatConversionError:
                    case ErrorType.SequenceOutOfRangeError:
                    case ErrorType.RepeatedOptionError:
                    case ErrorType.NoVerbSelectedError:
                    case ErrorType.BadVerbSelectedError:
                    case ErrorType.VersionRequestedError:
                    case ErrorType.SetValueExceptionError:
                    case ErrorType.InvalidAttributeConfigurationError:
                    case ErrorType.MissingGroupOptionError:
                    case ErrorType.GroupOptionAmbiguityError:
                    case ErrorType.MultipleDefaultVerbsError:
                    default:
                        break;
                }
            }

            var builder = SentenceBuilder.Create();
            var errorMessages = HelpText.RenderParsingErrorsTextAsLines(result, builder.FormatError, builder.FormatMutuallyExclusiveSetErrors, 1);
            var excList = errorMessages.Select(msg => new ArgumentException(msg)).ToList();

            if (excList.Any())
            {
                return new CommandResult<CommandResultError>(CommandResultType.ERROR, new CommandResultError(new AggregateException(excList)));
            }

            return new CommandResult<CommandResultError>(CommandResultType.ERROR, new CommandResultError(new ArgumentException("Error processing command.")));
        }
    }
}
