namespace Network.UnityClient.Behaviors
{
    public abstract class UNetworkConnectionHandlerBehavior
    {
        public UNetworkConnection Connection { get; private set; }

        protected UNetworkConnectionHandlerBehavior(UNetworkConnection unc) => Connection = unc;
        public virtual void Close() { }
    }
}