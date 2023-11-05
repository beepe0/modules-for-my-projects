using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network.UnityServer.Behaviors;
using Network.UnityServer.Handlers;
using Network.UnityTools;

namespace Network.UnityServer
{
    public abstract class UNetworkServer : UNetworkServerBehavior
    {
        private bool _isRunServer;

        private TcpListener _tcpListener;
        private UdpClient _udpListener;
        
        private Dictionary<ushort, UNetworkUser> _clients = new Dictionary<ushort, UNetworkUser>();
        private Dictionary<ushort, UNetworkRoom> _rooms = new Dictionary<ushort, UNetworkRoom>();
        
        private UNetworkServerRulesHandler _rulesHandler;
        private UNetworkServerDataHandler _dataHandler;

        public bool IsRunServer => _isRunServer;
        public TcpListener TcpListener => _tcpListener;
        public UdpClient UdpListener => _udpListener;
        public Dictionary<ushort, UNetworkUser> Clients => _clients;
        public Dictionary<ushort, UNetworkRoom> Rooms => _rooms;
        public UNetworkServerRulesHandler RulesHandler => _rulesHandler;
        public UNetworkServerDataHandler DataHandler => _dataHandler;
        
        public new void Create()
        {
            if (!_isRunServer)
            {
                OnCreate();
                _rulesHandler = new UNetworkServerRulesHandler(this);
                _dataHandler = new UNetworkServerDataHandler(this);
                for (ushort clientId = 0; clientId < slots; clientId++)
                {
                    _clients.Add(clientId, new UNetworkUser(this, clientId));
                }
                
                _tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(serverInternetProtocol), serverPort));
                _tcpListener.Start();
                _tcpListener.BeginAcceptTcpClient(CallBackAcceptTcpClient, null);
                _isRunServer = true;
                _udpListener = new UdpClient(serverPort);
                _udpListener.BeginReceive(CallBackUdpReceive, null);
            }
        }
        private void CallBackAcceptTcpClient(IAsyncResult asyncResult)
        {
            TcpClient cl = _tcpListener.EndAcceptTcpClient(asyncResult);
            _tcpListener.BeginAcceptTcpClient(CallBackAcceptTcpClient, null);

            foreach (UNetworkUser t in _clients.Values)
            {
                if(t.TcpHandler.TcpSocket == null) { t.TcpHandler.Connect(cl); return; }
            }
            
        }
        private void CallBackUdpReceive(IAsyncResult asyncResult)
        {
            if (_isRunServer)
            {
                try
                {
                    IPEndPoint epClient = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = _udpListener.EndReceive(asyncResult, ref epClient);
                    
                    if(data.Length < 4)
                    {
                        return;
                    }

                    UNetworkIOPacket inputPacket = new UNetworkIOPacket(data);
                    
                    UNetworkReadablePacket readablePacket = new UNetworkReadablePacket
                    {
                        Length = inputPacket.ReadUShort(),
                        Index = inputPacket.ReadUShort(),
                        PacketNumber = inputPacket.ReadUShort(),
                        BufferBytes = inputPacket.ReadBytes((ushort)(inputPacket.GetLength() - inputPacket.ReadPointer)),
                    };
                    
                    if(!_clients.ContainsKey(readablePacket.Index)) return;
             
                    var uNetworkClient = _clients[readablePacket.Index];

                    if (uNetworkClient.UdpHandler.EndPoint == null)
                    {
                        uNetworkClient.UdpHandler.Connect(epClient); 
                        uNetworkClient.UdpHandler.HandleData(readablePacket);
                    }
                    else if (uNetworkClient.UdpHandler.EndPoint.Equals(epClient))
                    {
                        uNetworkClient.UdpHandler.HandleData(readablePacket);
                    }

                    _udpListener.BeginReceive(CallBackUdpReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingUdp(e);
                    Close();
                }
            }
        }
        public new void Close()
        {
            if (_isRunServer)
            {
                OnClose();
                _isRunServer = false;
                
                _udpListener.Close();
                _tcpListener.Stop();
                
                if(_rulesHandler.Rules != null) _rulesHandler.Close();
                
                foreach (UNetworkUser networkClient in _clients.Values) networkClient.Close();
                
                _clients.Clear();
            }
        }
    }
}