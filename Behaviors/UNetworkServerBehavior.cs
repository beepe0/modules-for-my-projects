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
        
        public void StartServer() { }
        public void CloseServer() { }
        
        public virtual void OnStartServer() { }
        public virtual void OnCloseServer() { }
        public virtual void OnConnectClient(ushort clientId) { }
        public virtual void OnDisconnectClient(ushort clientId) { }
    }
}