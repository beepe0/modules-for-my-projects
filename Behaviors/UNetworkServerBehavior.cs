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
        public ushort slots = 20;
        public ushort slotsInRoom = 2;
        public ushort rooms = 10;
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;

        public abstract void StartServer<TClientType, TRoomType>(ushort serverId) where TRoomType : UNetworkRoom, new() where TClientType : UNetworkClient, new();
        public abstract void CloseServer();

        protected abstract void OnStartServer();
        protected abstract void OnCloseServer();
        public abstract void OnConnectClient(ushort clientId);
        public abstract void OnDisconnectClient(ushort clientId);
        public abstract void OnEntryToRoomClient(ushort clientId);
        public abstract void OnExitFromRoomClient(ushort clientId);
    }
}