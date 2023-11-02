using System;
using Network.UnityServer.Handlers;

namespace Network.UnityServer
{
    [Serializable]
    public sealed class UNetworkClient
    {
        private readonly ushort _clientId;

        private UNetworkServer _networkServer;
        private UNetworkServerProtocolTcpHandler _tcpHandler;
        private UNetworkServerProtocolUdpHandler _udpHandler;

        public ushort ClientId => _clientId;
        public UNetworkServer NetworkServer => _networkServer;
        public UNetworkServerProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkServerProtocolUdpHandler UdpHandler => _udpHandler;

        public UNetworkClient(UNetworkServer server, ushort clientId)
        {
            _clientId = clientId;
            _networkServer = server;
            
            _tcpHandler = new UNetworkServerProtocolTcpHandler(this);
            _udpHandler = new UNetworkServerProtocolUdpHandler(this);
        }
        
        public void Close()
        {
            if(_tcpHandler is { IsTcpConnect: true } || _udpHandler is { IsUdpConnect: true }) NetworkServer.GeneralRules.OnDisconnect(_clientId);
            
            if (_tcpHandler != null) _tcpHandler.Close();
            if (_udpHandler != null) _udpHandler.Close();
        }
    }
}