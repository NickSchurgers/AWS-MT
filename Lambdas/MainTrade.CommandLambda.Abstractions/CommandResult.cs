using MainTrade.CommandLambda;

namespace CommandLambda
{
    public class CommandResult<T> : ICommandResult<T>
    {
        public T Data { get; init; }

        public CommandResultType Type { get; init; }

        object ICommandResult.Data => Data;

        public CommandResult(CommandResultType type, T data)
        {
            Type = type;
            Data = data;
        }
    }
}
