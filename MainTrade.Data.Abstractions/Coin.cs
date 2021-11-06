using Amazon.DynamoDBv2.DataModel;

namespace MainTrade.Data
{
    public class Coin : MainTrade
    {
        [DynamoDBProperty("full_name")]
        public string Name { get; set; }

        [DynamoDBProperty("stable_coin")]
        public bool? IsStableCoin { get; set; }
    }
}
