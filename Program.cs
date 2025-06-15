using System.Net.Sockets;
using Newtonsoft.Json;
using AbxExchangeClient.Models;
using AbxExchangeClient.Utilities;
using AbxExchangeClient.Config;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var settings = config.GetSection("AbxExchange").Get<AbxExchangeSettings>();
        Console.WriteLine($"HOST = {settings.Host}, PORT = {settings.Port}, PACKET_SIZE = {settings.PacketSize}");


        var allPackets = new Dictionary<int, Packet>();

        // -----------------------------
        // Step 1: Call Type 1 - Stream All Packets
        // -----------------------------
        // Establish a TCP connection and send [0x01, 0x00]
        // 0x01 = Call Type 1 (Stream All)
        // 0x00 = dummy value for resendSeq (not used in Call Type 1)
        using (var client = new TcpClient(settings.Host, settings.Port))
        using (var stream = client.GetStream())
        {
            stream.Write(new byte[] { 0x01, 0x00 }, 0, 2);
            Console.WriteLine("Sent CallType 1 request to server");

            var memory = new List<byte>();
            var tempBuffer = new byte[1024];
            int bytesRead;

            // Receive data until the server closes the connection
            while ((bytesRead = stream.Read(tempBuffer, 0, tempBuffer.Length)) > 0)
            {
                memory.AddRange(tempBuffer.Take(bytesRead));
            }

            // Parse all 17-byte packets from the memory buffer
            for (int i = 0; i + settings.PacketSize <= memory.Count; i += settings.PacketSize)
            {
                var packetData = memory.Skip(i).Take(settings.PacketSize).ToArray();
                var packet = PacketParser.Parse(packetData);
                allPackets[packet.Sequence] = packet;
            }
        }

        // Step 2: Detect Missing Sequences
        var maxSeq = allPackets.Keys.Max();
        var missingSeqs = Enumerable.Range(1, maxSeq).Where(seq => !allPackets.ContainsKey(seq)).ToList();

        // -----------------------------
        // Step 3: Call Type 2 - Resend Missing Packets
        // -----------------------------
        // For each missing sequence, send [0x02, sequence]
        // 0x02 = Call Type 2 (Resend Request)
        // sequence = missing sequence number (1 byte)
        foreach (var seq in missingSeqs)
        {
            using (var client = new TcpClient(settings.Host, settings.Port))
            using (var stream = client.GetStream())
            {
                // Send Call Type 2 request with the missing sequence number
                stream.Write(new byte[] { 0x02, (byte)seq }, 0, 2);
                Console.WriteLine($"Sent CallType 2 request for missing packet #{seq}");

                var buffer = new byte[settings.PacketSize];
                int read = stream.Read(buffer, 0, settings.PacketSize);
                if (read > 0)
                {
                    var packet = PacketParser.Parse(buffer);
                    allPackets[packet.Sequence] = packet;
                    Console.WriteLine($"Received missing Packet #{packet.Sequence}: {packet.Symbol} {packet.Indicator} Qty={packet.Quantity} Price={packet.Price}");
                }
                else
                {
                    Console.WriteLine($"Warning: No data received for CallType 2 packet #{seq}");
                }
            }
        }

        // -----------------------------
        // Step 4: Save JSON
        // -----------------------------
        var ordered = allPackets.Values.OrderBy(p => p.Sequence).ToList();

        var outputPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "output.json");
        File.WriteAllText(Path.GetFullPath(outputPath), JsonConvert.SerializeObject(ordered, Newtonsoft.Json.Formatting.Indented));
        var fullPath = Path.GetFullPath(outputPath);

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(ordered, Newtonsoft.Json.Formatting.Indented));
        Console.WriteLine($"All packets saved to: {fullPath}");

    }
}
