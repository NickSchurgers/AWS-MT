using CommandLambda.Options;
using System.Threading.Tasks;

namespace CommandLambda
{
    public interface ICommand<TOptions> where TOptions : IOptions
    {
        public Task<CommandResult> ProcessAsync(TOptions options);
    }
}
