namespace Network.UnityServer.Behaviors
{
    public abstract class UNetworkServerHandlerBehavior
    {
        public UNetworkServer Server { get; private set; }

        protected UNetworkServerHandlerBehavior(UNetworkServer unc) => Server = unc;
        public virtual void Close() { }
    }
}