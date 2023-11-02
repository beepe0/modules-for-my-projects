﻿namespace Network.UnityServer.Behavior
{
    public abstract class UNetworkServerHandlerBehavior
    {
        protected UNetworkServer UncServer;
        public UNetworkServer Server => UncServer;

        protected UNetworkServerHandlerBehavior(UNetworkServer unc) => UncServer = unc;
        public virtual void Close() { }
    }
}