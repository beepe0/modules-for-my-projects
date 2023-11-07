using System;
using Network.UnityServer.Handlers;
using UnityEngine;

namespace Network.UnityServer
{
    [Serializable]
    public sealed class UNetworkClient
    {
        private ushort _index;
        
        private UNetworkServer _networkServer;
        private UNetworkServerProtocolTcpHandler _tcpHandler;
        private UNetworkServerProtocolUdpHandler _udpHandler;

        public ushort Index
        {
            get => _index;
            set => _index = value;
        }
        public UNetworkServer NetworkServer => _networkServer;
        public UNetworkServerProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkServerProtocolUdpHandler UdpHandler => _udpHandler;

        public UNetworkClient(UNetworkServer server, ushort index)
        {
            _index = index;
            _networkServer = server;
            
            _tcpHandler = new UNetworkServerProtocolTcpHandler(this);
            _udpHandler = new UNetworkServerProtocolUdpHandler(this);
        }
        public void Close()
        {
            if(_tcpHandler is { IsTcpConnect: true } || _udpHandler is { IsUdpConnect: true }) NetworkServer.OnDisconnectClient(_index);
            
            if (_tcpHandler != null) _tcpHandler.Close();
            if (_udpHandler != null) _udpHandler.Close();
        }
    }
}