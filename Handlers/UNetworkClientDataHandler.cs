using Network.UnityClient.Behavior;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkClientDataHandler : UNetworkClientHandlerBehavior
    {
        public UNetworkClientDataHandler(UNetworkClient unc) : base(unc){}

        public void SendDataTcp(UNetworkIOPacket outputPacket)
        {
            if(UncClient.TcpHandler == null) return;
                    
            outputPacket.Insert(UncClient.ClientId);
            outputPacket.Insert(outputPacket.GetLength());
            UncClient.TcpHandler.SendData(outputPacket.ToArray());
        }
        public void SendDataUpd(UNetworkIOPacket outputPacket)
        {
            if(UncClient.UdpHandler == null) return;
                    
            outputPacket.Insert(UncClient.ClientId);
            outputPacket.Insert(outputPacket.GetLength());
            UncClient.UdpHandler.SendData(outputPacket.ToArray());
        }
    }
}