using System.Text;
using AbxExchangeClient.Models;

namespace AbxExchangeClient.Utilities
{
    public static class PacketParser
    {
        /// <summary>
        /// Parses a 17-byte packet into a Packet object.
        /// Handles big-endian decoding for int32 fields.
        /// </summary>
        public static Packet Parse(byte[] buffer)
        {
            var symbol = Encoding.ASCII.GetString(buffer, 0, 4);
            var indicator = Encoding.ASCII.GetString(buffer, 4, 1);
            int quantity = BitConverter.ToInt32(buffer.Skip(5).Take(4).Reverse().ToArray());
            int price = BitConverter.ToInt32(buffer.Skip(9).Take(4).Reverse().ToArray());
            int sequence = BitConverter.ToInt32(buffer.Skip(13).Take(4).Reverse().ToArray());

            return new Packet
            {
                Symbol = symbol.Trim(),
                Indicator = indicator,
                Quantity = quantity,
                Price = price,
                Sequence = sequence
            };
        }
    }
}
