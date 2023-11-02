using System;
using System.Net;
using Network.UnityServer.Behavior;
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

        public UNetworkServerProtocolUdpHandler(UNetworkClient unc) : base(unc) { }

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
                if (Client.NetworkServer.UdpListener != null && _endPoint != null)
                {
                    Client.NetworkServer.UdpListener.BeginSend(data, data.Length, _endPoint, null, null);
                }
            }
            catch (Exception e)
            {
                UNetworkLogs.ErrorSendingUdp(e);
                Client.Close();
            }
        }
        public void HandleData(UNetworkReadablePacket handlerPacket) => UNetworkUpdate.AddToQueue(() => Client.NetworkServer.RulesHandler.ExecuteRule(handlerPacket));
    }
}