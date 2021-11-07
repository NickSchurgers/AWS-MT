using System.Collections.Generic;

namespace CommandLambda.CommandResults
{
    public record CommandResultMetricsEntry(string Quote, int MarketCap, double Risk, double Momentum, int AltRank, int MarketCapRank, IEnumerable<string> Exchanges)
        : CommandResultMetricsEntryBase(Quote, MarketCap, Risk, Momentum, AltRank, MarketCapRank);
}
