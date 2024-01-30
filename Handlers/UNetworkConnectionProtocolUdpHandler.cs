using System;
using System.Net;
using System.Net.Sockets;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public sealed class UNetworkConnectionProtocolUdpHandler : UNetworkConnectionHandlerBehavior
    {
        private IPEndPoint _endPoint;
        public UdpClient UdpClient { get; private set; }
        public IPEndPoint EndPoint => _endPoint;
        public bool IsUdpConnect { get; private set; }

        public UNetworkConnectionProtocolUdpHandler(UNetworkConnection unc) : base(unc){}
        
        public void Connect()
        {
            if (!IsUdpConnect)
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(Connection.serverInternetProtocol), Connection.serverPort);
        
                UdpClient = new UdpClient();
                
                UdpClient.Client.SendBufferSize = Connection.sendBufferSize;
                UdpClient.Client.ReceiveBufferSize = Connection.receiveBufferSize;
            
                UdpClient.Connect(_endPoint);
                UdpClient.BeginReceive(CallBackReceive, null);

                IsUdpConnect = true;
            }
        }
        private void CallBackReceive(IAsyncResult asyncResult)
        {
            if (IsUdpConnect)
            {
                try
                {
                    byte[] data = UdpClient.EndReceive(asyncResult, ref _endPoint);

                    if (data.Length < 4)
                    {
                        Connection.CloseClient();
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
            
                    UdpClient.BeginReceive(CallBackReceive, null);
                }
                catch(Exception e)
                {
                    UNetworkLogs.ErrorReceivingUdp(e);
                    Connection.CloseClient();
                }
            }
        }
        public void HandleData(UNetworkReadablePacket packetTools) => UNetworkUpdate.AddToQueue(() => Connection.RulesHandler.ExecuteRule(packetTools));
        public void SendData(byte[] data)
        {
            try
            {
                if (UdpClient != null && _endPoint != null)
                {
                    UdpClient.BeginSend(data, data.Length, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingUdp(e);
                Connection.CloseClient();
            }
        }

        public override void Close()
        {
            if (IsUdpConnect)
            {
                IsUdpConnect = false;
                
                UdpClient.Close();
                _endPoint = null;
            }
        }
    }
}