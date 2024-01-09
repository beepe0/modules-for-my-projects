using System.Collections.Generic;
using Network.UnityServer.Behaviors;

namespace Network.UnityServer
{
    public class UNetworkRoom
    {
        private ushort _index;
        
        private Dictionary<ushort, UNetworkClient> _clients = new Dictionary<ushort, UNetworkClient>();
        private UNetworkServer _networkServer;
        
        public ushort Index => _index;
        public UNetworkServer NetworkServer => _networkServer;
        public Dictionary<ushort, UNetworkClient> Clients => _clients;
        public static T CreateInstance<T>(UNetworkServer server, ushort index, ushort slots) where T : UNetworkRoom, new()
        {
            T room = new T();
            
            room._index = index;
            room._networkServer = server;
            
            return room;
        }
    }
}