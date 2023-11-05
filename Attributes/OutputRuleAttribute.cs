using System;

namespace Network.UnityServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class OutputRuleAttribute : Attribute
    {
        public OutputRuleAttribute(UNetworkServer server)
        {
            
        }
    }
}