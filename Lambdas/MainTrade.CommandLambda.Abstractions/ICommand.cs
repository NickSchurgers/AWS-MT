using MainTrade.CommandLambda.Options;
using System.Threading.Tasks;

namespace MainTrade.CommandLambda
{
    public interface ICommand<TOptions> where TOptions : IOptions
    {
        public Task<ICommandResult> ProcessAsync(TOptions options);
    }
}
