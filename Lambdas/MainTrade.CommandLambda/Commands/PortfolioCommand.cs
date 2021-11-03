using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MainTrade.CommandLambda.Options;
using MainTrade.Data;
using System;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class PortfolioCommand : ICommand<PortfolioOptions>
    {

        public Task<CommandResult> ProcessAsync(PortfolioOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
