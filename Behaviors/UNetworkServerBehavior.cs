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

        public void StartServer(ushort serverId){}
        public void CloseServer(){}

        protected void OnStartServer(){}
        protected void OnCloseServer(){}
        public void OnConnectClient(ushort clientId){}
        public void OnDisconnectClient(ushort clientId){}
    }
}