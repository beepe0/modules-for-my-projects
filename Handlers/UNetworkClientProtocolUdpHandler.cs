using System;
using System.Net;
using System.Net.Sockets;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public sealed class UNetworkClientProtocolUdpHandler : UNetworkClientHandlerBehavior
    {
        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private bool _isUdpConnect;
        
        public UdpClient UdpClient => _udpClient;
        public IPEndPoint EndPoint => _endPoint;
        public bool IsUdpConnect => _isUdpConnect;

        public UNetworkClientProtocolUdpHandler(UNetworkClient unc) : base(unc){}
        
        public void Connect()
        {
            if (!_isUdpConnect)
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(UncClient.ClientManager.serverInternetProtocol), UncClient.ClientManager.serverPort);
        
                _udpClient = new UdpClient();
                
                _udpClient.Client.SendBufferSize = UncClient.ClientManager.sendBufferSize;
                _udpClient.Client.ReceiveBufferSize = UncClient.ClientManager.receiveBufferSize;
            
                _udpClient.Connect(_endPoint);
                _udpClient.BeginReceive(CallBackReceive, null);

                _isUdpConnect = true;
            }
        }
        private void CallBackReceive(IAsyncResult asyncResult)
        {
            if (_isUdpConnect)
            {
                try
                {
                    byte[] data = _udpClient.EndReceive(asyncResult, ref _endPoint);

                    if (data.Length < 4)
                    {
                        UncClient.Close();
                        return;
                    }
                    UNetworkIOPacket inputPacket = new UNetworkIOPacket(data);
                
                    UNetworkReadablePacket readablePacketTools = new UNetworkReadablePacket
                    {
                        Length = inputPacket.ReadUShort(),
                        Index = inputPacket.ReadUShort(),
                        PacketNumber = inputPacket.ReadUShort(),
                        BufferBytes = inputPacket.ReadBytes((ushort)(inputPacket.GetLength() - inputPacket.ReadPointer)),
                    };
                    
                    HandleData(readablePacketTools);
            
                    _udpClient.BeginReceive(CallBackReceive, null);
                }
                catch(Exception e)
                {
                    UNetworkLogs.ErrorReceivingUdp(e);
                    UncClient.Close();
                }
            }
        }
        public void HandleData(UNetworkReadablePacket packetTools) => UNetworkUpdate.AddToQueue(() => UncClient.RulesHandler.ExecuteRule(packetTools));
        public void SendData(byte[] data)
        {
            try
            {
                if (_udpClient != null && _endPoint != null)
                {
                    _udpClient.BeginSend(data, data.Length, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingUdp(e);
                UncClient.Close();
            }
        }

        public override void Close()
        {
            if (_isUdpConnect)
            {
                _isUdpConnect = false;
                
                _udpClient.Close();
                _endPoint = null;
            }
        }
    }
}