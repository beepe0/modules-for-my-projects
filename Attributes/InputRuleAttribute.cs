using System;
using Network.UnityClient;
using Network.UnityServer;

namespace Network.UnityTools.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate)]
    public class InputRuleAttribute : Attribute
    {
        public InputRuleAttribute(UNetworkClient client)
        {
            
        }
        
        public InputRuleAttribute(UNetworkServer server)
        {
            
        }
    }
}