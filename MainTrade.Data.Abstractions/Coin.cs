using Amazon.DynamoDBv2.DataModel;

namespace MainTrade.Data
{
    [DynamoDBTable("coins")]
    public class Coin
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }

        [DynamoDBRangeKey("name")]
        public string Name { get; set; }

        [DynamoDBProperty("exchange_id")]
        public int ExchangeId { get; set; }

        [DynamoDBProperty("base")]
        public string Base { get; set; }

        [DynamoDBProperty("market_cap")]
        public int? MarketCap { get; set; }

        [DynamoDBProperty("market_cap_rank")]
        public int? MarketCapRank { get; set; }

        [DynamoDBProperty("alt_rank")]
        public int? AltRank { get; set; }

        [DynamoDBProperty("momentum")]
        public double? Momentum { get; set; }

        [DynamoDBProperty("risk")]
        public double? Risk { get; set; }

        [DynamoDBProperty("stablecoin")]
        public bool? IsStableCoin { get; set; }

        [DynamoDBProperty("leveraged")]
        public bool? IsLeveraged { get; set; }
    }
}
