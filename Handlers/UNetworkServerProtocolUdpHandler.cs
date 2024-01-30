using System;
using System.Net;
using Network.UnityServer.Behaviors;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    [Serializable]
    public sealed class UNetworkClientProtocolUdpHandler : UNetworkClientHandlerBehavior
    {
        private IPEndPoint _endPoint;
        private bool _isUdpConnect;

        public IPEndPoint EndPoint => _endPoint;
        public bool IsUdpConnect => _isUdpConnect;

        public UNetworkClientProtocolUdpHandler(UNetworkClient unc) : base(unc) { }

        public void Connect(IPEndPoint endPoint)
        {
            if (!_isUdpConnect)
            {
                _endPoint = endPoint;
                _isUdpConnect = true;
            }
        }
        public void SendData(byte[] data)
        {
            try
            {
                if (Client.CurrentServer.UdpListener != null && _endPoint != null)
                {
                    Client.CurrentServer.UdpListener.BeginSend(data, data.Length, _endPoint, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingUdp(e);
                Client.Disconnect();
            }
        }
        public void HandleData(UNetworkReadablePacket handlerPacket) => UNetworkUpdate.AddToQueue(() => Client.CurrentServer.RulesHandler.ExecuteRule(Client.Index, handlerPacket));
        public override void Close()
        {
            if (_isUdpConnect)
            {
                _isUdpConnect = false;
                _endPoint = null;
            }
        }
    }
}