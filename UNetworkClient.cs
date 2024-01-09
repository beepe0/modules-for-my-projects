using System;
using Network.UnityServer.Handlers;
using UnityEngine;

namespace Network.UnityServer
{
    public class UNetworkClient
    {
        private ushort _index;

        private UNetworkRoom _mainSession;
        private UNetworkServer _networkServer;
        private UNetworkServerProtocolTcpHandler _tcpHandler;
        private UNetworkServerProtocolUdpHandler _udpHandler;

        public ushort Index => _index;
        public UNetworkRoom MainSession => _mainSession;
        public UNetworkServer NetworkServer => _networkServer;
        public UNetworkServerProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkServerProtocolUdpHandler UdpHandler => _udpHandler;

        public static T CreateInstance<T>(UNetworkServer server, ushort index) where T : UNetworkClient, new()
        {
            T client = new T();
            
            client._index = index;
            client._networkServer = server;
            
            client._tcpHandler = new UNetworkServerProtocolTcpHandler(client);
            client._udpHandler = new UNetworkServerProtocolUdpHandler(client);

            return client;
        }
        public void Disconnect()
        {
            if (_tcpHandler is { IsTcpConnect: true } || _udpHandler is { IsUdpConnect: true }) NetworkServer.OnDisconnectClient(_index);
            
            if (_tcpHandler != null) _tcpHandler.Close();
            if (_udpHandler != null) _udpHandler.Close();
        }
        public void Enter(ushort id)
        {
            if (_tcpHandler is { IsTcpConnect: true } || _udpHandler is { IsUdpConnect: true }) NetworkServer.OnEntryToRoomClient(_index);

            _mainSession = _networkServer.Rooms[id];
        }
        public void Exit()
        {
            if (_tcpHandler is { IsTcpConnect: true } || _udpHandler is { IsUdpConnect: true }) NetworkServer.OnExitFromRoomClient(_index);

            _mainSession.Clients.Remove(_index);
        }
    }
}