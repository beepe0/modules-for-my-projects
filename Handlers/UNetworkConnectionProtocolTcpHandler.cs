using System;
using System.Net.Sockets;
using Network.UnityClient.Behaviors;
using Network.UnityTools;

namespace Network.UnityClient.Handlers
{
    public sealed class UNetworkConnectionProtocolTcpHandler : UNetworkConnectionHandlerBehavior
    {
        public TcpClient TcpClient { get; private set; }
        public NetworkStream NetworkStream { get; private set; }
        public bool IsTcpConnect { get; private set; }
        public byte[] ReceiveData { get; private set; }

        public UNetworkConnectionProtocolTcpHandler(UNetworkConnection unc) : base(unc){}
       
        public void Connect()
        {
            if (!IsTcpConnect)
            {
                TcpClient = new TcpClient();
                TcpClient.ReceiveBufferSize = Connection.receiveBufferSize;
                TcpClient.SendBufferSize = Connection.sendBufferSize;
        
                ReceiveData = new byte[Connection.receiveBufferSize];
          
                TcpClient.BeginConnect(Connection.serverInternetProtocol, Connection.serverPort, CallBackConnect, null);
            }
        }
        private void CallBackConnect(IAsyncResult asyncResult)
        {
            TcpClient.EndConnect(asyncResult);

            if (!TcpClient.Connected)
            {
                Connection.CloseClient();
                return;
            }

            NetworkStream = TcpClient.GetStream();
            NetworkStream.BeginRead(ReceiveData, 0, Connection.receiveBufferSize, CallBackReceive, null);
            
            IsTcpConnect = true;
        }
        private void CallBackReceive(IAsyncResult asyncResult)
        {
            if (IsTcpConnect)
            {
                try
                {
                    int sizeData = NetworkStream.EndRead(asyncResult);
            
                    if (sizeData < 4)
                    {
                        Connection.CloseClient();
                        return;
                    }

                    HandleData(sizeData, ReceiveData);
                    ReceiveData = new byte[Connection.receiveBufferSize];
                    NetworkStream.BeginRead(ReceiveData, 0, ReceiveData.Length, CallBackReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingTcp(e);
                    Connection.CloseClient();
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
                
                UNetworkUpdate.AddToQueue(() => Connection.RulesHandler.ExecuteRule(handlerPacketTools)); 
            }
        }
        public void SendData(byte[] data)
        {
            try
            {
                if (TcpClient != null)  
                {
                    NetworkStream.BeginWrite(data, 0, data.Length, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingTcp(e);
                Connection.CloseClient();
            }
        }

        public override void Close()
        {
            if (IsTcpConnect)
            {
                IsTcpConnect = false;
                
                TcpClient.Close();
                NetworkStream.Close();
                ReceiveData = null;
            }
        }
    }
}