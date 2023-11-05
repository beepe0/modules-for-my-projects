using System;
using System.Net.Sockets;
using Network.UnityServer.Behaviors;
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

        public UNetworkServerProtocolTcpHandler(UNetworkUser unc) : base(unc) { }
  
        public void Connect(TcpClient tcpSocket)
        {
            if (!_isTcpConnect)
            {
                _tcpSocket = tcpSocket;
                _tcpSocket.ReceiveBufferSize = User.NetworkServer.receiveBufferSize;
                _tcpSocket.SendBufferSize = User.NetworkServer.sendBufferSize;
        
                _networkStream = _tcpSocket.GetStream();
                _receiveData = new byte[User.NetworkServer.receiveBufferSize];
                _isTcpConnect = true;

                User.NetworkServer.OnConnectClient(User.Index);
                
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
                        User.Close();
                        return;
                    }

                    HandleData(sizeData, _receiveData);
                    _receiveData = new byte[User.NetworkServer.receiveBufferSize];
                    _networkStream.BeginRead(_receiveData, 0, _receiveData.Length, CallBackReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingTcp(e);
                    User.Close();
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

                UNetworkUpdate.AddToQueue(() => User.NetworkServer.RulesHandler.ExecuteRule(handlerPacket)); 
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
                User.Close();
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