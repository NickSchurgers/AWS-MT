using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using MainTrade.CommandLambda;
using MainTrade.CommandLambda.Options;
using System.Threading.Tasks;

namespace CommandLambda
{
    public abstract class BaseCommand<T> : ICommand<T> where T : IOptions
    {
        private readonly static AmazonDynamoDBClient _client = new();

        public DynamoDBContext Context => new(_client);

        public AmazonDynamoDBClient Client => _client;

        public abstract Task<CommandResult> ProcessAsync(T options);
    }
}
