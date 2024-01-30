using System.Collections.Generic;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkConnectionRulesHandler : UNetworkConnectionHandlerBehavior
    {
        public Dictionary<ushort, PacketHandler> Rules { get; private set; }
        
        public UNetworkConnectionRulesHandler(UNetworkConnection unc) : base(unc) => Rules = new Dictionary<ushort, PacketHandler>();

        public void AddRule(ushort packetNumber, PacketHandler packetHandler) => Rules.TryAdd(packetNumber, packetHandler);
        public void ExecuteRule(UNetworkReadablePacket packet)
        {
            if(Rules.ContainsKey(packet.PacketNumber)) Rules[packet.PacketNumber](packet);
        } 
        
        public override void Close()
        {
            Rules.Clear();
            Rules = null;
        }

        public delegate void PacketHandler(UNetworkReadablePacket packet);
    }
}