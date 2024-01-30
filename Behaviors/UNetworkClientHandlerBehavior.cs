namespace Network.UnityServer.Behaviors
{
    public abstract class UNetworkClientHandlerBehavior
    {
        public UNetworkClient Client { get; private set; }

        protected UNetworkClientHandlerBehavior(UNetworkClient unc) => Client = unc;
        public virtual void Close() { }
    }
}