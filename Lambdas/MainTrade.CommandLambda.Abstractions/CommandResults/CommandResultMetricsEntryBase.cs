namespace CommandLambda.CommandResults
{
    public abstract record CommandResultMetricsEntryBase(string Quote, int MarketCap, double Risk, double Momentum, int AltRank, int MarketCapRank);
}
