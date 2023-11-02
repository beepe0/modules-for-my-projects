using Network.UnityServer.Behavior;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    public class UNetworkServerDataHandler : UNetworkServerHandlerBehavior
    {
        public UNetworkServerDataHandler(UNetworkServer unc) : base(unc){}

        public void SendDataTcp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.TcpHandler.IsTcpConnect)) return;
                
            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            client.TcpHandler.SendData(outputPacket.ToArray());
        }

        public void SendDataToAllTcp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.TcpHandler.IsTcpConnect)) return;
            
            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            
            foreach (UNetworkClient c in UncServer.Clients.Values)
            {
                c.TcpHandler.SendData(outputPacket.ToArray());
            }
        }

        public void SendDataToAllExceptClientTcp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.TcpHandler.IsTcpConnect)) return;
            
            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            
            foreach (UNetworkClient c in UncServer.Clients.Values)
            {
                if (clientId != c.ClientId) c.TcpHandler.SendData(outputPacket.ToArray());
            }
        }

        public void SendDataUdp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.UdpHandler.IsUdpConnect)) return;
            
            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            client.UdpHandler.SendData(outputPacket.ToArray());
        }

        public void SendDataToAllUdp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.UdpHandler.IsUdpConnect)) return;

            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            
            foreach (UNetworkClient c in UncServer.Clients.Values)
            {
                c.UdpHandler.SendData(outputPacket.ToArray());
            }
        }

        public void SendDataToAllExceptClientUdp(ushort clientId, UNetworkIOPacket outputPacket)
        {
            if (!(UncServer.Clients.TryGetValue(clientId, out var client) && client != null && client.UdpHandler.IsUdpConnect)) return;

            outputPacket.Insert(clientId);
            outputPacket.Insert(outputPacket.GetLength());
            
            foreach (UNetworkClient c in UncServer.Clients.Values)
            {
                if(clientId != c.ClientId) c.UdpHandler.SendData(outputPacket.ToArray());
            }
        }
    }
}