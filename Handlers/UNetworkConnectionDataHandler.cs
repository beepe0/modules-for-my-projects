using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkConnectionDataHandler : UNetworkConnectionHandlerBehavior
    {
        public UNetworkConnectionDataHandler(UNetworkConnection unc) : base(unc){}

        public void SendDataTcp(UNetworkIOPacket outputPacket)
        {
            if(Connection.TcpHandler == null) return;
                    
            outputPacket.Insert(Connection.Index);
            outputPacket.Insert(outputPacket.GetLength());
            Connection.TcpHandler.SendData(outputPacket.ToArray());
        }
        public void SendDataUdp(UNetworkIOPacket outputPacket)
        {
            if(Connection.UdpHandler == null) return;
                    
            outputPacket.Insert(Connection.Index);
            outputPacket.Insert(outputPacket.GetLength());
            Connection.UdpHandler.SendData(outputPacket.ToArray());
        }
    }
}