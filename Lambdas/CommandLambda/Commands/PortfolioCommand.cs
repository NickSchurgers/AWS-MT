using CommandLambda.Options;
using System;
using System.Threading.Tasks;

namespace CommandLambda.Commands
{
    internal class PortfolioCommand : ICommand<PortfolioOptions>
    {
        public Task<CommandResult> ProcessAsync(PortfolioOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
