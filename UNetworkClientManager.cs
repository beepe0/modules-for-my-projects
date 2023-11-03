using Network.UnityTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.UnityClient
{
    public sealed class UNetworkClientManager : MonoBehaviour
    {
        [Header("Configuration")] 
        public bool dontDestroyOnLoad;
        
        [Header("Connection settings")]
        public ushort serverPort = 00000;
        public string serverInternetProtocol = "0.0.0.0";
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;

        private UNetworkClient _client = new();
        public UNetworkClient Client => _client;
        
        private void Awake() {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            _client.Start(this);
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => _client.Close();
    }
}