using Network.UnityTools;

namespace Network.UnityClient
{
    public static class UNetworkClientIORules
    {
        public interface IGeneralRules
        {
            public void OnDisconnect();
        }
        public interface IInputRules
        {
            public void HandShake(UNetworkReadablePacket packet);
        }
        
        public interface IOutputRules
        {
        }
    }
}