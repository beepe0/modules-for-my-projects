using Network.UnityTools;

namespace Network.UnityClient
{
    public sealed class UNetworkClientManager : UNetworkClient
    {
        private async void Awake() {
            if (dontDestroyOnLoad) DontDestroyOnLoad(this);
            if (startOnAwake) Create();
            if (connectOnAwake) await ConnectAsync();
        }
        private void FixedUpdate() => UNetworkUpdate.Update();
        private void OnApplicationQuit() => Close();
    }
}