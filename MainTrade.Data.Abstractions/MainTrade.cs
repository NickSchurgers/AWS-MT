using Amazon.DynamoDBv2.DataModel;

namespace MainTrade.Data
{
    [DynamoDBTable("maintrade")]
    public class MainTrade
    {
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }

        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}
