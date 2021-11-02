using CommandLambda.Options;
using System;
using System.Threading.Tasks;

namespace CommandLambda.Commands
{
    internal class MetricsCommand : ICommand<MetricsOptions>
    {
        public Task<CommandResult> ProcessAsync(MetricsOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
