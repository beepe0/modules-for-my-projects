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
        
        public void Create() { }
        public Task ConnectAsync() => Task.CompletedTask;
        public void Close() { }

        public virtual void OnCreate() { }
        public virtual void OnConnect() { }
        public virtual void OnClose() { }
    }
}