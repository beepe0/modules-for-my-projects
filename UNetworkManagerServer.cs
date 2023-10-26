using System.Threading.Tasks;
using UnityEngine;
using Network.UNTools;
using Singleton;

namespace Network.UServer
{
    public sealed class UNetworkManagerServer : Singleton<UNetworkManagerServer>
    {
        [Header("Connection settings")]
        public ushort serverPort = 12345;
        public string serverInternetProtocol = "127.0.0.1";

        [Header("Server settings")]
        public ushort slots = 2;
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;
        
        private async void Start()
        {
            await UNetworkCore.Start();
        }
        private void FixedUpdate()
        {
            UNetworkUpdate.Update();
        }
        private void OnApplicationQuit()
        {
            UNetworkCore.Close();
        }
        public static async Task WaitForInitialization() => await Task.Run(() => { while (Instance == null) { } });
    }
}