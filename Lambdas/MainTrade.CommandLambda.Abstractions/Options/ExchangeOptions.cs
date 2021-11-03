using CommandLine;
using MainTrade.CommandLambda.Options;

namespace CommandLambda.Options
{
    [Verb("exchange", HelpText = "Perform commands related to exchanges.")]
    public class ExchangeOptions : IOptions
    {
    }
}
