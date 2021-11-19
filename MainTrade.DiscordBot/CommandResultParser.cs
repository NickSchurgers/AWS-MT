using CommandLambda.CommandResults;
using DSharpPlus.Entities;
using MainTrade.CommandLambda;
using MainTrade.Lib.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainTrade.DiscordBot
{
    public class CommandResultParser
    {
        public async Task<DiscordWebhookBuilder> ParseResult(MemoryStream payload)
        {
            var jsonRoot = (await JsonDocument.ParseAsync(payload)).RootElement;
            var type = (CommandResultType)jsonRoot.GetProperty("Type").GetInt32();
            var data = jsonRoot.GetProperty("Data").GetRawText();

            return type switch
            {
                CommandResultType.METRICS => ParseMetrics(data),
                CommandResultType.PORTFOLIO => ParsePortfolio(data),
                CommandResultType.LIST => ParseList(data),
                CommandResultType.TEXT => ParseText(data),
                CommandResultType.ERROR => ParseError(data),
                _ => throw new ArgumentOutOfRangeException($"Invalid command result type: {type}"),
            };
        }

        private DiscordWebhookBuilder ParseMetrics(string data)
        {
            var entry = JsonSerializer.Deserialize<CommandResultMetrics>(data).Entry;
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Metrics for \"{entry.Quote}\"</b>:");

            FormatMetrics(entry, ref sb);

            sb.AppendLine($"<i>Listed on: {string.Join(",", entry.Exchanges.Select(x => x.FirstCharToUpper()))}</i>");

            return new DiscordWebhookBuilder()
                .WithContent(sb.ToString());
        }

        private DiscordWebhookBuilder ParsePortfolio(string data)
        {
            var pf = JsonSerializer.Deserialize<CommandResultPortfolio>(data);
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Generated portfolio for \"{pf.Exchange}\"</b>:");

            foreach (var entry in pf.Entries)
            {
                FormatMetrics(entry, ref sb);
            }

            return new DiscordWebhookBuilder()
                .WithContent(sb.ToString());
        }

        private DiscordWebhookBuilder ParseList(string data)
        {
            var list = JsonSerializer.Deserialize<CommandResultList>(data).List;
            var sb = new StringBuilder();

            sb.AppendLine("<b>Supported exchanges:</b>")
                .AppendLine();

            foreach (var entry in list)
            {
                sb.AppendLine($"- {entry.Text}");
            }

            return new DiscordWebhookBuilder()
                .WithContent(sb.ToString());
        }

        private DiscordWebhookBuilder ParseText(string data)
        {
            var entry = JsonSerializer.Deserialize<CommandResultText>(data).Entry;
            return new DiscordWebhookBuilder()
                 .WithContent(entry.Text);
        }

        private DiscordWebhookBuilder ParseError(string data)
        {
            var errors = JsonSerializer.Deserialize<CommandResultError>(data).Entries;
            var sb = new StringBuilder();

            sb.AppendLine("<b>One or multiple errors occured:</b>")
                .AppendLine();

            foreach (var entry in errors)
            {
                sb.AppendLine($"- {entry.Message}");
            }

            return new DiscordWebhookBuilder()
                .WithContent(sb.ToString());
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
