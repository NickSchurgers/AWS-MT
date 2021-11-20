using CommandLambda;
using CommandLambda.CommandResults;
using MainTrade.Lib.Extensions;
using System.Linq;
using System.Text;

namespace MainTrade.TelegramBotLambda
{
    internal class CommandResultParser : BaseCommandResultParser<string>
    {

        protected override string ParseMetrics(CommandResultMetrics metricsResult)
        {
            var entry = metricsResult.Entry;
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Metrics for \"{entry.Quote}\"</b>:");

            FormatMetrics(entry, ref sb);

            sb.AppendLine($"<i>Listed on: {string.Join(",", entry.Exchanges.Select(x => x.FirstCharToUpper()))}</i>");

            return sb.ToString();
        }

        protected override string ParsePortfolio(CommandResultPortfolio portfolioResult)
        {
            var (pf, exchange) = portfolioResult;
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Generated portfolio for \"{exchange}\"</b>:");

            foreach (var entry in pf)
            {
                FormatMetrics(entry, ref sb);
            }

            return sb.ToString();
        }

        protected override string ParseError(CommandResultError commandResultError)
        {
            var errors = commandResultError.Entries;
            var sb = new StringBuilder();

            sb.AppendLine("<b>One or multiple errors occured:</b>")
                .AppendLine();

            foreach (var entry in errors)
            {
                sb.AppendLine($"- {entry.Message}");
            }

            return sb.ToString();
        }

        protected override string ParseText(CommandResultText commandResultText) =>
            commandResultText.Entry.Text;


        protected override string ParseList(CommandResultList commandResultList)
        {
            var (list, header) = commandResultList;
            var sb = new StringBuilder();

            sb.AppendLine($"<b>{header}:</b>")
                .AppendLine();

            foreach (var entry in list)
            {
                sb.AppendLine($"- {entry.Text}");
            }

            return sb.ToString();
        }

        private static void FormatMetrics(CommandResultMetricsEntryBase entry, ref StringBuilder sb)
        {
            sb.AppendLine();

            if (entry is CommandResultPortfolioEntry pf)
            {
                sb.AppendLine($"<u>{entry.Quote} - {pf.Allocation}%</u>");
            }

            sb.AppendLine($"<i>Risk: {entry.Risk} | Momentum: {entry.Momentum}</i>")
            .AppendLine($"<i>Market cap: {entry.MarketCap} | Market cap rank: {entry.MarketCapRank} | Alt rank: {entry.AltRank}</i>");
        }
    }
}
