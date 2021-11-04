using CommandLine;

namespace MainTrade.CommandLambda.Options
{
    [Verb("portfolio", HelpText = "Generate a portfolio.")]
    public class PortfolioOptions : IOptions
    {
        [Option('e', "exchange", Required = true, HelpText = "Exchange to generate for.")]
        public string Exchange { get; set; }

        [Option('a', "amount", Required = true, HelpText = "Amount of coin pairs to use.")]
        public int Amount { get; set; }

        [Option('r', "risk", Default = 0.75, HelpText = "Maximum risk.")]
        public double Risk { get; set; }

        [Option('b', "base", HelpText = "Base to use.")]
        public string Base { get; set; }


        [Option("mcmin", SetName = "market_cap", HelpText = "Minimum market cap.")]
        public int? MarketCapMin { get; set; }

        [Option("mcmax", SetName = "market_cap", HelpText = "Maximum market cap.")]
        public int? MarketCapMax { get; set; }


        [Option("mcrmin", SetName = "market_cap_rank", HelpText = "Minimum market cap rank.")]
        public int? MarketCapRankMin { get; set; }

        [Option("mcrmax", SetName = "market_cap_rank", HelpText = "Maximum market cap rank.")]
        public int? MarketCapRankMax { get; set; }
    }
}
