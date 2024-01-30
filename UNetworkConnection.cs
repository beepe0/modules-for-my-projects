using System.Collections.Generic;
using System.Threading.Tasks;
using Network.UnityClient.Behaviors;
using Network.UnityClient.Handlers;
using UnityEngine;

namespace Network.UnityClient
{
    public class UNetworkConnection : MonoBehaviour
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
        
        public ushort Index { get; set; }
        public UNetworkRoom CurrentSession { get; set; } 
        public UNetworkConnectionProtocolTcpHandler TcpHandler { get; private set; }
        public UNetworkConnectionProtocolUdpHandler UdpHandler { get; private set; }
        public UNetworkConnectionRulesHandler RulesHandler { get; private set; }
        public UNetworkConnectionDataHandler DataHandler { get; private set; }
        
        public TRoom GetCurrentSession<TRoom>() where TRoom : UNetworkRoom, new() => CurrentSession as TRoom;
        public void StartClient()
        {
            TcpHandler = new UNetworkConnectionProtocolTcpHandler(this);
            UdpHandler = new UNetworkConnectionProtocolUdpHandler(this);
            RulesHandler = new UNetworkConnectionRulesHandler(this);
            DataHandler = new UNetworkConnectionDataHandler(this);
            OnStartClient();
        }
        public void ConnectClient()
        {
            TcpHandler.Connect();
            UdpHandler.Connect();
            OnConnectClient();
        }
        public void CloseClient()
        {
            if (TcpHandler is {IsTcpConnect: true} || UdpHandler is {IsUdpConnect: true}) OnCloseClient();
            
            if(TcpHandler != null) TcpHandler.Close();
            if(UdpHandler != null) UdpHandler.Close();
            if(RulesHandler.Rules != null) RulesHandler.Close();
        }
        protected virtual void OnStartClient(){}
        protected virtual void OnConnectClient(){}
        protected virtual void OnCloseClient(){}
    }
}