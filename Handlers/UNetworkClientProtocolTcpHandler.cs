using System;
using System.Net.Sockets;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public sealed class UNetworkClientProtocolTcpHandler : UNetworkClientHandlerBehavior
    {
        private TcpClient _tcpSocket;
        private NetworkStream _networkStream;
        private bool _isTcpConnect;
        private byte[] _receiveData;

        public TcpClient TcpClient => _tcpSocket;
        public NetworkStream NetworkStream => _networkStream;
        public bool IsTcpConnect => _isTcpConnect;
        public byte[] ReceiveData => _receiveData;

        public UNetworkClientProtocolTcpHandler(UNetworkClient unc) : base(unc){}
       
        public void Connect()
        {
            if (!_isTcpConnect)
            {
                _tcpSocket = new TcpClient();
                _tcpSocket.ReceiveBufferSize = UncClient.receiveBufferSize;
                _tcpSocket.SendBufferSize = UncClient.sendBufferSize;
        
                _receiveData = new byte[UncClient.receiveBufferSize];
          
                _tcpSocket.BeginConnect(UncClient.serverInternetProtocol, UncClient.serverPort, CallBackConnect, null);
            }
        }
        private void CallBackConnect(IAsyncResult asyncResult)
        {
            _tcpSocket.EndConnect(asyncResult);

            if (!_tcpSocket.Connected)
            {
                UncClient.Close();
                return;
            }

            _networkStream = _tcpSocket.GetStream();
            _networkStream.BeginRead(_receiveData, 0, UncClient.receiveBufferSize, CallBackReceive, null);
            
            _isTcpConnect = true;
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
                        UncClient.Close();
                        return;
                    }

                    HandleData(sizeData, _receiveData);
                    _receiveData = new byte[UncClient.receiveBufferSize];
                    _networkStream.BeginRead(_receiveData, 0, _receiveData.Length, CallBackReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingTcp(e);
                    UncClient.Close();
                }
            }
        }
        private void HandleData(int sizeData, byte[] data)
        {
            ushort readBytes = 0;
            UNetworkIOPacket packet = new UNetworkIOPacket(data);
            while (sizeData - 1 > packet.ReadPointer)
            {
                UNetworkReadablePacket handlerPacketTools = new UNetworkReadablePacket();
                handlerPacketTools.Length = packet.ReadUShort();
                handlerPacketTools.Index = packet.ReadUShort();
                handlerPacketTools.PacketNumber = packet.ReadUShort();
                handlerPacketTools.BufferBytes = packet.ReadBytes((ushort)((handlerPacketTools.Length + 2 + readBytes) - packet.ReadPointer));
                
                readBytes += (ushort)(handlerPacketTools.Length + 2);
                
                UNetworkUpdate.AddToQueue(() => UncClient.RulesHandler.ExecuteRule(handlerPacketTools)); 
            }
        }
        public void SendData(byte[] data)
        {
            try
            {
                if (_tcpSocket != null)  
                {
                    NetworkStream.BeginWrite(data, 0, data.Length, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingTcp(e);
                UncClient.Close();
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