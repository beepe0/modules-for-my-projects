using System.Threading.Tasks;
using Network.UnityClient.Behaviors;
using Network.UnityClient.Handlers;

namespace Network.UnityClient
{
    public class UNetworkClient : UNetworkClientBehavior
    {
        private ushort _index;
        
        private UNetworkClientProtocolTcpHandler _tcpHandler;
        private UNetworkClientProtocolUdpHandler _udpHandler;
        private UNetworkClientRulesHandler _rulesHandler;
        private UNetworkClientDataHandler _dataHandler;

        private UNetworkClientManager _clientManager;

        public ushort Index => _index;
        public UNetworkClientManager ClientManager => _clientManager;
        public UNetworkClientProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkClientProtocolUdpHandler UdpHandler => _udpHandler;
        public UNetworkClientRulesHandler RulesHandler => _rulesHandler;
        public UNetworkClientDataHandler DataHandler => _dataHandler;
        
        public new void Create()
        {
            _tcpHandler = new UNetworkClientProtocolTcpHandler(this);
            _udpHandler = new UNetworkClientProtocolUdpHandler(this);
            _rulesHandler = new UNetworkClientRulesHandler(this);
            _dataHandler = new UNetworkClientDataHandler(this);
        }
        public new async Task ConnectAsync()
        {
            await Task.Run(() => { while (_tcpHandler == null){} });
            _tcpHandler.Connect();
            await Task.Run(() => { while (_tcpHandler is {IsTcpConnect: false} || _udpHandler == null){} });
            _udpHandler.Connect();
            await Task.Run(() => { while (_udpHandler is {IsUdpConnect: false}){} });
        }
        public new void Close()
        {
            if (_tcpHandler is {IsTcpConnect: true} || _udpHandler is {IsUdpConnect: true}) OnClose();
            
            if(_tcpHandler != null) _tcpHandler.Close();
            if(_udpHandler != null) _udpHandler.Close();
            if(_rulesHandler.Rules != null) _rulesHandler.Close();
        }
    }
}
