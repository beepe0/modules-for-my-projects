using Network.UnityTools;

namespace Network.UnityServer
{
    public sealed class UNetworkServerManager : UNetworkServer
    {
        private void Awake() {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) Create();
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => Close(); 
    }
}