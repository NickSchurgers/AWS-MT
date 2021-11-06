using CommandLambda;
using CommandLambda.Options;
using MainTrade.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class ExchangeCommand : BaseCommand<ExchangeOptions>
    {
        public override async Task<CommandResult> ProcessAsync(ExchangeOptions options)
        {
            var exchanges = await Context.QueryAsync<Exchange>(PartitionKeys.EXCHANGE).GetRemainingAsync();
            return new CommandResult(CommandResultType.LIST, exchanges.Select(x => x.Sk));
        }
    }
}
