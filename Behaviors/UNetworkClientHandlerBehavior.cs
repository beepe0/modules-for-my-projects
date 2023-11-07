namespace Network.UnityServer.Behaviors
{
    public abstract class UNetworkClientHandlerBehavior
    {
        protected UNetworkClient UncClient;
        public UNetworkClient Client => UncClient;

        protected UNetworkClientHandlerBehavior(UNetworkClient unc) => UncClient = unc;
        public virtual void Close() { }
    }
}