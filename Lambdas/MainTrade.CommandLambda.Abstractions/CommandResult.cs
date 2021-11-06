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

    public record CommandResult(CommandResultType Type, object Result);
}
