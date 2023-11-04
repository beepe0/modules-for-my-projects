using System.Threading.Tasks;
using Network.UnityClient.Handlers;

namespace Network.UnityClient
{
    public class UNetworkClient
    {
        private ushort _index;
        
        private UNetworkClientProtocolTcpHandler _tcpHandler;
        private UNetworkClientProtocolUdpHandler _udpHandler;
        private UNetworkClientRulesHandler _rulesHandler;
        private UNetworkClientDataHandler _dataHandler;

        public UNetworkClientIORules.IGeneralRules GeneralRules;
        public UNetworkClientIORules.IInputRules InputRules;
        public UNetworkClientIORules.IOutputRules OutputRules;

        private UNetworkClientManager _clientManager;

        public ushort Index => _index;
        public UNetworkClientManager ClientManager => _clientManager;
        public UNetworkClientProtocolTcpHandler TcpHandler => _tcpHandler;
        public UNetworkClientProtocolUdpHandler UdpHandler => _udpHandler;
        public UNetworkClientRulesHandler RulesHandler => _rulesHandler;
        public UNetworkClientDataHandler DataHandler => _dataHandler;
        
        public void Start(UNetworkClientManager clientManager)
        {
            _tcpHandler = new UNetworkClientProtocolTcpHandler(this);
            _udpHandler = new UNetworkClientProtocolUdpHandler(this);
            _rulesHandler = new UNetworkClientRulesHandler(this);
            _dataHandler = new UNetworkClientDataHandler(this);

            _clientManager = clientManager;
        }
        public async Task ConnectAsync()
        {
            await Task.Run(() => { while (_tcpHandler == null){} });
            _tcpHandler.Connect();
            await Task.Run(() => { while (_tcpHandler is {IsTcpConnect: false} || _udpHandler == null){} });
            _udpHandler.Connect();
            await Task.Run(() => { while (_udpHandler is {IsUdpConnect: false}){} });
        }
        public async Task WaitForConnection() => await Task.Run(() => { while (!(_tcpHandler is {IsTcpConnect: true}) || !( _udpHandler is {IsUdpConnect: true})){} });
        public void Close()
        {
            if (_tcpHandler is {IsTcpConnect: true} || _udpHandler is {IsUdpConnect: true}) GeneralRules.OnDisconnect();
            
            if(_tcpHandler != null) _tcpHandler.Close();
            if(_udpHandler != null) _udpHandler.Close();
            if(_rulesHandler.Rules != null) _rulesHandler.Close();
        }
    }
}
