using System.Collections.Generic;
using Network.UnityClient.Behaviors;
using UnityEngine;

namespace Network.UnityClient
{
    public class UNetworkRoom : MonoBehaviour
    {
        public ushort Index { get; set; }
        public bool IsOpened { get; private set; }
        public Dictionary<ushort, UNetworkClient> Clients { get; private set; }
        public UNetworkConnection CurrentConnection { get; private set; }
        
        public static TRoom CreateInstance<TRoom>(UNetworkConnection connection, ushort roomId, Transform parent) where TRoom : UNetworkRoom, new()
        {
            GameObject go = new GameObject();
            TRoom room = go.AddComponent<TRoom>();
            go.name = "room " + roomId;
            go.transform.parent = parent;
            room.Index = roomId;
            room.Clients = new Dictionary<ushort, UNetworkClient>();
            room.CurrentConnection = connection;
            
            return room;
        }
        public static TRoom CreateInstance<TRoom>(UNetworkConnection connection, ushort roomId, Transform parent, GameObject prefab) where TRoom : UNetworkRoom, new()
        {
            GameObject go = Instantiate(prefab, parent, true);
            TRoom room;
            if (!go.TryGetComponent<TRoom>(out room))
            {
                room = go.AddComponent<TRoom>();
            }
            room.Index = roomId;
            room.CurrentConnection = connection;
            room.Clients = new Dictionary<ushort, UNetworkClient>();
            go.name = "room " + roomId;
            
            return room;
        }
        public TConnection GetCurrentServer<TConnection>() where TConnection : UNetworkConnection, new() => CurrentConnection as TConnection;
        public TClient GetClient<TClient>(ushort index) where TClient : UNetworkClient, new() => Clients[index] as TClient;
        public void Open() //should be internal //virtual test
        {
            if (IsOpened) return;
            
            IsOpened = true;
            //OnCreateRoom();
            Clients.Clear();
        }
        public void Enter(UNetworkClient client) //should be internal
        {
            if (!IsOpened) return;
            
            //OnEntryToRoomClient(clientId);
            client.CurrentSession = this;
            Clients.Add(client.Index, client);
        }
        public void Exit(ushort clientId) //should be internal
        {
            if (!IsOpened) return;
            
           // OnExitFromRoomClient(clientId);
            Clients.Remove(clientId);
        }
        public void Close() //should be internal //virtual test
        {
            if (!IsOpened) return;
            
            IsOpened = false;
           // OnCloseRoom();
            Clients.Clear();
        }
    }
}           