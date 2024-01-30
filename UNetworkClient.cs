using Network.UnityServer.Handlers;

namespace Network.UnityServer
{
    public class UNetworkClient //must be extended by Monobehaviour
    {
        public ushort Index { get; private set; } //set should be internal
        public UNetworkRoom CurrentSession { get; set; } //set should be internal
        public UNetworkServer CurrentServer { get; private set; } //set should be internal
        public UNetworkClientProtocolTcpHandler TcpHandler { get; private set; } //set should be internal
        public UNetworkClientProtocolUdpHandler UdpHandler { get; private set; } //set should be internal

        public static TClient CreateInstance<TClient>(UNetworkServer server, ushort index) where TClient : UNetworkClient, new()
        {
            TClient client = new TClient();
            
            client.Index = index;
            client.CurrentServer = server;
            
            client.TcpHandler = new UNetworkClientProtocolTcpHandler(client);
            client.UdpHandler = new UNetworkClientProtocolUdpHandler(client);

            return client;
        }
        public TRoom GetCurrentSession<TRoom>() where TRoom : UNetworkRoom, new() => CurrentSession as TRoom;
        public TServer GetCurrentServer<TServer>() where TServer : UNetworkServer, new() => CurrentServer as TServer;
        
        public void Disconnect() //should be internal
        {
            if (TcpHandler is { IsTcpConnect: true } || UdpHandler is { IsUdpConnect: true }) OnDisconnectClient();
            
            if (TcpHandler != null) TcpHandler.Close();
            if (UdpHandler != null) UdpHandler.Close();
        }
        public virtual void OnConnectClient(){}
        public virtual void OnDisconnectClient(){}
    }
}