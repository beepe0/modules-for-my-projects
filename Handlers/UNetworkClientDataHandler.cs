using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkClientDataHandler : UNetworkClientHandlerBehavior
    {
        public UNetworkClientDataHandler(UNetworkClient unc) : base(unc){}

        public void SendDataTcp(UNetworkIOPacket outputPacket)
        {
            if(UncClient.TcpHandler == null) return;
                    
            outputPacket.Insert(UncClient.Index);
            outputPacket.Insert(outputPacket.GetLength());
            UncClient.TcpHandler.SendData(outputPacket.ToArray());
        }
        public void SendDataUdp(UNetworkIOPacket outputPacket)
        {
            if(UncClient.UdpHandler == null) return;
                    
            outputPacket.Insert(UncClient.Index);
            outputPacket.Insert(outputPacket.GetLength());
            UncClient.UdpHandler.SendData(outputPacket.ToArray());
        }
    }
}