using System.Collections.Generic;
using Network.UnityServer.Behaviors;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    public class UNetworkServerRulesHandler : UNetworkServerHandlerBehavior
    {
        public Dictionary<ushort, PacketHandler> Rules { get; private set; }
        
        public UNetworkServerRulesHandler(UNetworkServer unc) : base(unc) => Rules = new Dictionary<ushort, PacketHandler>();
        
        public void AddRule(ushort packetNumber,  PacketHandler packetHandler) => Rules.TryAdd(packetNumber, packetHandler);
        public void ExecuteRule(ushort clientId, UNetworkReadablePacket packet)
        {
            if(Rules.ContainsKey(packet.PacketNumber)) Rules[packet.PacketNumber](clientId, packet);
        } 
        public override void Close()
        {
            Rules.Clear();
            Rules = null;
        }

        public delegate void PacketHandler(ushort clientId, UNetworkReadablePacket packet);
    }
}