using DSharpPlus.SlashCommands;
using System.Threading.Tasks;

namespace MainTrade.DiscordBot.Commands
{
    internal class PortfolioCommand : BaseCommand
    {

        [SlashCommand("portfolio", "Generate a portfolio")]
        public Task Command(InteractionContext ctx)
        {
            return InvokeCommand(ctx, new[] { "exchange" });
        }
    }
}
