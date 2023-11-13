using System.Threading.Tasks;
using UnityEngine;

namespace Network.UnityClient.Behaviors
{
    public abstract class UNetworkClientBehavior : MonoBehaviour
    {
        [Header("Configuration")] 
        public bool dontDestroyOnLoad;
        public bool startOnAwake;
        public bool connectOnAwake;
        
        [Header("Connection settings")]
        public ushort serverPort = 34567;
        public string serverInternetProtocol = "127.0.0.1";
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;
        
        public void StartClient() { }
        public void ConnectClient() {}
        public void CloseClient() { }

        protected void OnStartClient() { }
        protected void OnConnectClient() { }
        protected void OnCloseClient() { }
    }
}