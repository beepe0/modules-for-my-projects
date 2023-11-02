using System.Collections.Generic;
using Network.UnityServer.Behavior;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    public class UNetworkServerRulesHandler : UNetworkServerHandlerBehavior
    {
        private Dictionary<ushort, PacketHandler> _rulesHandler = new Dictionary<ushort, PacketHandler>();
        public Dictionary<ushort, PacketHandler> Rules => _rulesHandler;
        
        public UNetworkServerRulesHandler(UNetworkServer unc) : base(unc) { }

        public void UpdateGeneralRules(UNetworkServerIORules.IGeneralRules generalRules) => UncServer.GeneralRules = generalRules;
        public void UpdateInputRules(UNetworkServerIORules.IInputRules inputRules) => UncServer.InputRules = inputRules;
        public void UpdateOutputRules(UNetworkServerIORules.IOutputRules outputRules) => UncServer.OutputRules = outputRules;
        public void AddNewRule(ushort packetNumber,  PacketHandler packetHandler) => _rulesHandler.Add(packetNumber, packetHandler);
        public void ExecuteRule(UNetworkReadablePacket packet) => _rulesHandler[packet.PacketNumber](packet.Index, packet);
        public void Clear() => _rulesHandler.Clear();

        public delegate void PacketHandler(ushort clientId, UNetworkReadablePacket packet);

    }
}