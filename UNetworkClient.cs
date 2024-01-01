using System.Threading.Tasks;
using Network.UnityClient.Behaviors;
using Network.UnityClient.Handlers;

namespace Network.UnityClient
{
    public abstract class UNetworkClient : UNetworkClientBehavior
    {
        private ushort _index;
        
        private UNetworkClientProtocolTcpHandler _tcpHandler;
        private UNetworkClientProtocolUdpHandler _udpHandler;
        private UNetworkClientRulesHandler _rulesHandler;
        private UNetworkClientDataHandler _dataHandler;

        public ushort Index
        {
            get => _index;
            set => _index = value;
        }

        public UNetworkClientProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkClientProtocolUdpHandler UdpHandler => _udpHandler;
        public UNetworkClientRulesHandler RulesHandler => _rulesHandler;
        public UNetworkClientDataHandler DataHandler => _dataHandler;
        
        public override void StartClient()
        {
            _tcpHandler = new UNetworkClientProtocolTcpHandler(this);
            _udpHandler = new UNetworkClientProtocolUdpHandler(this);
            _rulesHandler = new UNetworkClientRulesHandler(this);
            _dataHandler = new UNetworkClientDataHandler(this);
            OnStartClient();
        }

        public override void ConnectClient()
        {
            _tcpHandler.Connect();
            _udpHandler.Connect();
            OnConnectClient();
        }
        public override void CloseClient()
        {
            if (_tcpHandler is {IsTcpConnect: true} || _udpHandler is {IsUdpConnect: true}) OnCloseClient();
            
            if(_tcpHandler != null) _tcpHandler.Close();
            if(_udpHandler != null) _udpHandler.Close();
            if(_rulesHandler.Rules != null) _rulesHandler.Close();
        }
    }
}