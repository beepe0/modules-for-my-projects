using System;
using System.Net;
using Network.UnityServer.Behaviors;
using Network.UnityTools;

namespace Network.UnityServer.Handlers
{
    [Serializable]
    public sealed class UNetworkServerProtocolUdpHandler : UNetworkClientHandlerBehavior
    {
        private IPEndPoint _endPoint;
        private bool _isUdpConnect;

        public IPEndPoint EndPoint => _endPoint;
        public bool IsUdpConnect => _isUdpConnect;

        public UNetworkServerProtocolUdpHandler(UNetworkUser unc) : base(unc) { }

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
                if (User.NetworkServer.UdpListener != null && _endPoint != null)
                {
                    User.NetworkServer.UdpListener.BeginSend(data, data.Length, _endPoint, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingUdp(e);
                User.Close();
            }
        }
        public void HandleData(UNetworkReadablePacket handlerPacket) => UNetworkUpdate.AddToQueue(() => User.NetworkServer.RulesHandler.ExecuteRule(handlerPacket));
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