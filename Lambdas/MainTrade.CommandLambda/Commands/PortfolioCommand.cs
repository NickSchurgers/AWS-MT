using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using CommandLambda;
using MainTrade.CommandLambda.Options;
using MainTrade.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class PortfolioCommand : BaseCommand<PortfolioOptions>
    {

        public override async Task<CommandResult> ProcessAsync(PortfolioOptions options)
        {
            var conditions = new List<ScanCondition> {
                new ScanCondition(nameof(Pair.Exchanges), ScanOperator.Contains, options.Exchange.Trim()),
            };
            var dynamoConfig = new DynamoDBOperationConfig
            {
                QueryFilter = conditions
            };

            var coins = await Context.QueryAsync<Pair>(PartitionKeys.PAIR, dynamoConfig).GetRemainingAsync();
            var text = string.Join(Environment.NewLine, coins.Select(x => x.Sk));

            return new CommandResult(text);
        }
    }
}
