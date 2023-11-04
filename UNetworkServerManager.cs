using Network.UnityTools;
using UnityEngine;

namespace Network.UnityServer
{
    public sealed class UNetworkServerManager : MonoBehaviour
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

        private UNetworkServer _server = new();
        public UNetworkServer Server => _server;

        private void Awake() {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) _server.Start(this);
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => _server.Close(); 
    }
}