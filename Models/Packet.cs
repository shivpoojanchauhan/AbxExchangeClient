using Newtonsoft.Json;

namespace AbxExchangeClient.Models
{
    public class Packet
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("buysellindicator")]
        public string Indicator { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("packetSequence")]
        public int Sequence { get; set; }
    }
}
