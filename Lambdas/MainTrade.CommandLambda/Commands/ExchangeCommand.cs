using CommandLambda;
using CommandLambda.Options;
using MainTrade.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class ExchangeCommand : BaseCommand<ExchangeOptions>
    {
        public override async Task<CommandResult> ProcessAsync(ExchangeOptions options)
        {
            var exchanges = await Context.ScanAsync<Exchange>(null).GetRemainingAsync();
            return new CommandResult(string.Join(Environment.NewLine, exchanges.Select(x => x.Name)));
        }
    }
}
