using System.Threading.Tasks;
using Network.UNTools;
using UnityEngine;
using Singleton;

namespace Network.UClient
{
    public sealed class UNetworkManagerClient : Singleton<UNetworkManagerClient>
    {
        [Header("Connection settings")]
        public ushort serverPort = 12345;
        public string serverInternetProtocol = "127.0.0.1";
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;
        
        private async void Start() => await UNetworkCore.StartAsync();
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => UNetworkCore.Close();
        public static async Task WaitForInitialization() => await Task.Run(() => { while (Instance == null) { } });
    }
}