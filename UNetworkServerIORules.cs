using Network.UnityTools;

namespace Network.UnityServer
{
    public static class UNetworkServerIORules
    {
        public interface IGeneralRules
        {
            public void OnWelcome(ushort clientId);
            public void OnDisconnect(ushort clientId);
            public void OnClose();
        }
        public interface IInputRules
        {
        }
        
        public interface IOutputRules
        {
        }
    }
}