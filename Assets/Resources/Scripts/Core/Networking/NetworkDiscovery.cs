using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatGPTCommon
{
    public class NetworkDiscovery
    {
        private const string Subnet = "192.168.1";
        private const int StartRange = 1;
        private const int EndRange = 255;

        public async Task<IEnumerable<string>> DiscoverHosts(int port)
        {
            List<string> hosts = new List<string>();
            for (int i = StartRange; i <= EndRange; i++)
            {
                string ipAddress = $"{Subnet}.{i}";

                await Task.Run(() =>
                {
                    try
                    {
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
                        Debug.Log($"Host found at IP: {ipAddress}");
                        socket.Close();
                        hosts.Add(ipAddress);
                    }
                    catch (SocketException)
                    {

                    }
                });
            }
            return hosts;
        }
    }
}
