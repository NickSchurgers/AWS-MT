using Amazon.DynamoDBv2.DataModel;

namespace MainTrade.Data
{
    [DynamoDBTable("exchanges")]
    public class Exchange
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }

        [DynamoDBRangeKey("name")]
        public string Name { get; set; }

    }
}
