using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace MainTrade.Data
{
    public class Pair : MainTrade
    {
        [DynamoDBProperty("value")]
        public double? Value { get; set; }

        [DynamoDBProperty("exchanges")]
        public List<string> Exchanges { get; set; }

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

        [DynamoDBProperty("leveraged")]
        public bool? IsLeveraged { get; set; }
    }
}
