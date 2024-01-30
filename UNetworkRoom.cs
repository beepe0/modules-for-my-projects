using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.UnityServer
{
    public class UNetworkRoom //must be extended by Monobehaviour
    {
        public ushort Index { get; private set; }
        public bool IsOpened { get; private set; }
        public Dictionary<ushort, UNetworkClient> Clients { get; private set; }
        public UNetworkServer CurrentServer { get; private set; }
        
        public static TRoom CreateInstance<TRoom>(UNetworkServer server, ushort index, ushort slots) where TRoom : UNetworkRoom, new()
        {
            TRoom room = new TRoom(); 
            
            room.Index = index;
            room.IsOpened = false;
            room.Clients = new Dictionary<ushort, UNetworkClient>();
            room.CurrentServer = server;
            
            return room;
        }
        public TClient GetClient<TClient>(ushort index) where TClient : UNetworkClient, new() => Clients[index] as TClient;
        public TServer GetCurrentServer<TServer>() where TServer : UNetworkServer, new() => CurrentServer as TServer;

        public void Open() //should be internal //virtual test
        {
            if (IsOpened) return;
            
            IsOpened = true;
            OnCreateRoom();
            Clients.Clear();
        }
        public void Enter(ushort clientId) //should be internal
        {
            if (!IsOpened) return;
            
            OnEntryToRoomClient(clientId);
            CurrentServer.GetClient<UNetworkClient>(clientId).CurrentSession = this;
            Clients.Add(clientId, CurrentServer.GetClient<UNetworkClient>(clientId));
        }
        public void Exit(ushort clientId) //should be internal
        {
            if (!IsOpened) return;
            
            OnExitFromRoomClient(clientId);
            Clients.Remove(clientId);
        }
        public void Close() //should be internal //virtual test
        {
            if (!IsOpened) return;
            
            IsOpened = false;
            OnCloseRoom();
            Clients.Clear();
        }
        public virtual void OnCreateRoom(){}
        public virtual void OnCloseRoom(){}
        public virtual void OnEntryToRoomClient(ushort clientId){}
        public virtual void OnExitFromRoomClient(ushort clientId){}
    }
}