﻿using UnityEngine;

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

        public abstract void StartClient();
        public abstract void ConnectClient();
        public abstract void CloseClient();

        protected abstract void OnStartClient();
        protected abstract void OnConnectClient();
        protected abstract void OnCloseClient();
    }
}