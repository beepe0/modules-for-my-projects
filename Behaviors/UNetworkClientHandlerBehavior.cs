namespace Network.UnityServer.Behaviors
{
    public abstract class UNetworkClientHandlerBehavior
    {
        protected UNetworkUser UncUser;
        public UNetworkUser User => UncUser;

        protected UNetworkClientHandlerBehavior(UNetworkUser unc) => UncUser = unc;
        public virtual void Close() { }
    }
}