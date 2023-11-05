using System.Collections.Generic;
using Network.UnityServer.Behaviors;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    public class UNetworkServerRulesHandler : UNetworkServerHandlerBehavior
    {
        private Dictionary<ushort, PacketHandler> _rulesHandler = new Dictionary<ushort, PacketHandler>();
        public Dictionary<ushort, PacketHandler> Rules => _rulesHandler;
        
        public UNetworkServerRulesHandler(UNetworkServer unc) : base(unc) { }
        
        public void AddRule(ushort packetNumber,  PacketHandler packetHandler) => _rulesHandler.Add(packetNumber, packetHandler);
        public void ExecuteRule(UNetworkReadablePacket packet) => _rulesHandler[packet.PacketNumber](packet.Index, packet);
        public override void Close()
        {
            _rulesHandler.Clear();
            _rulesHandler = null;
        }

        public delegate void PacketHandler(ushort clientId, UNetworkReadablePacket packet);

    }
}