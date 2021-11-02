using CommandLine;

namespace CommandLambda.Options
{
    [Verb("metrics", HelpText = "Perform magic on single coin pairs.")]
    public class MetricsOptions
    {
        [Option('p', "pair", Required = true)]
        public string Pair { get; set; }

        [Option('c', "chart", Default = false)]
        public bool? ShowChart { get; set; }

        [Option('l', "log", Default = false)]
        public bool? UseLog { get; set; }
    }
}
