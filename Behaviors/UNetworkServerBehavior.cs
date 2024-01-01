using UnityEngine;

namespace Network.UnityServer.Behaviors
{
    public abstract class UNetworkServerBehavior : MonoBehaviour
    {
        [Header("Configuration")] 
        public bool dontDestroyOnLoad;
        public bool startOnAwake;
        
        [Header("Connection settings")]
        public ushort serverPort = 34567;
        public string serverInternetProtocol = "127.0.0.1";

        [Header("Server settings")]
        public ushort slots = 2;
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;

        public abstract void StartServer(ushort serverId);
        public abstract void CloseServer();

        protected abstract void OnStartServer();
        protected abstract void OnCloseServer();
        public abstract void OnConnectClient(ushort clientId);
        public abstract void OnDisconnectClient(ushort clientId);
    }
}