using Network.UNTools;

namespace Network.UClient
{
    public static class UNetworkIORules
    {
        public interface IGeneralRules
        {
            public void OnDisconnect();
        }
        public interface IInputRules
        {
            public void OnWelcome(UNetworkReadablePacket inputPacket);
        }
        
        public interface IOutputRules
        {
            public void OnWelcome();
        }
    }
}