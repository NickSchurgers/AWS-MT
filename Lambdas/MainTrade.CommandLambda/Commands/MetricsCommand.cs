using CommandLambda;
using CommandLambda.CommandResults;
using MainTrade.CommandLambda.Options;
using MainTrade.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class MetricsCommand : BaseCommand<MetricsOptions>
    {
        public override async Task<ICommandResult> ProcessAsync(MetricsOptions options)
        {
            var pairs = await Context
                .QueryAsync<Pair>(PartitionKeys.PAIR, Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, new List<object> { options.Pair.Trim() })
                .GetRemainingAsync();

            if(!pairs.Any() || pairs.Count > 1)
            {
                new CommandResult<CommandResultText>(CommandResultType.TEXT, new(new("Pair not found.")));
            }

            var pair = pairs.First();

            return new CommandResult<CommandResultMetrics>(CommandResultType.METRICS, new(new(pair.Sk,
                    pair.MarketCap.GetValueOrDefault(),
                    pair.Risk.GetValueOrDefault(),
                    pair.Momentum.GetValueOrDefault(),
                    pair.AltRank.GetValueOrDefault(),
                    pair.MarketCapRank.GetValueOrDefault(), 
                    pair.Exchanges)));
        }
    }
}
