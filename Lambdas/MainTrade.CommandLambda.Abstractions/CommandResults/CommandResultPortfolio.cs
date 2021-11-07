using System.Collections.Generic;
using System.Linq;

namespace CommandLambda.CommandResults
{
    public record CommandResultPortfolio(IEnumerable<CommandResultPortfolioEntry> Entries, string Exchange);
}
