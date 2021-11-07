using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using CommandLambda;
using CommandLambda.CommandResults;
using MainTrade.CommandLambda.Options;
using MainTrade.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class PortfolioCommand : BaseCommand<PortfolioOptions>
    {

        public override async Task<ICommandResult> ProcessAsync(PortfolioOptions options)
        {
            IEnumerable<Pair> pairs = await GetPairs(options);
            pairs = Sort(options, pairs)
                .Take(options.Amount);

            var text = string.Join(Environment.NewLine, pairs.Select(x => $"Coin: {x.Quote} \\ Market cap: {x.MarketCap} \\ Risk: {x.Risk} \\ AltRank: {x.AltRank}"));

            return new CommandResult<CommandResultPortfolio>(CommandResultType.PORTFOLIO, new CommandResultPortfolio(text));
        }

        private Task<List<Pair>> GetPairs(PortfolioOptions options)
        {
            var conditions = new List<ScanCondition> {
                new ScanCondition(nameof(Pair.Exchanges), ScanOperator.Contains, options.Exchange.Trim()),
                new ScanCondition(nameof(Pair.Base), ScanOperator.Equal, options.Base.Trim()),
                new ScanCondition(nameof(Pair.Risk), ScanOperator.LessThanOrEqual, options.Risk),
            };

            if (options.MarketCapMin.HasValue)
            {
                conditions.Add(new ScanCondition(
                    nameof(Pair.MarketCap),
                    ScanOperator.GreaterThanOrEqual,
                    options.MarketCapMin.Value));
            }

            if (options.MarketCapMax.HasValue)
            {
                conditions.Add(new ScanCondition(
                    nameof(Pair.MarketCap),
                    ScanOperator.LessThanOrEqual,
                    options.MarketCapMax.Value));
            }

            if (options.MarketCapRankMin.HasValue)
            {
                conditions.Add(new ScanCondition(
                    nameof(Pair.MarketCapRank),
                    ScanOperator.GreaterThanOrEqual,
                    options.MarketCapRankMin.Value));
            }

            if (options.MarketCapRankMax.HasValue)
            {
                conditions.Add(new ScanCondition(
                    nameof(Pair.MarketCapRank),
                    ScanOperator.LessThanOrEqual,
                    options.MarketCapRankMax.Value));
            }

            var dynamoConfig = new DynamoDBOperationConfig
            {
                QueryFilter = conditions,
            };

            return Context
                .QueryAsync<Pair>(PartitionKeys.PAIR, dynamoConfig)
                .GetRemainingAsync();
        }

        private static IEnumerable<Pair> Sort(PortfolioOptions options, IEnumerable<Pair> pairs)
        {
            Func<Pair, double> sortBy = options.Sort.Trim() switch
            {
                "momentum" => (Pair x) => x.Momentum.GetValueOrDefault(),
                "risk" => (Pair x) => x.Risk.GetValueOrDefault(),
                "alt_rank" => (Pair x) => x.AltRank.GetValueOrDefault(),
                _ => throw new ArgumentOutOfRangeException()
            };

            return string.Equals(options.SortDirection.Trim(), "asc")
                ? pairs.OrderBy(x => sortBy(x)).ToList()
                : pairs.OrderByDescending(x => sortBy(x)).ToList();
        }
    }
}
