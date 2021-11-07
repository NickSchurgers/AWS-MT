using System.Collections.Generic;

namespace CommandLambda.CommandResults
{
    public record CommandResultList(IEnumerable<CommandResultTextEntry> List);
}
