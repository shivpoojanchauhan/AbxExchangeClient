# AbxExchangeClient
This document provides a specification for developers who want to code a client to interact with the ABX mock exchange server. The ABX server simulates a stock exchange environment and allows clients to request data related to the order book.
# 📈 ABX Exchange Client

This project is a TCP client built using C# (.NET 8) that connects to a Node.js-based stock exchange server. It:
- Streams real-time stock ticker packets.
- Detects missing sequences.
- Requests retransmission (Call Type 2).
- Writes the final ordered packet data to `output.json`.

# 📁 Project Structure

AbxExchangeClient/
├── AbxExchangeClient.csproj
├── appsettings.json
├── output.json ⬅️ Generated after successful run
├── Program.cs
├── Models/
│ └── Packet.cs
├── Utilities/
│ └── PacketParser.cs
├── Config/
│ └── AbxExchangeSettings.cs
└── NodeServer/
└── main.js ⬅️ Node.js mock exchange server

# ⚙️ Configuration

Edit the `appsettings.json` file to update host, port, and packet size:

{
  "AbxExchange": {
    "Host": "127.0.0.1",
    "Port": 3000,
    "PacketSize": 17
  }
}

# 📦 NuGet Packages Used
------Package Name	---------Purpose
- Newtonsoft.Json	Serialize packets to output.json
- Microsoft.Extensions.Configuration	Configuration loading
- Microsoft.Extensions.Configuration.Json	Read from appsettings.json
- Microsoft.Extensions.Configuration.Binder	Map config to POCO class

# Install packages with using Nuget-Solution:
Install packages with:
- dotnet add package Newtonsoft.Json
- dotnet add package Microsoft.Extensions.Configuration
- dotnet add package Microsoft.Extensions.Configuration.Json
- dotnet add package Microsoft.Extensions.Configuration.Binder

# How to Run

1. Launch main.js in Visual Studio Code.
2. Navigate to Run > Run Without Debugging.
3. Execute the .NET console application in debug mode and observe the output displayed in the .NET command console
