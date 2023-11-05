using System.Collections.Generic;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkClientRulesHandler : UNetworkClientHandlerBehavior
    {
        private Dictionary<ushort, PacketHandler> _rulesHandler = new Dictionary<ushort, PacketHandler>();
        public Dictionary<ushort, PacketHandler> Rules => _rulesHandler;
        
        public UNetworkClientRulesHandler(UNetworkClient unc) : base(unc){}
        
        public void AddRule(ushort packetNumber,  PacketHandler packetHandler) => _rulesHandler.Add(packetNumber, packetHandler);
        public void ExecuteRule(UNetworkReadablePacket packet) => _rulesHandler[packet.PacketNumber](packet);
        public override void Close()
        {
            _rulesHandler.Clear();
            _rulesHandler = null;
        }

        public delegate void PacketHandler(UNetworkReadablePacket packet);
    }
}