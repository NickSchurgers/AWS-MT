using System.Collections.Generic;

namespace CommandLambda.CommandResults
{
    public record CommandResultError(IEnumerable<CommandResultErrorEntry> Entries);
}
