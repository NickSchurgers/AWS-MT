using DSharpPlus.SlashCommands;
using System;
using System.Threading.Tasks;

namespace MainTrade.DiscordBot.Commands
{
    internal class ExchangeCommand : BaseCommand
    {

        [SlashCommand("exchange", "Retrieve a list of supported exchanges")]
        public Task Command(InteractionContext ctx)
        {
            return InvokeCommand(ctx, new[] { "exchange" });
        }
    }
}
