using Amazon.DynamoDBv2.DataModel;

namespace MainTrade.Data
{
    public class Coin : MainTrade
    {
        [DynamoDBProperty("name")]
        public string Name { get; set; }

        [DynamoDBProperty("stable_coin")]
        public bool? IsStableCoin { get; set; }
    }
}
