using MainTrade.CommandLambda.Options;
using System;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda.Commands
{
    internal class MetricsCommand : ICommand<MetricsOptions>
    {
        public Task<ICommandResult> ProcessAsync(MetricsOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
