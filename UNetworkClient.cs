using UnityEngine;

namespace Network.UnityClient
{
    public class UNetworkClient : MonoBehaviour
    {
        public ushort Index { get; set; }
        public UNetworkRoom CurrentSession { get; set; } 
        public UNetworkConnection CurrentConnection { get; set; }
        
        public static TClient CreateInstance<TClient>(UNetworkConnection connection, ushort clientId, Transform parent) where TClient : UNetworkClient, new()
        {
            GameObject go = new GameObject();
            TClient client = go.AddComponent<TClient>();
            go.name = "client " + clientId;
            go.transform.parent = parent;
            client.Index = clientId;
            client.CurrentConnection = connection;

            return client;
        }
        public static TClient CreateInstance<TClient>(UNetworkConnection connection, ushort clientId, Transform parent, GameObject prefab) where TClient : UNetworkClient, new()
        {
            GameObject go = Instantiate(prefab, parent, true);
            TClient client;
            if (!go.TryGetComponent<TClient>(out client))
            {
                client = go.AddComponent<TClient>();
            }
            client.Index = clientId;
            client.CurrentConnection = connection;
            go.name = "client " + clientId;
            
            return client;
        }
        public TRoom GetCurrentSession<TRoom>() where TRoom : UNetworkRoom, new() => CurrentSession as TRoom;
        public TConnection GetCurrentConnection<TConnection>() where TConnection : UNetworkConnection, new() => CurrentConnection as TConnection;
    }
}