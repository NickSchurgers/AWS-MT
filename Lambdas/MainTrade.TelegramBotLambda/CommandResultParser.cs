using Amazon.Lambda.Core;
using CommandLambda.CommandResults;
using MainTrade.CommandLambda;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MainTrade.Lib.Extensions;
using System.Linq;

namespace MainTrade.TelegramBotLambda
{
    internal static class CommandResultParser
    {
        public static async Task<string> ParseResult(MemoryStream payload)
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

        private static string ParseMetrics(string data)
        {
            var entry = JsonSerializer.Deserialize<CommandResultMetrics>(data).Entry;
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Metrics for \"{entry.Quote}\"</b>:");

            FormatMetrics(entry, ref sb);

            sb.AppendLine($"<i>Listed on: {string.Join(",", entry.Exchanges.Select(x => x.FirstCharToUpper()))}</i>");

            return sb.ToString();
        }

        private static string ParsePortfolio(string data)
        {
            var pf = JsonSerializer.Deserialize<CommandResultPortfolio>(data);
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Generated portfolio for \"{pf.Exchange}\"</b>:");

            foreach (var entry in pf.Entries)
            {
                FormatMetrics(entry, ref sb);
            }

            return sb.ToString();
        }

        private static string ParseList(string data)
        {
            var list = JsonSerializer.Deserialize<CommandResultList>(data).List;
            var sb = new StringBuilder();

            sb.AppendLine("<b>Supported exchanges:</b>")
                .AppendLine();

            foreach (var entry in list)
            {
                sb.AppendLine($"- {entry.Text}");
            }

            return sb.ToString();
        }

        private static string ParseText(string data)
        {
            var entry = JsonSerializer.Deserialize<CommandResultText>(data).Entry;
            return entry.Text;
        }

        private static string ParseError(string data)
        {
            var errors = JsonSerializer.Deserialize<CommandResultError>(data).Entries;
            var sb = new StringBuilder();

            sb.AppendLine("<b>One or multiple errors occured:</b>")
                .AppendLine();

            foreach (var entry in errors)
            {
                sb.AppendLine($"- {entry.Message}");
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
