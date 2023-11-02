﻿using System.Collections.Generic;
using Network.UnityClient.Behavior;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public class UNetworkClientRulesHandler : UNetworkClientHandlerBehavior
    {
        private Dictionary<ushort, PacketHandler> _rulesHandler = new Dictionary<ushort, PacketHandler>();
        public Dictionary<ushort, PacketHandler> Rules => _rulesHandler;
        
        public UNetworkClientRulesHandler(UNetworkClient unc) : base(unc){}

        public void UpdateGeneralRules(UNetworkClientIORules.IGeneralRules generalRules) => UncClient.GeneralRules = generalRules;
        public void UpdateInputRules(UNetworkClientIORules.IInputRules inputRules) => UncClient.InputRules = inputRules;
        public void UpdateOutputRules(UNetworkClientIORules.IOutputRules outputRules) => UncClient.OutputRules = outputRules;
        public void AddNewRule(ushort packetNumber,  PacketHandler packetHandler) => _rulesHandler.Add(packetNumber, packetHandler);
        public void ExecuteRule(UNetworkReadablePacket packetTools) => _rulesHandler[packetTools.PacketNumber](packetTools);

        public override void Close()
        {
            _rulesHandler.Clear();
            _rulesHandler = null;
        }

        public delegate void PacketHandler(UNetworkReadablePacket packetTools);
    }
}