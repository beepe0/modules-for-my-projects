using Network.UnityTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network.UnityClient
{
    public sealed class UNetworkClientManager : MonoBehaviour
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

        private UNetworkClient _client = new();
        public UNetworkClient Client => _client;
        
        private async void Awake() {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) _client.Start(this);
            if (connectOnAwake) await _client.ConnectAsync();
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => _client.Close();
    }
}