using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network.UnityServer.Handlers;
using Network.UnityTools;
using UnityEngine;

namespace Network.UnityServer
{
    public class UNetworkServer : MonoBehaviour
    {
        [Header("Configuration")] 
        public bool dontDestroyOnLoad;
        public bool startOnAwake;
        
        [Header("Connection settings")]
        public ushort serverPort = 34567;
        public string serverInternetProtocol = "127.0.0.1";

        [Header("Server settings")]
        public ushort slots = 20;
        public ushort slotsInRoom = 2;
        public ushort rooms = 10;
        
        [Header("Client settings")]
        public ushort receiveBufferSize = 512;
        public ushort sendBufferSize = 512;
        
        public ushort Index { get; private set; }
        public bool IsRunServer { get; private set; }
        public Dictionary<ushort, UNetworkClient> Clients { get; private set; }
        public Dictionary<ushort, UNetworkRoom> Rooms { get; private set; }
        public TcpListener TcpListener { get; private set; }
        public UdpClient UdpListener { get; private set; }
        public UNetworkServerRulesHandler RulesHandler { get; private set; }
        public UNetworkServerDataHandler DataHandler { get; private set; }

        public void StartServer<TRoom, TClient>(UNetworkServer server) where TRoom : UNetworkRoom, new() where TClient : UNetworkClient, new()
        {
            if (!IsRunServer)
            {
                Index = server.Index;
                Clients = new Dictionary<ushort, UNetworkClient>();
                Rooms = new Dictionary<ushort, UNetworkRoom>();
                RulesHandler = new UNetworkServerRulesHandler(server);
                DataHandler = new UNetworkServerDataHandler(server);
               
                for (ushort id = 0; id < slots + 1; id++)
                {
                    if(id == server.Index) continue;
                    Clients.Add(id, UNetworkClient.CreateInstance<TClient>(server, id));
                }
                for (ushort id = 0; id < rooms; id++)
                {
                    Rooms.Add(id, UNetworkRoom.CreateInstance<TRoom>(server, id, slotsInRoom));
                }
                
                TcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(serverInternetProtocol), serverPort));
                TcpListener.Start();
                TcpListener.BeginAcceptTcpClient(CallBackAcceptTcpClient, null);
                IsRunServer = true;
                UdpListener = new UdpClient(serverPort);
                UdpListener.BeginReceive(CallBackUdpReceive, null);
                OnStartServer();
            }
        }
        public void CloseServer()
        {
            if (IsRunServer)
            {
                OnCloseServer();

                IsRunServer = false;
                
                UdpListener.Close();
                TcpListener.Stop();
                
                if(RulesHandler.Rules != null) RulesHandler.Close();
                
                foreach (UNetworkClient networkClient in Clients.Values) networkClient.Disconnect();
                
                Clients.Clear();
            }
        }
        private void CallBackAcceptTcpClient(IAsyncResult asyncResult)
        {
            TcpClient cl = TcpListener.EndAcceptTcpClient(asyncResult);
            TcpListener.BeginAcceptTcpClient(CallBackAcceptTcpClient, null);

            foreach (UNetworkClient t in Clients.Values)
            {
                if(t.TcpHandler.TcpSocket == null) { t.TcpHandler.Connect(cl); return; }
            }
        }
        private void CallBackUdpReceive(IAsyncResult asyncResult)
        {
            if (IsRunServer)
            {
                try
                {
                    IPEndPoint epClient = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = UdpListener.EndReceive(asyncResult, ref epClient);
                    
                    if(data.Length < 4)
                    {
                        return;
                    }

                    UNetworkIOPacket inputPacket = new UNetworkIOPacket(data);
                    
                    UNetworkReadablePacket readablePacket = new UNetworkReadablePacket
                    {
                        Length = inputPacket.ReadUShort(),
                        Index = inputPacket.ReadUShort(),
                        PacketNumber = inputPacket.ReadUShort(),
                        BufferBytes = inputPacket.ReadBytes((ushort)(inputPacket.GetLength() - inputPacket.ReadPointer)),
                    };
                    
                    if(!Clients.ContainsKey(readablePacket.Index)) return;
             
                    var uNetworkClient = Clients[readablePacket.Index];

                    if (uNetworkClient.UdpHandler.EndPoint == null)
                    {
                        uNetworkClient.UdpHandler.Connect(epClient); 
                        uNetworkClient.UdpHandler.HandleData(readablePacket);
                    }
                    else if (uNetworkClient.UdpHandler.EndPoint.Equals(epClient))
                    {
                        uNetworkClient.UdpHandler.HandleData(readablePacket);
                    }

                    UdpListener.BeginReceive(CallBackUdpReceive, null);
                }
                catch (Exception e)
                {
                    UNetworkLogs.ErrorReceivingUdp(e);
                    CloseServer();
                }
            }
        }
        public TClient GetClient<TClient>(ushort index) where TClient : UNetworkClient, new() => Clients[index] as TClient;
        public TRoom GetRoom<TRoom>(ushort index) where TRoom : UNetworkRoom, new() => Rooms[index] as TRoom;

        protected virtual void OnStartServer(){}
        protected virtual void OnCloseServer(){}
    }
}