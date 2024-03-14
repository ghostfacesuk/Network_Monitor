using System;
using System.Net.NetworkInformation;
using System.Threading;

class NetworkMonitor
{
    static void Main(string[] args)
    {
        // Find the Ethernet network interface
        NetworkInterface ethernetInterface = GetEthernetInterface();
        if (ethernetInterface == null)
        {
            Console.WriteLine("Ethernet interface not found.");
            return;
        }

        // Start monitoring
        Console.WriteLine("Monitoring network bandwidth...");
        long lastBytesReceived = ethernetInterface.GetIPStatistics().BytesReceived;
        long lastBytesSent = ethernetInterface.GetIPStatistics().BytesSent;

        while (true)
        {
            long bytesReceived = ethernetInterface.GetIPStatistics().BytesReceived;
            long bytesSent = ethernetInterface.GetIPStatistics().BytesSent;

            // Calculate bandwidth usage in MegaBytes/sec
            double bytesReceivedPerSecond = (bytesReceived - lastBytesReceived) / (1024.0 * 1024.0);
            double bytesSentPerSecond = (bytesSent - lastBytesSent) / (1024.0 * 1024.0);
            Console.WriteLine($"Current bandwidth - Received: {bytesReceivedPerSecond:F2} MB/s, Sent: {bytesSentPerSecond:F2} MB/s");

            lastBytesReceived = bytesReceived;
            lastBytesSent = bytesSent;

            Thread.Sleep(1000); // Update every second
        }
    }

    // Find the Ethernet network interface
    static NetworkInterface GetEthernetInterface()
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface networkInterface in interfaces)
        {
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                return networkInterface;
            }
        }
        return null;
    }
}