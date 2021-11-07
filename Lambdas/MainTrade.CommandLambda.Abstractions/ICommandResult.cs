namespace MainTrade.CommandLambda
{
    public enum CommandResultType
    {
        METRICS,
        PORTFOLIO,
        LIST,
        TEXT,
        ERROR
    }

    public interface ICommandResult
    {
        public CommandResultType Type { get; }

        public object Data { get; }
    }

    public interface ICommandResult<T> : ICommandResult
    {
        public new T Data { get; }
    }
}
