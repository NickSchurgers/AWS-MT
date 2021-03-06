using MainTrade.Data;

namespace CommandLambda.CommandResults
{
    public record CommandResultPortfolioEntry(double Allocation, string Quote, int MarketCap, double Risk, double Momentum, int AltRank, int MarketCapRank)
        : CommandResultMetricsEntryBase(Quote, MarketCap, Risk, Momentum, AltRank, MarketCapRank);
}
