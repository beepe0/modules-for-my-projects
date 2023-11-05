using System;
using Network.UnityClient;
using Network.UnityServer;

namespace Network.UnityTools.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class OutputRuleAttribute : Attribute
    {
        public OutputRuleAttribute(UNetworkClient client)
        {
            
        }
        
        public OutputRuleAttribute(UNetworkServer server)
        {
            
        }
    }
}