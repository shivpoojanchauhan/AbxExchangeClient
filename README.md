# AbxExchangeClient
This document provides a specification for developers who want to code a client to interact with the ABX mock exchange server. The ABX server simulates a stock exchange environment and allows clients to request data related to the order book.
# 📈 ABX Exchange Client

This project is a TCP client built using C# (.NET 8) that connects to a Node.js-based stock exchange server. It:
- Streams real-time stock ticker packets.
- Detects missing sequences.
- Requests retransmission (Call Type 2).
- Writes the final ordered packet data to `output.json` in `AbxExchangeClient` application root directory.

# 📁 Project Structure:

```text
AbxExchangeClient/
├── AbxExchangeClient.csproj
├── appsettings.json
├── output.json ⬅️ Generated after successful run
├── Program.cs
├── Models/
│   └── Packet.cs
├── Utilities/
│   └── PacketParser.cs
├── Config/
    └── AbxExchangeSettings.cs

```

# ⚙️ Configuration:

You can modify the appsettings.json file to configure the host, port, and packet size according to your system's specific host address, port number, and desired packet size.
```text
{
  "AbxExchange": {
    "Host": "127.0.0.1",
    "Port": 3000,
    "PacketSize": 17
  }
}
```
### 📦 NuGet Packages Used

| Package Name                             | Purpose                                      |
|------------------------------------------|----------------------------------------------|
| `Newtonsoft.Json`                        | Serialize packet data to `output.json`       |
| `Microsoft.Extensions.Configuration`     | Load and manage application configuration    |
| `Microsoft.Extensions.Configuration.Json`| Read settings from `appsettings.json`        |
| `Microsoft.Extensions.Configuration.Binder` | Bind configuration values to POCO classes |

# Install packages with using Nuget Package Manager:
Install packages with:
- dotnet add package Newtonsoft.Json
- dotnet add package Microsoft.Extensions.Configuration
- dotnet add package Microsoft.Extensions.Configuration.Json
- dotnet add package Microsoft.Extensions.Configuration.Binder

# How to Run:

1. Launch main.js in Visual Studio Code.
2. Navigate to Run > Run Without Debugging.
3. Execute the .NET console application in debug mode and observe the output displayed in the .NET command console
