using CommandLambda;
using CommandLambda.CommandResults;
using DSharpPlus.Entities;

namespace MainTrade.DiscordBot
{
    public class CommandResultParser : BaseCommandResultParser<DiscordWebhookBuilder>
    {
        protected override DiscordWebhookBuilder ParseError(CommandResultError commandResultError)
        {
            return CreateBuilder("");
        }

        protected override DiscordWebhookBuilder ParseList(CommandResultList commandResultList)
        {
            return CreateBuilder("");
        }

        protected override DiscordWebhookBuilder ParseMetrics(CommandResultMetrics metricsResult)
        {
            return CreateBuilder("");
        }

        protected override DiscordWebhookBuilder ParsePortfolio(CommandResultPortfolio portfolioResult)
        {
            return CreateBuilder("");
        }

        protected override DiscordWebhookBuilder ParseText(CommandResultText commandResultText)
        {
            return CreateBuilder("");
        }

        private static DiscordWebhookBuilder CreateBuilder(string content) =>
            new DiscordWebhookBuilder().WithContent(content);
    }
}
