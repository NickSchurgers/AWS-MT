using CommandLambda.CommandResults;
using MainTrade.CommandLambda;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var metrics = JsonSerializer.Deserialize<CommandResultMetrics>(data);
            return metrics.Text;
        }

        private static string ParsePortfolio(string data)
        {
            var pf = JsonSerializer.Deserialize<CommandResultPortfolio>(data);
            var sb = new StringBuilder();

            sb.AppendLine($"<b>Generated portfolio for \"{pf.Exchange}\"</b>:");

            foreach (var entry in pf.Entries)
            {
                sb.AppendLine()
                    .AppendLine($"<u>{entry.Quote} - {entry.Allocation}%</u>")
                    .AppendLine($"<i>Risk: {entry.Risk} | Momentum: {entry.Momentum}</i>")
                    .AppendLine($"<i>Market cap: {entry.MarketCap} | Market cap rank: {entry.MarketCapRank} | Alt rank: {entry.AltRank}</i>");
            }

            return sb.ToString();
        }

        private static string ParseList(string data)
        {
            var list = JsonSerializer.Deserialize<CommandResultList>(data).List;
            var sb = new StringBuilder();

            sb.AppendLine("<b>Supported exchanges:</b>");

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
            var error = JsonSerializer.Deserialize<CommandResultError>(data);
            return error.Exception.Message;
        }
    }
}
