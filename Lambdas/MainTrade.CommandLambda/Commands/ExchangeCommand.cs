using CommandLambda;
using CommandLambda.CommandResults;
using CommandLambda.Options;
using MainTrade.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class ExchangeCommand : BaseCommand<ExchangeOptions>
    {
        public override async Task<ICommandResult> ProcessAsync(ExchangeOptions options)
        {
            var exchanges = await Context.QueryAsync<Exchange>(PartitionKeys.EXCHANGE).GetRemainingAsync();
            return new CommandResult<CommandResultList>(CommandResultType.LIST, new CommandResultList(exchanges.Select(x => x.Sk)));
        }
    }
}
