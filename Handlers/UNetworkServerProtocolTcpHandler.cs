using System;
using System.Net.Sockets;
using Network.UnityServer.Behavior;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    [Serializable]
    public sealed class UNetworkServerProtocolTcpHandler : UNetworkClientHandlerBehavior
    {
        private TcpClient _tcpSocket;
        private NetworkStream _networkStream;
        private bool _isTcpConnect;
        private byte[] _receiveData;

        public TcpClient TcpSocket => _tcpSocket;
        public NetworkStream NetworkStream => _networkStream;
        public bool IsTcpConnect => _isTcpConnect;
        public byte[] ReceiveData => _receiveData;

        public UNetworkServerProtocolTcpHandler(UNetworkClient unc) : base(unc) { }
  
        public void Connect(TcpClient tcpSocket)
        {
            if (!_isTcpConnect)
            {
                _tcpSocket = tcpSocket;
                _tcpSocket.ReceiveBufferSize = Client.NetworkServer.ServerManager.receiveBufferSize;
                _tcpSocket.SendBufferSize = Client.NetworkServer.ServerManager.sendBufferSize;
        
                _networkStream = _tcpSocket.GetStream();
                _receiveData = new byte[Client.NetworkServer.ServerManager.receiveBufferSize];
                _isTcpConnect = true;

                Client.NetworkServer.GeneralRules.OnWelcome(Client.Index);
                
                _networkStream.BeginRead(_receiveData, 0, _receiveData.Length, CallBackReceive, null);
            }
        }
        private void CallBackReceive(IAsyncResult asyncResult)
        {
            if (_isTcpConnect)
            {
                try
                {
                    int sizeData = _networkStream.EndRead(asyncResult);

                    if (sizeData < 4)
                    {
                        Client.Close();
                        return;
                    }

                    HandleData(sizeData, _receiveData);
                    _receiveData = new byte[Client.NetworkServer.ServerManager.receiveBufferSize];
                    _networkStream.BeginRead(_receiveData, 0, _receiveData.Length, CallBackReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingTcp(e);
                    Client.Close();
                }
            }
        }
        private void HandleData(int sizeData, byte[] data)
        {
            UNetworkIOPacket packet = new UNetworkIOPacket(data);
            while (sizeData - 1 > packet.ReadPointer)
            {
                UNetworkReadablePacket handlerPacket = new UNetworkReadablePacket
                {
                    Length =  packet.ReadUShort(),
                    Index = packet.ReadUShort(),
                    PacketNumber = packet.ReadUShort(),
                    BufferBytes = packet.ReadBytes((ushort)(sizeData - packet.ReadPointer))
                };

                UNetworkUpdate.AddToQueue(() => Client.NetworkServer.RulesHandler.ExecuteRule(handlerPacket)); 
            }
        }
        public void SendData(byte[] data)
        {
            try
            {
                if (_tcpSocket != null)
                {
                    _networkStream.BeginWrite(data, 0, data.Length, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingTcp(e);
                Client.Close();
            }
        }
        public override void Close()
        {
            if (_isTcpConnect)
            {
                _isTcpConnect = false;
                
                _tcpSocket.Close();
                _networkStream.Close();
                _receiveData = null;
            }
        }
    }
}