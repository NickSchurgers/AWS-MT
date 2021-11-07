using CommandLine;

namespace MainTrade.CommandLambda.Options
{
    [Verb("portfolio", HelpText = "Generate a portfolio.")]
    public class PortfolioOptions : IOptions
    {
        [Option('e', "exchange", Required = true, HelpText = "Exchange to generate for.")]
        public string Exchange { get; set; }

        [Option('c', "amount", Required = true, HelpText = "Amount of coin pairs to use.")]
        public int Amount { get; set; }

        [Option('r', "risk", Default = 0.75, HelpText = "Maximum risk. Defaults to 0.75")]
        public double Risk { get; set; }

        [Option('b', "base", Default = "BTC", HelpText = "Base to use. Defaults to BTC.")]
        public string Base { get; set; }

        [Option('s', "sort", Default = "momentum", HelpText = "Sort results by momentum, risk or alt_rank. Defaults to momentum.")]
        public string Sort { get; set; }

        [Option('d', "direction", Default = "desc", HelpText = "Sort direction; asc or desc. Defaults to desc.")]
        public string SortDirection { get; set; }

        [Option("rw", Default = 0.50, HelpText = "Portfolio allocation weight for risk. Defaults to 0.50")]
        public double RiskWeight { get; set; }

        [Option("mw", Default = 0.50, HelpText = "Portfolio allocation weight for momentum. Defaults to 0.50")]
        public double MomentumWeight { get; set; }

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
