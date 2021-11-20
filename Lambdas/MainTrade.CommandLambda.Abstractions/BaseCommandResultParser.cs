using Amazon.Lambda.Model;
using CommandLambda.CommandResults;
using MainTrade.CommandLambda;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandLambda
{
    public abstract class BaseCommandResultParser<T> : ICommandResultParser<T>
    {
        public async Task<T> ParseAsync(InvokeResponse response)
        {
            var jsonRoot = (await JsonDocument.ParseAsync(response.Payload)).RootElement;
            var type = (CommandResultType)jsonRoot.GetProperty("Type").GetInt32();
            var data = jsonRoot.GetProperty("Data").GetRawText();

            return type switch
            {
                CommandResultType.METRICS => ParseMetrics(JsonSerializer.Deserialize<CommandResultMetrics>(data)),
                CommandResultType.PORTFOLIO => ParsePortfolio(JsonSerializer.Deserialize<CommandResultPortfolio>(data)),
                CommandResultType.LIST => ParseList(JsonSerializer.Deserialize<CommandResultList>(data)),
                CommandResultType.TEXT => ParseText(JsonSerializer.Deserialize<CommandResultText>(data)),
                CommandResultType.ERROR => ParseError(JsonSerializer.Deserialize<CommandResultError>(data)),
                _ => throw new ArgumentOutOfRangeException($"Invalid command result type: {type}"),
            };
        }

        protected abstract T ParseMetrics(CommandResultMetrics metricsResult);

        protected abstract T ParsePortfolio(CommandResultPortfolio portfolioResult);

        protected abstract T ParseError(CommandResultError commandResultError);

        protected abstract T ParseText(CommandResultText commandResultText);

        protected abstract T ParseList(CommandResultList commandResultList);
    }
}
